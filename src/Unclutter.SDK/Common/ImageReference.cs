#nullable enable
using System;
using System.ComponentModel;
using System.Reflection;

namespace Unclutter.SDK.Common
{
    /// <summary>
    /// Image reference
    /// </summary>
    [TypeConverter(typeof(ImageReferenceConverter))]
    public readonly struct ImageReference
    {
        /// <summary>
        /// Gets an <see cref="ImageReference"/> which isn't referencing any image
        /// </summary>
        public static readonly ImageReference None = default;

        /// <summary>
        /// true if it's the default instance
        /// </summary>
        public bool IsDefault => Assembly is null && Name is null;

        /// <summary>
        /// Assembly of image or null if <see cref="Name"/> is a URI
        /// </summary>
        public Assembly? Assembly { get; }

        /// <summary>
        /// Name of image
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assembly">Assembly of image or null if <paramref name="name"/> is a pack: URI</param>
        /// <param name="name">Name of image</param>
        public ImageReference(Assembly? assembly, string name)
        {
            Assembly = assembly;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Parses a string and tries to create an <see cref="ImageReference"/>
        /// </summary>
        /// <param name="value">String to parse</param>
        /// <param name="result">Result</param>
        /// <returns></returns>
        public static bool TryParse(string? value, out ImageReference result)
        {
            result = default;
            switch (value)
            {
                case null:
                    return false;
                case "":
                    return true;
            }

            if (value.StartsWith("/", StringComparison.OrdinalIgnoreCase) ||
                value.StartsWith("pack:", StringComparison.OrdinalIgnoreCase) ||
                value.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
            {
                result = new ImageReference(null, value);
                return true;
            }

            var index = value.IndexOf(',');
            if (index < 0)
            {
                result = new ImageReference(null, value);
                return true;
            }
            var asmName = value.Substring(0, index).Trim();
            var name = value.Substring(index + 1).Trim();
            Assembly asm;
            try
            {
                asm = Assembly.Load(asmName);
            }
            catch
            {
                return false;
            }
            result = new ImageReference(asm, name);
            return true;
        }

        /// <summary>
        /// Converts this instance to a string that can be passed to <see cref="TryParse(string, out ImageReference)"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsDefault)
                return string.Empty;
            if (Assembly is null)
                return Name;
            return Assembly.GetName().Name + "," + Name;
        }
    }
}
