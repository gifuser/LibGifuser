using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Gifuser.Core
{
    public class GifRecorder : IScreenRecorder
    {
        private class GifScreenRecorderData
        {
            public string FileName
            {
                get;
                set;
            }

            public TimeSpan Delay
            {
                get;
                set;
            }

            public int Frames
            {
                get;
                set;
            }
        }

        private bool _recording;
        private TimeSpan _delay;
        private readonly BackgroundWorker _worker;
        private readonly Stopwatch _watch;

        public event EventHandler<ScreenRecordCompletedEventArgs> Completed;
        protected virtual void OnCompleted(object sender, ScreenRecordCompletedEventArgs e)
        {
            if (Completed != null)
            {
                Completed(this, e);
            }
        }

        public event EventHandler<ScreenshotTakenEventArgs> ScreenshotTaken;
        protected virtual void OnScreenshotTaken(object sender, ScreenshotTakenEventArgs e)
        {
            if (ScreenshotTaken != null)
            {
                ScreenshotTaken(this, e);
            }
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            GifScreenRecorderData data = (GifScreenRecorderData)(e.Argument);
			
            int delay = (int)(data.Delay.TotalMilliseconds);
			IntPtr screenRecord = IntPtr.Zero;

			try
			{	
				screenRecord = NativeMethods.beginScreenRecord(data.FileName, ToGifDelay(data.Delay));

				while (!_worker.CancellationPending)
				{
					NativeMethods.captureScreen(screenRecord);

                    Thread.Sleep(delay);

					data.Frames++;
					_worker.ReportProgress(0, data);
				}
			}
			finally
			{
				if (screenRecord != IntPtr.Zero)
				{
					NativeMethods.endScreenRecord(screenRecord);
				}
			}

            e.Result = data;
        }

        private void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GifScreenRecorderData data = (GifScreenRecorderData)(e.UserState);

            FileInfo info = new FileInfo(data.FileName);

            OnScreenshotTaken(this, new ScreenshotTakenEventArgs(info.Length, data.Frames));
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _watch.Stop();
            _recording = false;

			if (e.Error == null)
			{
				GifScreenRecorderData data = (GifScreenRecorderData)(e.Result);
				
				OnCompleted(
					this,
					new ScreenRecordCompletedEventArgs(
						data.FileName,
						data.Delay,
						data.Frames
					)
				);
			}
			else
			{
				OnCompleted(
					this,
					new ScreenRecordCompletedEventArgs(e.Error)
				);
			}

            _watch.Reset();
        }

        public GifRecorder()
        {
            _disposedValue = false;
            _recording = false;
            _watch = new Stopwatch();
            _watch.Reset();
            _worker = new BackgroundWorker();
            _delay = TimeSpan.FromMilliseconds(100);
            _worker.WorkerSupportsCancellation = true;
            _worker.WorkerReportsProgress = true;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _worker.ProgressChanged += _worker_ProgressChanged;
        }

        public bool Recording
        {
            get
            {
                return _recording;
            }
        }

        private static ushort ToGifDelay(TimeSpan delay)
        {
            return (ushort)(delay.TotalMilliseconds / 10.0);
        }

        public TimeSpan Delay
        {
            get
            {
                return _delay;
            }
            set
            {
                if (ToGifDelay(value) > ushort.MaxValue)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _delay = value;
            }
        }

        public void StartAsync(string fileName, bool allowOverwrite)
        {
			if (_recording || _worker.IsBusy)
			{
				throw new Exception("Already recording");
			}

			_recording = true;
			_watch.Start();

			try
			{
				string newFileName;
				if (fileName == null)
				{
					string tmpDir = Path.GetTempPath();
					newFileName = Path.Combine(tmpDir, string.Format("{0}{1}{2}", "gifuser-", Guid.NewGuid().ToString("N"), ".gif"));
				}
				else
				{
					if (File.Exists(fileName))
					{
						if (allowOverwrite)
						{
							File.Delete(fileName);
						}
						else
						{
							throw new ArgumentException("file already exists and overwrite is not allowed");
						}
					}

					newFileName = fileName;
				}

				GifScreenRecorderData data = new GifScreenRecorderData();
				data.FileName = newFileName;
				data.Delay = _delay;
				data.Frames = 0;

				_worker.RunWorkerAsync(data);
			}
			catch
			{
				_recording = false;
				_watch.Stop();
				_watch.Reset();
				throw;
			}
        }

        public void FinishAsync()
        {
			if (_recording && _worker.IsBusy && !_worker.CancellationPending)
			{
				_worker.CancelAsync();
			}
			else if (_worker.CancellationPending)
			{
				throw new Exception("Cancellation already requested");
			}
			else
			{
				throw new Exception("Not recording");
			}
        }

        public TimeSpan Elapsed
        {
            get
            {
                return _watch.Elapsed;
            }
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _worker.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GifRecorder() {
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
