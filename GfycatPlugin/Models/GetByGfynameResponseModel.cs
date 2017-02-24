using Newtonsoft.Json;

namespace GfycatPlugin.Models
{
	internal class GetByGfynameResponseModel
	{
		[JsonProperty("gfyItem")]
		public GfyItemModel GfyItem
		{
			get;
			set;
		}
	}
}
