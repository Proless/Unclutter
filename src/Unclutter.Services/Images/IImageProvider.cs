using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Images;
using Unclutter.SDK.Models;

namespace Unclutter.Services.Images
{
    public interface IImageProvider
    {
        public Task<ImageReference> DownloadImageFor(IGameDetails game, CancellationToken cancellationToken = default);
        public Task<ImageReference> DownloadImageFor(IUserDetails user, CancellationToken cancellationToken = default);
        public ImageReference GetImageFor(IGameDetails game);
        public ImageReference GetImageFor(IUserDetails user);
        public ImageReference GetImageFrom(string file, int height, int width);
    }
}
