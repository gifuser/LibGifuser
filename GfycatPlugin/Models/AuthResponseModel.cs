using Newtonsoft.Json;

namespace GfycatPlugin.Models
{
	internal class AuthResponseModel
	{
		[JsonProperty("token_type")]
		public string TokenType
		{
			get;
			set;
		}

		[JsonProperty("scope")]
		public string Scope
		{
			get;
			set;
		}

		[JsonProperty("expires_in")]
		public long ExpiresIn
		{
			get;
			set;
		}

		[JsonProperty("access_token")]
		public string AccessToken
		{
			get;
			set;
		}
	}
}
