using Newtonsoft.Json;

namespace GiphyPlugin.Models.Get.Images
{
	internal class ImageCommonModel
	{
		[JsonProperty("url")]
		public string Url
		{
			get;
			set;
		}

		[JsonProperty("width")]
		public string Width
		{
			get;
			set;
		}

		[JsonProperty("height")]
		public string Height
		{
			get;
			set;
		}

		[JsonProperty("size")]
		public string Size
		{
			get;
			set;
		}
	}
}
