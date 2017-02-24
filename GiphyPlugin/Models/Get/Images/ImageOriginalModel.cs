using Newtonsoft.Json;

namespace GiphyPlugin.Models.Get.Images
{
	internal class ImageOriginalModel : ImageMp4WebpModel
	{
		[JsonProperty("frames")]
		public string Frames
		{
			get;
			set;
		}

		[JsonProperty("hash")]
		public string Hash
		{
			get;
			set;
		}
	}
}
