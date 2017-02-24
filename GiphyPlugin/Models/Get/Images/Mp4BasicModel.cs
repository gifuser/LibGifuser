using Newtonsoft.Json;

namespace GiphyPlugin.Models.Get.Images
{
	internal class Mp4BasicModel
	{
		[JsonProperty("mp4")]
		public string Mp4
		{
			get;
			set;
		}

		[JsonProperty("mp4_size")]
		public string Mp4Size
		{
			get;
			set;
		}
	}
}
