#nullable enable
using System;

namespace Unclutter.SDK.Images
{
    /// <summary>
    /// An <see cref="ImageReference"/> which uses a <see cref="Uri"/> to load images.
    /// </summary>
    public class PackUriImageReference : UriImageReference
    {
        /* Fields */
        private readonly string _packUri;

        /* Properties */
        public override bool HasImageSource => true;
        public override bool IsDefault => false;

        /* Constructor */
        public PackUriImageReference(string packUri)
        {
            if (string.IsNullOrWhiteSpace(packUri))
            {
                throw new ArgumentException("The pack uri can not be Empty, Whitespace or null !");
            }

            _packUri = packUri;
        }

        /* Methods */
        public override Uri GetImageUri()
        {
            Uri uri;
            if (!_packUri.StartsWith("pack://", StringComparison.OrdinalIgnoreCase))
            {
                uri = new Uri($"pack://application:,,,/{_packUri.Trim('/')}", UriKind.Absolute);
            }
            else
            {
                uri = new Uri(_packUri, UriKind.RelativeOrAbsolute);
                if (!uri.IsAbsoluteUri)
                {
                    uri = new Uri($"pack://application:,,,/{_packUri.Trim('/')}", UriKind.Absolute);
                }
            }

            return uri;
        }
    }
}
