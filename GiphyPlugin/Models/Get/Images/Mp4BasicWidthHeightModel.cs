using Newtonsoft.Json;

namespace GiphyPlugin.Models.Get.Images
{
	internal class Mp4BasicWidthHeightModel : Mp4BasicModel
	{
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

	}
}
