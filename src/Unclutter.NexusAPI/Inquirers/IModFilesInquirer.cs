using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// File specific routes (E.g. retrieving file information, retrieving download link)
    /// </summary>
    public interface IModFilesInquirer : IInquirerRateManager
    {
        /// <summary>
        /// Looks up a file MD5 file hash and returns an <see cref="IEnumerable{T}"/> of all mod files and the associated mods
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="md5Hash">The file md5 hash for lookup</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<IEnumerable<NexusFileHashResult>> GetMD5HashResult(string gameDomain, string md5Hash, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns a specified mod file, using a specified game and mod
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="fileId">The mod file Id to retrieve</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        Task<NexusModFile> GetModFileAsync(string gameDomain, long modId, long fileId, CancellationToken cancellationToken = default);
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
        Task<IEnumerable<NexusModFileDownloadLink>> GetModFileDownloadLinksAsync(string gameDomain, long modId, long fileId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Generate download link for mod file. For premium users, will return array of download links with their preferred download location in the first element.<br/>
        /// <br/>NOTE: Non-premium members must provide the key and expiry from the .nxm link provided by the website.
        /// <br/>It is recommended for clients to extract them from the NXM link before sending this request.
        /// <br/>This ensures that all non-premium members must access the website to download through the API.
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="fileId">The mod file Id to retrieve</param>
        /// <param name="downloadKey">The Key provided by Nexus Mods Website</param>
        /// <param name="linkExpires">The expiry of the key</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<IEnumerable<NexusModFileDownloadLink>> GetModFileDownloadLinksAsync(string gameDomain, long modId, long fileId, string downloadKey, long linkExpires, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns a list of all files for a specific mod
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<NexusModFileCollection> GetModFilesAsync(string gameDomain, long modId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Return a list of all files for a specific mod of specific categories
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="categories">Determines which file should be retrieved by their category</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<NexusModFileCollection> GetModFilesAsync(string gameDomain, long modId, NexusModFileCategory[] categories, CancellationToken cancellationToken = default);
    }
}