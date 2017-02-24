using System.Net;
using Newtonsoft.Json;

namespace GiphyPlugin.Models
{
	internal class MetaResponseModel
	{
		[JsonProperty("status")]
		public HttpStatusCode Status
		{
			get;
			set;
		}

		[JsonProperty("msg")]
		public string Message
		{
			get;
			set;
		}

		[JsonProperty("response_id")]
		public string ResponseId
		{
			get;
			set;
		}
	}
}
