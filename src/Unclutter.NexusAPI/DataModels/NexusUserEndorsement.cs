using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusUserEndorsement
	{
		[JsonProperty("mod_id")]
		public long ModId { get; set; }

		[JsonProperty("domain_name")]
		public string DomainName { get; set; }

		[JsonProperty("date")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTimeOffset Date { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }
	}
}
