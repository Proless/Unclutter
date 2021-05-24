#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.API.Models.ModFile;

namespace Unclutter.API
{
    /// <summary>
    /// Routes specific to retrieve information regarding mod files (file information, download link,...)
    /// </summary>
    public class ModFilesClient : NexusClient
    {
        /// <summary>
        /// Create a new instance of the <see cref="ModFilesClient"/> class
        /// </summary>
        /// <param name="httpClient">The HttpClient instance to use for this endpoint</param>
        public ModFilesClient(HttpClient httpClient) : base(httpClient) { }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override Action<HttpRequestMessage>? RequestConfigurator { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public override IList<JsonConverter>? Converters => null;

        /// <summary>
        /// Returns a specific mod file, using a specified game and mod
        /// </summary>
        public Task<NexusModFile> GetModFileAsync(string apiKey, string gameDomain, long modId, long fileId, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/{modId}/files/{fileId}.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<NexusModFile>(request, cancellationToken);
        }

        /// <summary>
        /// Return a list of all files for a specific mod.
        /// </summary>
        public Task<NexusModFilesCollection> GetModFilesAsync(string apiKey, string gameDomain, long modId, NexusModFileCategory[] categories, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/{modId}/files.json")
                .WithAPIKey(apiKey);

            if (categories.Any(c => c != NexusModFileCategory.Deleted))
            {
                var includeCategories = string.Join(",", categories.Where(c => c != NexusModFileCategory.Deleted).Distinct().Select(c => c.ToString().ToLower()));
                request.WithQueryParameter("category", includeCategories);
            }

            return ProcessRequestAsync<NexusModFilesCollection>(request.Create(), cancellationToken);
        }

        /// <summary>
        /// Return a list of all files for a specific mod, including deleted files.
        /// </summary>
        public Task<NexusModFilesCollection> GetModFilesAsync(string apiKey, string gameDomain, long modId, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/{modId}/files.json")
                .WithAPIKey(apiKey);

            return ProcessRequestAsync<NexusModFilesCollection>(request.Create(), cancellationToken);
        }

        /// <summary>
        /// Generate download link for mod file. For premium users, will return array of download links with their preferred download location in the first element.<br/>
        /// <br/>NOTE: Non-premium members must provide the key and expiry from the .nxm link provided by the website.
        /// <br/>It is recommended for clients to extract them from the NXM link before sending this request.
        /// <br/>This ensures that all non-premium members must access the website to download through the API.
        /// </summary>
        public Task<IEnumerable<NexusModFileDownloadLink>> GetDownloadLinksAsync(string apiKey, string gameDomain, long modId, long fileId, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/{modId}/files/{fileId}/download_link.json")
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusModFileDownloadLink>>(request, cancellationToken);
        }

        /// <summary>
        /// Generate download link for mod file. For premium users, will return array of download links with their preferred download location in the first element.<br/>
        /// <br/>NOTE: Non-premium members must provide the key and expiry from the .nxm link provided by the website.
        /// <br/>It is recommended for clients to extract them from the NXM link before sending this request.
        /// <br/>This ensures that all non-premium members must access the website to download through the API.
        /// </summary>
        public Task<IEnumerable<NexusModFileDownloadLink>> GetDownloadLinksAsync(string apiKey, string gameDomain, long modId, long fileId, string downloadKey, long expires, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
                .Get($"/games/{gameDomain}/mods/{modId}/files/{fileId}/download_link.json")
                .WithQueryParameter("key", downloadKey)
                .WithQueryParameter("expires", expires.ToString())
                .WithAPIKey(apiKey)
                .Create();

            return ProcessRequestAsync<IEnumerable<NexusModFileDownloadLink>>(request, cancellationToken);
        }

        /// <summary>
        /// Looks up a MD5 file hash and returns a list of all mod files and the associated mods matching the provided MD5 hash
        /// </summary>
        public Task<IEnumerable<NexusModFileHashQueryResult>> QueryFileHashAsync(string apiKey, string gameDomain, string md5Hash, CancellationToken cancellationToken)
        {
            var request = RequestBuilder
              .Get($"/games/{gameDomain}/mods/md5_search/{md5Hash}.json")
              .WithAPIKey(apiKey)
              .Create();

            return ProcessRequestAsync<IEnumerable<NexusModFileHashQueryResult>>(request, cancellationToken);
        }
    }
}
