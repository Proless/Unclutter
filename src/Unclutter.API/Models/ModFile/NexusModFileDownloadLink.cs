using System;
using System.Text.Json.Serialization;

namespace Unclutter.API.Models.ModFile
{
    public class NexusModFileDownloadLink
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("URI")]
        public Uri Uri { get; set; }
    }
}
