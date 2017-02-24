using Newtonsoft.Json;

namespace GiphyPlugin.Models.Upload
{
	internal class UploadResponseDataModel
	{
		[JsonProperty("id")]
		public string Id
		{
			get;
			set;
		}
	}
}
