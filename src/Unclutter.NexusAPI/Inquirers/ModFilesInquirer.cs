using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;
using Unclutter.NexusAPI.Internals;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// File specific routes (E.g. retrieving file information, retrieving download link)
    /// </summary>
    public class ModFilesInquirer : InquirerBase, IModFilesInquirer
    {
        /// <summary>
        /// File specific routes (E.g. retrieving file information, retrieving download link)
        /// </summary>
        /// <param name="client">The NexusMods client to use for this endpoint</param>
        public ModFilesInquirer(INexusAPIClient client) : base(client) { }

        /// <summary>
        /// Return a list of all files for a specific mod of specific categories
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="categories">Determines which file should be retrieved by their category</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusModFileCollection> GetModFilesAsync(string gameDomain, long modId, NexusModFileCategory[] categories, CancellationToken cancellationToken = default)
        {

            var categoriesList = categories.Where(c => c != NexusModFileCategory.Deleted).Distinct();

            var commaSeparatedCategoryList = string.Join(",", categoriesList);

            var requestUri = ConstructRequestUri(Routes.ModFiles.Files, gameDomain, modId.ToString()).AddQuery("category", commaSeparatedCategoryList);

            return Client.ProcessRequestAsync<NexusModFileCollection>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns a list of all files for a specific mod
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusModFileCollection> GetModFilesAsync(string gameDomain, long modId, CancellationToken cancellationToken = default)
        {

            var requestUri = ConstructRequestUri(Routes.ModFiles.Files, gameDomain, modId.ToString());

            return Client.ProcessRequestAsync<NexusModFileCollection>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Returns a specified mod file, using a specified game and mod
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="fileId">The mod file Id to retrieve</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<NexusModFile> GetModFileAsync(string gameDomain, long modId, long fileId, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.ModFiles.File, gameDomain, modId.ToString(), fileId.ToString());

            return Client.ProcessRequestAsync<NexusModFile>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Generate download link for mod file. For premium users, will return array of download links with their preferred download location in the first element.<br/>
        /// <br/>NOTE: Non-premium members must provide the key and expiry from the .nxm link provided by the website.
        /// <br/>It is recommended for clients to extract them from the NXM link before sending this request.
        /// <br/>This ensures that all non-premium members must access the website to download through the API.
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="fileId">The mod file Id to retrieve</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusModFileDownloadLink>> GetModFileDownloadLinksAsync(string gameDomain, long modId, long fileId, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.ModFiles.DownloadLink, gameDomain, modId.ToString(), fileId.ToString());

            return Client.ProcessRequestAsync<IEnumerable<NexusModFileDownloadLink>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Generate download link for mod file. For premium users, will return array of download links with their preferred download location in the first element.<br/>
        /// <br/>NOTE: Non-premium members must provide the key and expiry from the .nxm link provided by the website.
        /// <br/>It is recommended for clients to extract them from the NXM link before sending this request.
        /// <br/>This ensures that all non-premium members must access the website to download through the API.
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="fileId">The mod file Id to retrieve</param>
        /// <param name="download_key">The Key provided by Nexus Mods Website</param>
        /// <param name="link_expires">The expiry of the key</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusModFileDownloadLink>> GetModFileDownloadLinksAsync(string gameDomain, long modId, long fileId, string download_key, long link_expires, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.ModFiles.DownloadLink, gameDomain, modId.ToString(), fileId.ToString()).AddQuery("key", download_key).AddQuery("expires", link_expires.ToString());

            return Client.ProcessRequestAsync<IEnumerable<NexusModFileDownloadLink>>(requestUri, HttpMethod.Get, cancellationToken);
        }

        /// <summary>
        /// Looks up a file MD5 file hash and returns an <see cref="IEnumerable{T}"/> of all mod files and the associated mods
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="md5_hash">The file md5 hash for lookup</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        public Task<IEnumerable<NexusFileHashResult>> GetMD5HashResult(string gameDomain, string md5_hash, CancellationToken cancellationToken = default)
        {
            var requestUri = ConstructRequestUri(Routes.ModFiles.FileHash, gameDomain, md5_hash);

            return Client.ProcessRequestAsync<IEnumerable<NexusFileHashResult>>(requestUri, HttpMethod.Get, cancellationToken);
        }
    }
}
