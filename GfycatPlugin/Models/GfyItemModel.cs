using Newtonsoft.Json;

namespace GfycatPlugin.Models
{
	internal class GfyItemModel
	{
		[JsonProperty("gfyId")]
		public string GfyId
		{
			get;
			set;
		}

		[JsonProperty("gfyName")]
		public string GfyName
		{
			get;
			set;
		}

		[JsonProperty("gfyNumber")]
		public string GfyNumber
		{
			get;
			set;
		}

		[JsonProperty("webmUrl")]
		public string WebmUrl
		{
			get;
			set;
		}

		[JsonProperty("gifUrl")]
		public string GifUrl
		{
			get;
			set;
		}

		[JsonProperty("mobileUrl")]
		public string MobileUrl
		{
			get;
			set;
		}

		[JsonProperty("mobilePosterUrl")]
		public string MobilePosterUrl
		{
			get;
			set;
		}

		[JsonProperty("miniUrl")]
		public string MiniUrl
		{
			get;
			set;
		}

		[JsonProperty("miniPosterUrl")]
		public string MiniPosterUrl
		{
			get;
			set;
		}

		[JsonProperty("posterUrl")]
		public string PosterUrl
		{
			get;
			set;
		}

		[JsonProperty("thumb360Url")]
		public string Thumb360Url
		{
			get;
			set;
		}

		[JsonProperty("thumb360PosterUrl")]
		public string Thumb360PosterUrl
		{
			get;
			set;
		}

		[JsonProperty("thumb100PosterUrl")]
		public string Thumb100PosterUrl
		{
			get;
			set;
		}

		[JsonProperty("max5mbGif")]
		public string Max5mbGif
		{
			get;
			set;
		}

		[JsonProperty("max2mbGif")]
		public string Max2mbGif
		{
			get;
			set;
		}

		[JsonProperty("max1mbGif")]
		public string Max1mbGif
		{
			get;
			set;
		}

		[JsonProperty("gif100px")]
		public string Gif100px
		{
			get;
			set;
		}

		[JsonProperty("mjpgUrl")]
		public string MjpgUrl
		{
			get;
			set;
		}

		[JsonProperty("width")]
		public long Width
		{
			get;
			set;
		}

		[JsonProperty("height")]
		public long Height
		{
			get;
			set;
		}

		[JsonProperty("avgColor")]
		public string AvgColor
		{
			get;
			set;
		}

		[JsonProperty("frameRate")]
		public long FrameRate
		{
			get;
			set;
		}

		[JsonProperty("numFrames")]
		public long NumFrames
		{
			get;
			set;
		}

		[JsonProperty("mp4Size")]
		public long Mp4Size
		{
			get;
			set;
		}

		[JsonProperty("webmSize")]
		public long WebmSize
		{
			get;
			set;
		}

		[JsonProperty("gifSize")]
		public long GifSize
		{
			get;
			set;
		}

		[JsonProperty("source")]
		public long Source
		{
			get;
			set;
		}

		[JsonProperty("createDate")]
		public long CreateDate
		{
			get;
			set;
		}

		[JsonProperty("nsfw")]
		public long Nsfw
		{
			get;
			set;
		}

		[JsonProperty("mp4Url")]
		public string Mp4Url
		{
			get;
			set;
		}

		[JsonProperty("likes")]
		public long Likes
		{
			get;
			set;
		}

		[JsonProperty("published")]
		public long Published
		{
			get;
			set;
		}

		[JsonProperty("dislikes")]
		public long Dislikes
		{
			get;
			set;
		}

		[JsonProperty("extraLemmas")]
		public string ExtraLemmas
		{
			get;
			set;
		}

		[JsonProperty("md5")]
		public string Md5
		{
			get;
			set;
		}

		[JsonProperty("views")]
		public long Views
		{
			get;
			set;
		}

		[JsonProperty("tags")]
		public string[] Tags
		{
			get;
			set;
		}

		[JsonProperty("userName")]
		public string UserName
		{
			get;
			set;
		}

		[JsonProperty("title")]
		public string Title
		{
			get;
			set;
		}

		[JsonProperty("description")]
		public string Description
		{
			get;
			set;
		}

		[JsonProperty("languageCategories")]
		public object LanguageCategories
		{
			get;
			set;
		}

		[JsonProperty("domainWhitelist")]
		public object[] DomainWhitelist
		{
			get;
			set;
		}

	}
}
