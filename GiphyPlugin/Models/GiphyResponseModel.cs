using Newtonsoft.Json;

namespace GiphyPlugin.Models
{
	internal class GiphyResponseModel<TData>
	{
		[JsonProperty("data")]
		public TData Data
		{
			get;
			set;
		}
		
		[JsonProperty("meta")]
		public MetaResponseModel Meta
		{
			get;
			set;
		}
	}
}
