using Newtonsoft.Json;

namespace GiphyPlugin.Models.Get.Images
{
	internal class ImageWebpModel : ImageCommonModel
	{
		[JsonProperty("webp")]
		public string Webp
		{
			get;
			set;
		}

		[JsonProperty("webp_size")]
		public string WebpSize
		{
			get;
			set;
		}

	}
}
