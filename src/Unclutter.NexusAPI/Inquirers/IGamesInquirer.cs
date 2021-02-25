using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// Routes specific to retrieve information regarding supported games
    /// </summary>
    public interface IGamesInquirer : IInquirerRateManager
    {
        /// <summary>
        /// Returns a specified game, along with download count, file count and categories.
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        Task<NexusGame> GetGameAsync(string gameDomain, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of all games, optionally can also return unapproved games
        /// </summary>
        /// <param name="includeUnapproved">Determines whether to include unapproved games or not</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        Task<IEnumerable<NexusGame>> GetGamesAsync(bool includeUnapproved = false, CancellationToken cancellationToken = default);
    }
}