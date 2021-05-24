using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;

namespace Unclutter.Services.Images
{
    public class ImageProvider : IImageProvider
    {
        /* Fields */
        private readonly HttpClient _downloadClient;
        private readonly string _gameImagesDir;
        private readonly string _userImagesDir;

        /* Constructors */
        public ImageProvider(IDirectoryService directoryService)
        {
            _downloadClient = new HttpClient();

            _gameImagesDir = Path.Combine(directoryService.DataDirectory, "images", "games");
            _userImagesDir = Path.Combine(directoryService.DataDirectory, "images", "users");

            directoryService.EnsureDirectoryAccess(_gameImagesDir);
            directoryService.EnsureDirectoryAccess(_userImagesDir);
        }

        /* Methods */
        public ImageSource GetImageFrom(string file, int height, int width)
        {
            var source = new BitmapImage();

            if (file is null || !File.Exists(file))
            {
                source.Freeze();
                return source;
            }

            source.BeginInit();
            source.UriSource = new Uri(file);
            source.DecodePixelHeight = height;
            source.DecodePixelWidth = width;
            source.CacheOption = BitmapCacheOption.OnLoad;
            source.EndInit();
            source.Freeze();
            return source;
        }

        public Task<ImageSource> DownloadImageFor(IGameDetails game, CancellationToken cancellationToken = default)
        {
            ImageSource imageSource = new BitmapImage();
            imageSource.Freeze();

            if (game is null) return Task.FromResult(imageSource);

            var uri = $"https://staticdelivery.nexusmods.com/Images/games/4_3/tile_{game.Id}.jpg";
            var imageFile = GetImageFileFor(game);

            return _downloadClient.GetStreamAsync(uri).ContinueWith(task =>
            {
                if (File.Exists(imageFile))
                    File.Delete(imageFile);

                Image.FromStream(task.Result).GetThumbnailImage(180, 255, null, IntPtr.Zero).Save(imageFile, ImageFormat.Jpeg);
                return GetImageFor(game);
            }, cancellationToken);
        }

        public Task<ImageSource> DownloadImageFor(IUserDetails user, CancellationToken cancellationToken = default)
        {
            ImageSource imageSource = new BitmapImage();
            imageSource.Freeze();

            if (user is null) return Task.FromResult(imageSource);

            var uri = user.ProfileUri;
            var imageFile = GetImageFileFor(user);

            return _downloadClient.GetStreamAsync(uri, cancellationToken).ContinueWith(task =>
            {
                if (File.Exists(imageFile))
                    File.Delete(imageFile);

                Image.FromStream(task.Result).GetThumbnailImage(200, 191, null, IntPtr.Zero).Save(imageFile, ImageFormat.Jpeg);
                return GetImageFor(user);
            }, cancellationToken);
        }

        public ImageSource GetImageFor(IGameDetails game)
        {
            var imageFile = GetImageFileFor(game);
            if (!File.Exists(imageFile))
            {
                ExtractImage(game);
            }
            return GetImageFrom(imageFile, 255, 180);
        }

        public ImageSource GetImageFor(IUserDetails user)
        {
            var imageFile = GetImageFileFor(user);
            return GetImageFrom(File.Exists(imageFile) ? imageFile : null, 191, 200);
        }

        public void Dispose()
        {
            try
            {
                _downloadClient?.Dispose();
            }
            catch (ObjectDisposedException)
            { }
        }

        /* Helpers */
        private string GetImageFileFor(IGameDetails game)
        {
            return Path.Combine(_gameImagesDir, $"tile_{game.Id}.jpg");
        }

        private string GetImageFileFor(IUserDetails user)
        {
            return Path.Combine(_userImagesDir, $"photo-{user.Id}.jpg");
        }

        private void ExtractImage(IGameDetails game)
        {
            // using the Embedded resource file "images.zip"
            var resourceName = Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .First(r => r.EndsWith("images.zip"));
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var archive = new ZipArchive(stream ?? throw new InvalidOperationException("Couldn't find the embedded resource file images.zip"));

            var imageFile = GetImageFileFor(game);
            var entry = archive.Entries.FirstOrDefault(e => e.Name == Path.GetFileName(imageFile));
            entry?.ExtractToFile(imageFile, true);
        }
    }
}
