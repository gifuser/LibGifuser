using Newtonsoft.Json;

namespace GfycatPlugin.Models
{
	internal class AuthRequestModel
	{
		[JsonProperty("grant_type")]
		public string GrantType
		{
			get;
			set;
		}
		
		[JsonProperty("client_id")]
		public string ClientId
		{
			get;
			set;
		}

		[JsonProperty("client_secret")]
		public string ClientSecret
		{
			get;
			set;
		}
	}
}
