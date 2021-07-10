#nullable enable
using System.IO;
using System.Windows.Media.Imaging;

namespace Unclutter.SDK.Images
{
    public abstract class StreamImageReference : ImageReference
    {
        public override BitmapImage? GetImageSource(ImageOptions? options = null)
        {
            var imgOptions = options ?? new ImageOptions();
            var imageStream = GetImageStream();
            var source = GetBitmapImage(imageStream, imgOptions.Height, imgOptions.Width);
            return source;
        }
        protected BitmapImage? GetBitmapImage(Stream? stream, int height = 0, int width = 0)
        {
            if (stream == null)
            {
                return null;
            }

            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = stream;
            source.DecodePixelHeight = height;
            source.DecodePixelWidth = width;
            source.CacheOption = BitmapCacheOption.OnLoad;
            source.CreateOptions = BitmapCreateOptions.DelayCreation;
            source.EndInit();
            source.Freeze();
            return source;
        }
        protected abstract Stream? GetImageStream();
    }
}
