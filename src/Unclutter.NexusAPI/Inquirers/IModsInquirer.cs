using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// Mod specific routes (E.g. retrieving latest mods, endorsing a mod)
    /// </summary>
    public interface IModsInquirer : IInquirerRateManager
    {
        /// <summary>
        /// Abstain from endorsing a specified mod version
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id to retrieve</param>
        /// <param name="version">The mod version to abstain from endorsing</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<NexusMessage> Abstain(string gameDomain, long modId, string version, CancellationToken cancellationToken = default);
        /// <summary>
        /// Endorse a specified mod version
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id to retrieve</param>
        /// <param name="version">The mod version to endorse</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<NexusMessage> Endorse(string gameDomain, long modId, string version, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of the 10 latest added mods for a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<IEnumerable<NexusMod>> GetLatestAddedMods(string gameDomain, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of the 10 trending mods for a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<IEnumerable<NexusMod>> GetLatestTrendingMods(string gameDomain, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of the 10 latest updated mods for a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<IEnumerable<NexusMod>> GetLatestUpdatedMods(string gameDomain, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns a specified mod, from a specified game
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id to retrieve</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<NexusMod> GetMod(string gameDomain, long modId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns a Dictionary of all change logs for a mod
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="modId">The mod Id</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<Dictionary<string, IEnumerable<string>>> GetModChangeLogs(string gameDomain, long modId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of mods that have been updated in a given period, with timestamps of their last update.
        /// </summary>
        /// <param name="gameDomain">The game domain name</param>
        /// <param name="period">The time period!</param>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        /// <returns></returns>
        Task<IEnumerable<NexusModUpdate>> GetUpdatedMods(string gameDomain, NexusTimePeriod period, CancellationToken cancellationToken = default);
    }
}