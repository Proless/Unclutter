#nullable enable
using System;
using System.Windows.Media.Imaging;

namespace Unclutter.SDK.Images
{
    public abstract class UriImageReference : ImageReference
    {
        public abstract Uri GetImageUri();
        public override string ToString()
        {
            return GetImageUri().ToString();
        }
        public override BitmapImage? GetImageSource(ImageOptions? options = null)
        {
            var imgOptions = options ?? new ImageOptions();
            try
            {
                var source = GetBitmapImage(GetImageUri(), imgOptions.Height, imgOptions.Width);
                return source;
            }
            catch
            {
                return null;
            }
        }
        protected BitmapImage? GetBitmapImage(Uri? uri, int height = 0, int width = 0)
        {
            if (uri == null)
            {
                return null;
            }

            try
            {
                var source = new BitmapImage();
                source.BeginInit();
                source.UriSource = uri;
                source.DecodePixelHeight = height;
                source.DecodePixelWidth = width;
                source.CacheOption = BitmapCacheOption.OnLoad;
                source.CreateOptions = BitmapCreateOptions.DelayCreation;
                source.EndInit();
                source.Freeze();
                return source;
            }
            catch
            {
                return null;
            }
        }
    }
}
