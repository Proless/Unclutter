using System;
using System.Text.Json.Serialization;

namespace Unclutter.API.Models.ModFile
{
    public class NexusModFile
    {
        [JsonPropertyName("id")]
        public long[] Id { get; set; }

        [JsonPropertyName("uid")]
        public long UId { get; set; }

        [JsonPropertyName("file_id")]
        public long FileId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("category_name")]
        public string CategoryName { get; set; }

        [JsonPropertyName("category_id")]
        public NexusModFileCategory Category { get; set; }

        [JsonPropertyName("is_primary")]
        public bool IsPrimary { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        [JsonPropertyName("uploaded_timestamp")]
        public long UploadedTimestamp { get; set; }

        [JsonPropertyName("uploaded_time")]
        public DateTimeOffset UploadedTime { get; set; }

        [JsonPropertyName("mod_version")]
        public string ModVersion { get; set; }

        [JsonPropertyName("external_virus_scan_url")]
        public Uri ExternalVirusScanUrl { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("size_kb")]
        public long SizeKb { get; set; }

        [JsonPropertyName("changelog_html")]
        public string ChangelogHtml { get; set; }

        [JsonPropertyName("content_preview_link")]
        public Uri ContentPreviewLink { get; set; }
    }
}
