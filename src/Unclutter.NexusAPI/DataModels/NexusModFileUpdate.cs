using System;
using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusModFileUpdate
	{
		[JsonProperty("old_file_id")]
		public long OldFileId { get; set; }

		[JsonProperty("new_file_id")]
		public long NewFileId { get; set; }

		[JsonProperty("old_file_name")]
		public string OldFileName { get; set; }

		[JsonProperty("new_file_name")]
		public string NewFileName { get; set; }

		[JsonProperty("uploaded_timestamp")]
		public long UploadedTimestamp { get; set; }

		[JsonProperty("uploaded_time")]
		public DateTimeOffset UploadedTime { get; set; }
	}
}
