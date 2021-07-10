#nullable enable
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Unclutter.SDK.Images
{
    public class SizedImageReference : ImageReference
    {
        /* Fields */
        private readonly ImageReference _imageReference;
        private readonly Size _size;

        /* Properties */
        public override bool IsDefault => _imageReference.IsDefault;
        public override bool HasImageSource => _imageReference.HasImageSource;

        /* Constructors */
        public SizedImageReference(ImageReference imageReference, Size size)
        {
            _imageReference = imageReference;
            _size = size;
        }

        /* Methods */
        public override BitmapImage? GetImageSource(ImageOptions? options = null)
        {
            return _imageReference.GetImageSource(new ImageOptions { Width = _size.Width, Height = _size.Height });
        }
        public override object? GetImageObject(ImageOptions? options = null)
        {
            return _imageReference.GetImageObject(new ImageOptions { Width = _size.Width, Height = _size.Height });
        }
    }
}
