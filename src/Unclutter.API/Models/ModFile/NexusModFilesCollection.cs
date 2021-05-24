using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Unclutter.API.Models.ModFile
{
    public class NexusModFilesCollection
    {
        [JsonPropertyName("files")]
        public IEnumerable<NexusModFile> ModFiles { get; set; }

        [JsonPropertyName("file_updates")]
        public IEnumerable<NexusModFileUpdate> Updates { get; set; }
    }
}
