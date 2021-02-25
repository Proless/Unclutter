using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI.DataModels;

namespace Unclutter.NexusAPI.Inquirers
{
    /// <summary>
    /// Routes specific to retrieve information regarding colour-specific themes for games
    /// </summary>
    public interface IColourSchemesInquirer : IInquirerRateManager
    {
        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> of all colour schemes, including the primary, secondary and 'darker' colours.
        /// </summary>
        /// <param name="cancellationToken">Enables cancellation of the HTTP request</param>
        Task<IEnumerable<NexusColourScheme>> GetColourSchemesAsync(CancellationToken cancellationToken = default);
    }
}