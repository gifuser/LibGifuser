using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using GfycatPlugin.Models;
using Gifuser.Upload;
using Newtonsoft.Json;

namespace GfycatPlugin
{
	public class GfycatFileTrackedUpload : MultipartFormDataUpload
	{
		private const string TOKEN_ENDPOINT = "https://api.gfycat.com/v1/oauth/token";
		private const string GFYCATS_ENDPOINT = "https://api.gfycat.com/v1/gfycats";
		private const string STATUS_ENDPOINT = "https://api.gfycat.com/v1/gfycats/fetch/status";
		
		private readonly string _clientId;
		private readonly string _clientSecret;

		public GfycatFileTrackedUpload(string clientId, string clientSecret)
			: base(new Uri("https://filedrop.gfycat.com"))
		{
			if (clientId == null)
			{
				throw new ArgumentNullException("clientId");
			}

			if (clientSecret == null)
			{
				throw new ArgumentNullException("clientSecret");
			}

			_clientId = clientId;
			_clientSecret = clientSecret;
		}

		public override long MaxFileSize
		{
			get
			{
				return 300L * 1024L * 1024L;
			}
		}

		public override string Name
		{
			get
			{
				return "gfycat";
			}
		}

		public override string Url
		{
			get
			{
				return "https://gfycat.com";
			}
		}

		public override bool ReportsProgress
		{
			get
			{
				return false;
			}
		}
        
		private class GfycatState
		{
			public AuthResponseModel Auth
			{ 
				get;
				set;
			}

			public CreateFileResponseModel File
			{
				get;
				set;
			}
		}

		protected override object CreateState()
		{
			HttpWebRequest request = (HttpWebRequest)(WebRequest.Create(TOKEN_ENDPOINT));
			request.Method = "POST";
			request.ContentType = "application/json; charset=UTF-8";
			request.Accept = "application/json; charset=UTF-8";
			request.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";

			JsonSerializer serializer = new JsonSerializer();

			using (Stream reqStream = request.GetRequestStream())
			using (StreamWriter writer = new StreamWriter(reqStream, Encoding.UTF8))
			{
				serializer.Serialize(
					writer,
					new AuthRequestModel
					{
						GrantType = "client_credentials",
						ClientId = _clientId,
						ClientSecret = _clientSecret
					}
				);
			}

			HttpWebResponse response = (HttpWebResponse)(request.GetResponse());

			if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
			{
				using (Stream respStream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(respStream, Encoding.UTF8))
				{
					AuthResponseModel auth = (AuthResponseModel)(serializer.Deserialize(reader, typeof(AuthResponseModel)));

					if (auth == null || auth.AccessToken == null)
					{
						throw new Exception("Unable to get access token from response");
					}
					else
					{
						return new GfycatState
						{
							Auth = auth
						};
					}
				}
			}
			else
			{
				throw new Exception("Unable to get auth token");
			}
		}

		protected override void ConfigureRequest(HttpWebRequest request, object state)
		{
			request.Accept = "application/json; charset=UTF-8";
			request.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
		}

		private static string GetBearerToken(AuthResponseModel auth)
		{
			return "Bearer " + auth.AccessToken;
		}

		protected override void ConfigureFormFields(NameValueCollection fields, object state)
		{
			GfycatState gfycat = (GfycatState)state;
			AuthResponseModel auth = gfycat.Auth;

			HttpWebRequest request = (HttpWebRequest)(WebRequest.Create(GFYCATS_ENDPOINT));
			request.Method = "POST";
			request.ContentType = "application/json; charset=UTF-8";
			request.Accept = "application/json; charset=UTF-8";
			request.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
			request.Headers[HttpRequestHeader.Authorization] = GetBearerToken(auth);

			JsonSerializer serializer = new JsonSerializer();

			using (Stream reqStream = request.GetRequestStream())
			using (StreamWriter writer = new StreamWriter(reqStream, Encoding.UTF8))
			{
				serializer.Serialize(
					writer,
					new CreateFileRequestModel
					{
						NoMd5 = "true",
						NoResize = "true"
					}
				);
			}

			HttpWebResponse response = (HttpWebResponse)(request.GetResponse());

			if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
			{
				using (Stream respStream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(respStream, Encoding.UTF8))
				{
					CreateFileResponseModel file = (CreateFileResponseModel)(serializer.Deserialize(reader, typeof(CreateFileResponseModel)));

					if (file == null || file.GfyName == null)
					{
						throw new Exception("Unable to get response from remote file creation");
					}
					else
					{
						gfycat.File = file;
						
						fields.Add("key", file.GfyName);
					}
				}
			}
			else
			{
				throw new Exception("Unable to create remote file");
			}
		}

		private UploadStatusResponseModel GetStatus(AuthResponseModel auth, string gfyname)
		{
			using (WebClient client = new WebClient())
			{
                string targetUrl = string.Format("{0}/{1}", STATUS_ENDPOINT, gfyname);
				client.Headers[HttpRequestHeader.Accept] = "application/json; charset=UTF-8";
				client.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
				client.Headers[HttpRequestHeader.Authorization] = GetBearerToken(auth);

				string json = client.DownloadString(targetUrl);

				return JsonConvert.DeserializeObject<UploadStatusResponseModel>(json);
			}
		}

		protected override string GetLink(HttpWebResponse response, object state)
		{
			switch (response.StatusCode)
			{
				case HttpStatusCode.OK:
				case HttpStatusCode.Created:
				case HttpStatusCode.NoContent:
					{
                        GfycatState gfycat = (GfycatState)state;
                        UploadStatusResponseModel status = null;
                        int current = 0;
                        int maxTries = 20;
                        string link = null;
                        bool completed = false;

                        while (current < maxTries && !completed)
                        {
                            status = GetStatus(gfycat.Auth, gfycat.File.GfyName);

                            if (status == null)
                            {
                                throw new Exception("Unexpected get status response");
                            }
							
							completed = (status.Task == "complete");

							if (completed)
							{
								if (status.GfyName != null)
								{
									link = string.Format("{0}/{1}", Url, status.GfyName);
								}
							}

							Thread.Sleep(1000);
                            current++;
                        }

						if (!completed)
						{
							throw new Exception("Unexpected task status: " + status.Task);
						}

						if (link == null)
						{
							link = string.Format("{0}/{1}", Url, gfycat.File.GfyName);
						}

						return link;

						/*
                        if (completed)
                        {
                            if (link == null)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    string targetUrl = string.Format("{0}/{1}", GFYCATS_ENDPOINT, gfycat.File.GfyName);
                                    client.Headers[HttpRequestHeader.Accept] = "application/json; charset=UTF-8";
                                    client.Headers[HttpRequestHeader.AcceptCharset] = "UTF-8";
                                    client.Headers[HttpRequestHeader.Authorization] = GetBearerToken(gfycat.Auth);

                                    GetByGfynameResponseModel gfyResponse = JsonConvert.DeserializeObject<GetByGfynameResponseModel>(
                                        client.DownloadString(targetUrl)
                                    );

                                    if (gfyResponse == null || gfyResponse.GfyItem == null)
                                    {
                                        throw new Exception("Unable to get gfycat by gfyname");
                                    }
                                    else
                                    {
                                        return gfyResponse.GfyItem.GifUrl;
                                    }
                                }
                            }
                            else
                            {
                                return link;
                            }
                        }
                        else
                        {
                            throw new Exception("Unexpected task status: " + status.Task);
                        }
						*/
					}
				default:
					{
						throw new Exception("Unsuccessfull file upload");
					}
			}
		}
    }
}
