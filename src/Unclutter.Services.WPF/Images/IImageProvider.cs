using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Unclutter.SDK.IModels;

namespace Unclutter.Services.WPF.Images
{
    public interface IImageProvider : IDisposable
    {
        public Task<ImageSource> DownloadImageFor(IGameDetails game, CancellationToken cancellationToken = default);
        public Task<ImageSource> DownloadImageFor(IUserDetails user, CancellationToken cancellationToken = default);
        public ImageSource GetImageFor(IGameDetails game);
        public ImageSource GetImageFor(IUserDetails user);
        public ImageSource GetImageFrom(string file, int height, int width);
    }
}
