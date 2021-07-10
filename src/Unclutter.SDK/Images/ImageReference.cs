#nullable enable
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Unclutter.SDK.Images
{
    ///<summary>
    /// Represents a base class to reference an Image.
    /// </summary>
    /// <remarks>
    /// The public methods and properties should never throw exceptions, instead they should return a default value.
    /// </remarks>
    public abstract class ImageReference
    {
        public static ImageReference Default { get; } = new DefaultImageReference();

        /// <summary>
        /// Determines if this instance has no reference to any image.
        /// </summary>
        public abstract bool IsDefault { get; }

        /// <summary>
        /// Determines if an <see cref="ImageSource"/> can be obtained.
        /// </summary>
        public abstract bool HasImageSource { get; }

        /// <summary>
        /// Get an <see cref="ImageSource"/> to the referenced Image
        /// </summary>
        /// <param name="options">The options to use when rendering the image</param>
        public abstract BitmapImage? GetImageSource(ImageOptions? options = null);

        /// <summary>
        /// Get the Image object which this <see cref="ImageReference"/> represents.
        /// </summary>
        /// <remarks>
        /// The returned object will be hosted in a <see cref="ContentControl"/>
        /// </remarks>
        /// <param name="options">The options to use when rendering the image</param>
        public virtual object? GetImageObject(ImageOptions? options = null)
        {
            Image? image;
            try
            {
                var source = GetImageSource(options);
                image = GetImage(source);
            }
            catch
            {
                image = null;
            }
            return image;
        }

        /* Helpers */
        protected Image GetImage(ImageSource? imageSource)
        {
            var image = new Image
            {
                Source = imageSource,
                Stretch = Stretch.Uniform,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
            RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);

            return image;
        }
    }

    public class DefaultImageReference : ImageReference
    {
        public override bool IsDefault => true;
        public override bool HasImageSource => false;
        public override BitmapImage? GetImageSource(ImageOptions? options = null)
        {
            return null;
        }
        public override object? GetImageObject(ImageOptions? options = null)
        {
            return null;
        }
        public override string ToString()
        {
            return nameof(DefaultImageReference);
        }
    }
}
