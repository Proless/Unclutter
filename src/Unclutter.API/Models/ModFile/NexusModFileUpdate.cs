using System;
using System.Text.Json.Serialization;

namespace Unclutter.API.Models.ModFile
{
    public class NexusModFileUpdate
    {
        [JsonPropertyName("old_file_id")]
        public long OldFileId { get; set; }

        [JsonPropertyName("new_file_id")]
        public long NewFileId { get; set; }

        [JsonPropertyName("old_file_name")]
        public string OldFileName { get; set; }

        [JsonPropertyName("new_file_name")]
        public string NewFileName { get; set; }

        [JsonPropertyName("uploaded_timestamp")]
        public long UploadedTimestamp { get; set; }

        [JsonPropertyName("uploaded_time")]
        public DateTimeOffset UploadedTime { get; set; }
    }
}
