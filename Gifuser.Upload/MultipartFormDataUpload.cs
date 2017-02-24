using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Gifuser.Upload
{
	public abstract class MultipartFormDataUpload : IFileTrackedUpload
	{
		private const int CHUNK_SIZE = 32 * 1024; // 32 kb

		private bool _isUploading;
		private readonly int _chunkSize;
		private readonly BackgroundWorker _worker;
		private readonly Uri _uploadUri;

		public MultipartFormDataUpload(Uri uploadUri, int chunkSize)
		{
			if (uploadUri == null)
			{
				throw new ArgumentNullException("uploadUri");
			}

			if (chunkSize <= 0)
			{
				throw new ArgumentOutOfRangeException("chunkSize");
			}

			_uploadUri = uploadUri;
			_chunkSize = chunkSize;

			_isUploading = false;
			_worker = new BackgroundWorker();
			_worker.WorkerReportsProgress = true;
			_worker.WorkerSupportsCancellation = true;
			_worker.DoWork += _worker_DoWork;
			_worker.ProgressChanged += _worker_ProgressChanged;
			_worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
		}

		public MultipartFormDataUpload(Uri uploadUri)
			: this(uploadUri, CHUNK_SIZE)
		{

		}

		private void WriteBytes(byte[] data, byte[] buffer, RequestState state, Stream requestStream)
		{
			if (!_worker.CancellationPending)
			{
				int bytesToWrite = 0;

				if (data.Length > buffer.Length)
				{
					for (int offset = 0; !_worker.CancellationPending && offset < data.Length; offset += buffer.Length)
					{
						bytesToWrite = Math.Min(buffer.Length, data.Length - offset);
						state.Offset += bytesToWrite;

						requestStream.Write(data, offset, bytesToWrite);
						FireReportProgress(state);
					}
				}
				else
				{
					bytesToWrite = data.Length;
					state.Offset += bytesToWrite;

					requestStream.Write(data, 0, bytesToWrite);
					FireReportProgress(state);
				}
			}
		}

		private void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
			RequestState state = (RequestState)(e.Argument);

			state.Offset = 0;

			HttpWebRequest request = (HttpWebRequest)(WebRequest.Create(_uploadUri));

			string boundary = "-------------------------------" + DateTime.Now.Ticks.ToString("X", NumberFormatInfo.InvariantInfo);

			request.Method = "POST";
			request.ContentType = "multipart/form-data; boundary=" + boundary;

			object extensionState = CreateState();

			try
			{
				ConfigureRequest(request, extensionState);

				string formFieldFormat = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n";

				NameValueCollection fields = new NameValueCollection();

				ConfigureFormFields(fields, extensionState);

				byte[] formFieldBytes = null;
				using (MemoryStream m = new MemoryStream())
				{
					foreach (string key in fields)
					{
						string formFieldValue = string.Format(
							formFieldFormat,
							boundary,
							key,
							fields[key]
						);

						byte[] currentFormFieldBytes = Encoding.UTF8.GetBytes(formFieldValue);

						m.Write(currentFormFieldBytes, 0, currentFormFieldBytes.Length);
					}

					formFieldBytes = m.ToArray();
				}

				long fileSize;
				using (FileStream fs = File.OpenRead(state.FileName))
				{
					fileSize = fs.Length;
				}

				string fileHeaderFormat = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: application/octet-stream\r\n\r\n";

				string fileHeaderValue = string.Format(
					fileHeaderFormat,
					boundary,
					FileFormFieldName(extensionState),
                    FileFormFieldFileName(state.FileName, extensionState)
				);

				byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeaderValue);

				string boundaryFormat = "\r\n--{0}--\r\n";

				string boundaryValue = string.Format(boundaryFormat, boundary);

				byte[] boundaryBytes = Encoding.ASCII.GetBytes(boundaryValue);

				long totalBytes = formFieldBytes.Length + fileHeaderBytes.Length + fileSize + boundaryBytes.Length;

				state.TotalBytes = totalBytes;

				if (ReportsProgress)
				{
					request.SendChunked = true;
				}
				else
				{
					request.ContentLength = totalBytes;
				}

				byte[] buffer = new byte[_chunkSize];

				if (buffer.Length >= totalBytes)
				{
					int destinationIndex = 0;

					Array.Copy(formFieldBytes, 0, buffer, destinationIndex, formFieldBytes.Length);
					destinationIndex += formFieldBytes.Length;

					Array.Copy(fileHeaderBytes, 0, buffer, destinationIndex, fileHeaderBytes.Length);
					destinationIndex += fileHeaderBytes.Length;

					using (FileStream stream = File.OpenRead(state.FileName))
					{
						int streamSize = (int)(stream.Length);
						stream.Read(buffer, destinationIndex, streamSize);
						destinationIndex += streamSize;
					}

					Array.Copy(boundaryBytes, 0, buffer, destinationIndex, boundaryBytes.Length);
					destinationIndex += boundaryBytes.Length;

					if (destinationIndex != totalBytes)
					{
						throw new Exception("total bytes mismatch");
					}

					using (Stream requestStream = request.GetRequestStream())
					{
						state.Offset = destinationIndex;

						requestStream.Write(buffer, 0, destinationIndex);
						FireReportProgress(state);
					}
				}
				else
				{
					using (Stream requestStream = request.GetRequestStream())
					{
						WriteBytes(formFieldBytes, buffer, state, requestStream);
						WriteBytes(fileHeaderBytes, buffer, state, requestStream);
						
						using (FileStream stream = File.OpenRead(state.FileName))
						{
							int bytesRead = 0;
							while (!_worker.CancellationPending && (bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
							{
								state.Offset += bytesRead;
								
								requestStream.Write(buffer, 0, bytesRead);
								FireReportProgress(state);
							}
						}
						
						if (_worker.CancellationPending)
						{
							request.Abort();
						}
						else
						{
							WriteBytes(boundaryBytes, buffer, state, requestStream);
						}
					}
				}

				if (_worker.CancellationPending)
				{
					e.Cancel = true;
				}
				else
				{
					HttpWebResponse response = (HttpWebResponse)(request.GetResponse());

					state.Link = GetLink(response, extensionState);
					e.Result = state;
				}
			}
			finally
			{
				if (extensionState != null && extensionState is IDisposable)
				{
					IDisposable disposableState = (IDisposable)extensionState;
					if (disposableState != null)
					{
						disposableState.Dispose();
					}
				}
			}
		}

		private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			RequestState state = (RequestState)(e.UserState);
			int percent = (int)((state.Offset * 100L) / state.TotalBytes);
			OnProgress(new UploadProgressEventArgs(percent, state.UserState));
		}

		private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			string link = null;
			object userState = null;

			try
			{
				if (!e.Cancelled && e.Error == null)
				{
					RequestState state = (RequestState)(e.Result);
					link = state.Link;
					userState = state.UserState;
				}

				OnCompleted(new UploadCompletedEventArgs(e.Cancelled, link, userState, e.Error));
			}
			finally
			{
				_isUploading = false;
			}
		}


		public abstract long MaxFileSize { get; }

		public abstract string Name { get; }

		public virtual bool ReportsProgress
		{
			get
			{
				return true;
			}
		}

		public abstract string Url { get; }


		public event EventHandler<UploadCompletedEventArgs> Completed;
		protected virtual void OnCompleted(UploadCompletedEventArgs e)
		{
			if (Completed != null)
			{
				Completed(this, e);
			}
		}

		public event EventHandler<UploadProgressEventArgs> Progress;
		protected virtual void OnProgress(UploadProgressEventArgs e)
		{
			if (Progress != null)
			{
				Progress(this, e);
			}
		}

		private void FireReportProgress(RequestState state)
		{
			if (ReportsProgress)
			{
				_worker.ReportProgress(0, state);
			}
		}

		public void CancelAsync()
		{
			if (_isUploading)
			{
				_worker.CancelAsync();
			}
			else
			{
				throw new Exception("There is not an upload in progress");
			}
		}

		public UploadRequirementStatus CheckRequirementStatus(string fileName)
		{
			UploadRequirementStatus status;

			if (fileName == null)
			{
				status = UploadRequirementStatus.FileNull;
			}
			else if (!File.Exists(fileName))
			{
				status = UploadRequirementStatus.FileNotFound;
			}
			else
			{
				long size;
				using (FileStream stream = File.OpenRead(fileName))
				{
					size = stream.Length;
				}

				if (size > MaxFileSize)
				{
					status = UploadRequirementStatus.FileTooLarge;
				}
				else
				{
					status = UploadRequirementStatus.Success;
				}
			}

			return status;
		}

		private class RequestState
		{
			public string FileName
			{
				get;
				set;
			}

			public long TotalBytes
			{
				get;
				set;
			}

			public long Offset
			{
				get;
				set;
			}

			public object UserState
			{
				get;
				set;
			}

			public string Link
			{
				get;
				set;
			}
		}

		public void StartAsync(string fileName)
		{
			StartAsync(fileName, null);
		}

		protected virtual object CreateState()
		{
			return null;
		}
		protected virtual void ConfigureRequest(HttpWebRequest request, object state)
        {

        }
		protected virtual string FileFormFieldName(object state)
        {
            return "file";
        }
        protected virtual string FileFormFieldFileName(string fileName, object state)
        {
            return Path.GetFileName(fileName);
        }
		protected virtual void ConfigureFormFields(NameValueCollection fields, object state)
		{
			
		}
		protected abstract string GetLink(HttpWebResponse response, object state);

		public void StartAsync(string fileName, object userState)
		{
			if (_isUploading)
			{
				throw new Exception("There is an upload in progress");
			}
			else
			{
				UploadRequirementStatus status = CheckRequirementStatus(fileName);

				if (status != UploadRequirementStatus.Success)
				{
					throw new Exception("Upload does not meet requirements");
				}

				_isUploading = true;

				RequestState state = new RequestState
				{
					FileName = fileName,
					UserState = userState
				};

				_worker.RunWorkerAsync(state);
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					_worker.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~ImgurUploadService() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion

	}
}
