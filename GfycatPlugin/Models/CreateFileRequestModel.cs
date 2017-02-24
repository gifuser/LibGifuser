using Newtonsoft.Json;

namespace GfycatPlugin.Models
{
	internal class CreateFileRequestModel
	{
		[JsonProperty("noMd5")]
		public string NoMd5
		{
			get;
			set;
		}

		[JsonProperty("noResize")]
		public string NoResize
		{
			get;
			set;
		}
	}
}
