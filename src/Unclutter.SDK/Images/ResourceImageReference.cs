#nullable enable
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Unclutter.SDK.Images
{
    /// <summary>
    /// Represents a reference to an Image in an assembly. (EmbeddedResource, Resource)
    /// </summary>
    public class ResourceImageReference : StreamImageReference
    {
        #region Static
        /// <summary>
        /// Try to get a resource <see cref="ResourceImageReference"/> from a string
        /// </summary>
        /// <param name="value">The string value to parse</param>
        /// <param name="reference">The created <see cref="ResourceImageReference"/></param>
        public static bool TryParse(string value, out ResourceImageReference? reference)
        {
            reference = null;
            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("img:", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (value.Equals("img:Default", StringComparison.OrdinalIgnoreCase))
            {
                reference = new ResourceImageReference(null, null);
                return true;
            }

            try
            {
                var location = value[value.IndexOf(";", StringComparison.OrdinalIgnoreCase)..];
                var asmName = value.Substring(3, value.IndexOf(",v", StringComparison.OrdinalIgnoreCase));

                if (string.IsNullOrWhiteSpace(location) && string.IsNullOrWhiteSpace(asmName))
                {
                    return false;
                }

                Assembly? asm = null;
                if (!string.IsNullOrWhiteSpace(asmName))
                {
                    asm = Assembly.Load(asmName);
                }

                reference = new ResourceImageReference(asm, location);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /* Properties */
        public override bool IsDefault => ReferencedAssembly == null && Path == null;
        public override bool HasImageSource => true;
        public Assembly? ReferencedAssembly { get; }
        public string? Path { get; }

        /* Constructor */
        public ResourceImageReference(string? path) : this(Assembly.GetCallingAssembly(), path) { }
        public ResourceImageReference(Assembly? referencedAssembly, string? path)
        {
            ReferencedAssembly = referencedAssembly;
            Path = path;
        }

        /* Methods */
        protected override Stream? GetImageStream()
        {
            if (TryGetResourceStream(ReferencedAssembly, Path, out var resourceStream) ||
                TryGetEmbeddedResourceStream(ReferencedAssembly, Path, out resourceStream))
            {
                return resourceStream;
            }

            return null;
        }

        /// <summary>
        /// Generate a string that represents this <see cref="ResourceImageReference"/> instance :
        /// </summary>
        /// <remarks>
        /// <code>img:{AssemblyShortName},v{AssemblyVersion};{ImageLocation}</code> <br/>
        /// if <see cref="IsDefault"/> is <see langword="true"/> :
        /// <code>img:Default</code>
        /// The string can be passed to <see cref="TryParse"/>
        /// </remarks>
        public override string ToString()
        {
            if (IsDefault)
            {
                return "img:Default";
            }

            var asmName = ReferencedAssembly?.GetName();
            return $"img:{asmName?.Name ?? string.Empty},v{asmName?.Version?.ToString() ?? string.Empty};{Path ?? string.Empty}";
        }

        /* Helpers */
        private bool TryGetResourceStream(Assembly? assembly, string? path, out Stream? stream)
        {
            stream = null;
            if (assembly == null || string.IsNullOrWhiteSpace(path)) return false;

            foreach (var binaryResourceFile in assembly.GetManifestResourceNames().Where(n => n.EndsWith(".resources", StringComparison.OrdinalIgnoreCase)))
            {
                ResourceManager? resourceManager = null;
                try
                {
                    var resourceName = binaryResourceFile[..^10]; // 10 = ".resources".Length;
                    resourceManager = new ResourceManager(resourceName, assembly) { IgnoreCase = true };
                    var resourceStream = resourceManager.GetStream(path.TrimStart('/'), CultureInfo.CurrentUICulture);
                    if (resourceStream != null)
                    {
                        stream = resourceStream;
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    resourceManager?.ReleaseAllResources();
                }
            }
            return false;
        }
        private bool TryGetEmbeddedResourceStream(Assembly? assembly, string? path, out Stream? stream)
        {
            stream = null;
            if (assembly == null || string.IsNullOrWhiteSpace(path)) return false;

            var fullName = GetEmbeddedResourceName(assembly, path).ToLowerInvariant();
            var resourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.ToLowerInvariant() == fullName || n.EndsWith(path, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(resourceName))
            {
                try
                {
                    var resourceStream = assembly.GetManifestResourceStream(resourceName);
                    if (resourceStream != null)
                    {
                        stream = resourceStream;
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        private string GetEmbeddedResourceName(Assembly assembly, string path)
        {
            var segments = path.Split('/');
            var name = $"{assembly.GetName().Name}";
            foreach (var segment in segments)
            {
                name += $".{segment}";
            }
            return name;
        }
    }
}
