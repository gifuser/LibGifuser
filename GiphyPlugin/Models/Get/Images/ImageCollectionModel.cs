using Newtonsoft.Json;

namespace GiphyPlugin.Models.Get.Images
{
	internal class ImageCollectionModel
	{
		[JsonProperty("fixed_height")]
		public ImageMp4WebpModel FixedHeight
		{
			get;
			set;
		}

		[JsonProperty("fixed_height_still")]
		public ImageCommonModel FixedHeightStill
		{
			get;
			set;
		}

		[JsonProperty("fixed_height_downsampled")]
		public ImageWebpModel FixedHeightDownsampled
		{
			get;
			set;
		}

		[JsonProperty("fixed_height_small")]
		public ImageMp4WebpModel FixedHeightSmall
		{
			get;
			set;
		}

		[JsonProperty("fixed_height_small_still")]
		public ImageCommonModel FixedHeightSmallStill
		{
			get;
			set;
		}

		[JsonProperty("fixed_width")]
		public ImageMp4WebpModel FixedWidth
		{
			get;
			set;
		}

		[JsonProperty("fixed_width_still")]
		public ImageCommonModel FixedWidthStill
		{
			get;
			set;
		}

		[JsonProperty("fixed_width_downsampled")]
		public ImageWebpModel FixedWidthDownsampled
		{
			get;
			set;
		}

		[JsonProperty("fixed_width_small")]
		public ImageMp4WebpModel FixedWidthSmall
		{
			get;
			set;
		}

		[JsonProperty("fixed_width_small_still")]
		public ImageCommonModel FixedWidthSmallStill
		{
			get;
			set;
		}

		[JsonProperty("downsized")]
		public ImageCommonModel Downsized
		{
			get;
			set;
		}

		[JsonProperty("downsized_still")]
		public ImageCommonModel DownsizedStill
		{
			get;
			set;
		}

		[JsonProperty("downsized_large")]
		public ImageCommonModel DownsizedLarge
		{
			get;
			set;
		}

		[JsonProperty("downsized_medium")]
		public ImageCommonModel DownsizedMedium
		{
			get;
			set;
		}

		[JsonProperty("downsized_small")]
		public Mp4BasicModel DownsizedSmall
		{
			get;
			set;
		}

		[JsonProperty("original")]
		public ImageOriginalModel Original
		{
			get;
			set;
		}

		[JsonProperty("original_still")]
		public ImageCommonModel OriginalStill
		{
			get;
			set;
		}

		[JsonProperty("original_mp4")]
		public Mp4BasicWidthHeightModel OriginalMp4
		{
			get;
			set;
		}

		[JsonProperty("looping")]
		public Mp4BasicModel Looping
		{
			get;
			set;
		}

		[JsonProperty("preview")]
		public Mp4BasicWidthHeightModel Preview
		{
			get;
			set;
		}

		[JsonProperty("hd")]
		public Mp4BasicWidthHeightModel Hd
		{
			get;
			set;
		}
	}
}
