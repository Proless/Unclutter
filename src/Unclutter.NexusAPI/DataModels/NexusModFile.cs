using System;
using Newtonsoft.Json;

namespace Unclutter.NexusAPI.DataModels
{
	public class NexusModFile
	{
		[JsonProperty("file_id")]
		public long FileId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("category_name")]
		public string CategoryName { get; set; }

		[JsonProperty("category_id")]
		public NexusModFileCategory Category { get; set; }

		[JsonProperty("is_primary")]
		public bool IsPrimary { get; set; }

		[JsonProperty("size")]
		public long Size { get; set; }

		[JsonProperty("file_name")]
		public string FileName { get; set; }

		[JsonProperty("uploaded_timestamp")]
		public long UploadedTimestamp { get; set; }

		[JsonProperty("uploaded_time")]
		public DateTimeOffset UploadedTime { get; set; }

		[JsonProperty("mod_version")]
		public string ModVersion { get; set; }

		[JsonProperty("external_virus_scan_url")]
		public Uri ExternalVirusScanUrl { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("size_kb")]
		public long SizeKb { get; set; }

		[JsonProperty("changelog_html")]
		public string ChangelogHtml { get; set; }

		[JsonProperty("content_preview_link")]
		public Uri ContentPreviewLink { get; set; }
	}
}
