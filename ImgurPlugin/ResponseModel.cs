using System.Net;
using Newtonsoft.Json;

namespace ImgurPlugin
{
	internal class ResponseModel
	{
		[JsonProperty("data")]
		public ImageResponseModel Data
		{
			get;
			set;
		}

		[JsonProperty("success")]
		public bool Success
		{
			get;
			set;
		}

		[JsonProperty("status")]
		public HttpStatusCode Status
		{
			get;
			set;
		}
	}
}
