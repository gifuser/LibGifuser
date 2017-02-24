using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Gifuser.Upload;
using Newtonsoft.Json;

namespace ImgurPlugin
{
	public class ImgurFileTrackedUpload : MultipartFormDataUpload
	{
		private readonly string _clientId;

		public ImgurFileTrackedUpload(string clientId)
			: base(new Uri("https://api.imgur.com/3/image"))
		{
			if (clientId == null)
			{
				throw new ArgumentNullException("clientId");
			}

			_clientId = clientId;
		}

		public override long MaxFileSize
		{
			get
			{
				return 10L * 1024L * 1024L;
			}
		}

		public override string Name
		{
			get
			{
				return "imgur";
			}
		}

		public override string Url
		{
			get
			{
				return "https://www.imgur.com";
			}
		}

		protected override string FileFormFieldName(object state)
		{
			return "image";
		}

		protected override void ConfigureRequest(HttpWebRequest request, object state)
		{
			request.Accept = "application/json; charset=UTF-8";
			request.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
			request.Headers[HttpRequestHeader.Authorization] = "Client-ID " + _clientId;
		}

		protected override string GetLink(HttpWebResponse response, object state)
		{
			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
				case HttpStatusCode.Created:
					{
						using (Stream responseStream = response.GetResponseStream())
						using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
						{
							JsonSerializer serializer = new JsonSerializer();
							ResponseModel result = (ResponseModel)(serializer.Deserialize(reader, typeof(ResponseModel)));

							if (result == null || result.Data == null)
							{
								throw new WebException("Invalid response data");
							}
							else if (result.Success)
							{
								return result.Data.Link;
							}
							else
							{
								throw new WebException("Unsuccessful upload");
							}
						}
					}
				default:
					{
						throw new WebException("Upload failed");
					}
			}
		}

		protected override void ConfigureFormFields(NameValueCollection fields, object state)
		{
			fields.Add("type", "file");
		}
    }
}
