#nullable enable
using System;
using System.IO;
using System.Reflection;

namespace Unclutter.SDK.Images
{
    public class FileImageReference : UriImageReference
    {
        /* Fields */
        private readonly Assembly? _assembly;
        private readonly string _path;

        /* Properties */
        public override bool IsDefault => false;
        public override bool HasImageSource => true;

        /* Constructors */
        public FileImageReference(string path) : this(path, null) { }
        public FileImageReference(string path, bool isRelativeToCallingAssembly) : this(path, null)
        {
            if (isRelativeToCallingAssembly)
            {
                _assembly = Assembly.GetCallingAssembly();
            }
        }
        public FileImageReference(string path, string? basePath)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("File path can not be Empty, Whitespace or null !", nameof(path));
            }

            if (basePath == null)
            {
                _path = !Path.IsPathFullyQualified(path) ? Path.GetFullPath(path) : path;
            }
            else
            {
                _path = Path.GetFullPath(path, basePath);
            }
        }

        /* Methods */
        public override Uri GetImageUri()
        {
            Uri uri;
            if (_assembly == null)
            {
                uri = new Uri(_path);
            }
            else
            {
                var assemblyPath = new Uri(_assembly.Location);
                var basePath = Path.GetDirectoryName(new Uri(assemblyPath.LocalPath).LocalPath);
                uri = basePath != null ? new Uri(Path.GetFullPath(_path, basePath)) : new Uri(Path.GetFullPath(_path));
            }
            return uri;
        }
    }
}
