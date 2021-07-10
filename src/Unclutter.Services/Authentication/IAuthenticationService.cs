using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Models;

namespace Unclutter.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<IUserDetails> RequestAPIKey(string applicationReference, CancellationToken cancellationToken);
        Task<IUserDetails> ValidateAPIKey(string key, CancellationToken cancellationToken);
    }
}
