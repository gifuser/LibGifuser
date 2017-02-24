using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Gifuser.Upload;
using GiphyPlugin.Models.Get;
using GiphyPlugin.Models.Upload;
using Newtonsoft.Json;

namespace GiphyPlugin
{
	public class GiphyFileTrackedUpload : MultipartFormDataUpload
	{
		private readonly string _apiKey;
		private readonly string _username;

		public GiphyFileTrackedUpload(string apiKey, string userName)
			: base(new Uri("http://upload.giphy.com/v1/gifs"))
		{
			if (apiKey == null)
			{
				throw new ArgumentNullException("apiKey");
			}

			_apiKey = apiKey;
			_username = userName;
		}

		public GiphyFileTrackedUpload(string apiKey)
			: this(apiKey, null)
		{

		}

		public override long MaxFileSize
		{
			get
			{
				return 100L * 1024L * 1024L;
			}
		}

		public override string Name
		{
			get
			{
				return "giphy";
			}
		}

		public override string Url
		{
			get
			{
				return "http://www.giphy.com";
			}
		}
        
		protected override void ConfigureRequest(HttpWebRequest request, object state)
		{
			request.Accept = "application/json; charset=UTF-8";
			request.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
		}

		private string RequestGifLink(UploadResponseDataModel data)
		{
			string targetUrl = string.Format("http://api.giphy.com/v1/gifs/{0}?api_key={1}", data.Id, _apiKey);

			using (WebClient client = new WebClient())
			{
				GetByIdResponseModel result = JsonConvert.DeserializeObject<GetByIdResponseModel>(
					client.DownloadString(targetUrl)
				);

				if (result == null || result.Data == null || result.Meta == null)
				{
					throw new WebException("Invalid get gif by id data");
				}
				else if (result.Meta.Status == HttpStatusCode.OK)
				{
					return result.Data.Url;
				}
				else
				{
					throw new WebException("Unsuccessful get gif by id");
				}
			}
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
							UploadResponseModel result = (UploadResponseModel)(serializer.Deserialize(reader, typeof(UploadResponseModel)));

							if (result == null || result.Data == null || result.Meta == null)
							{
								throw new WebException("Invalid response data");
							}
							else if (result.Meta.Status == HttpStatusCode.OK)
							{
								return RequestGifLink(result.Data);
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
			fields.Add("api_key", _apiKey);

			if (_username != null)
			{
				fields.Add("username", _username);
			}
		}
    }
}
