using Newtonsoft.Json;

namespace GfycatPlugin.Models
{
	internal class CreateFileResponseModel
	{
		[JsonProperty("isOk")]
		public bool IsOk
		{
			get;
			set;
		}

		[JsonProperty("gfyname")]
		public string GfyName
		{
			get;
			set;
		}

		[JsonProperty("secret")]
		public string Secret
		{
			get;
			set;
		}

		[JsonProperty("uploadType")]
		public string UploadType
		{
			get;
			set;
		}
	}
}
