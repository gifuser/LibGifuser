using Newtonsoft.Json;

namespace GfycatPlugin
{
	internal class UploadStatusResponseModel
	{
		[JsonProperty("task")]
		public string Task
		{
			get;
			set;
		}

		[JsonProperty("time")]
		public long? Time
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

        [JsonProperty("gifUrl")]
        public string GifUrl
        {
            get;
            set;
        }
    }
}
