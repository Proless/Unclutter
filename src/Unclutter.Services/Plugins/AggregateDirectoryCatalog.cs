using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Unclutter.Services.Plugins
{
    public class AggregateDirectoryCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged, ICompositionElement
    {
        /* Fields */
        private AggregateCatalog _aggregateCatalog;
        private readonly string _path;

        /* Events */
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

        /* Properties */
        string ICompositionElement.DisplayName => GetDisplayName();
        ICompositionElement ICompositionElement.Origin => null;
        public override IQueryable<ComposablePartDefinition> Parts => _aggregateCatalog.Parts;

        /* Constructor */
        public AggregateDirectoryCatalog(string path) : this(path, "*.dll") { }
        public AggregateDirectoryCatalog(string path, string searchPattern)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException(nameof(path));

            _path = path;

            Initialize(path, searchPattern);
        }

        /* Methods */
        public override string ToString()
        {
            return GetDisplayName();
        }

        /* Helpers */
        private void Initialize(string path, string searchPattern)
        {
            var directoryCatalogs = GetFoldersRecursive(path).Select(dir => new DirectoryCatalog(dir, searchPattern));
            _aggregateCatalog = new AggregateCatalog();
            _aggregateCatalog.Changed += (o, e) => Changed?.Invoke(o, e);
            _aggregateCatalog.Changing += (o, e) => Changing?.Invoke(o, e);

            foreach (var catalog in directoryCatalogs)
            {
                _aggregateCatalog.Catalogs.Add(catalog);
            }
        }
        private string GetDisplayName()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0} (Path=\"{1}\")", GetType().Name, _path);
        }

        #region Static
        private static IEnumerable<string> GetFoldersRecursive(string path)
        {
            var result = new List<string> { path };
            foreach (var child in Directory.GetDirectories(path))
            {
                result.AddRange(GetFoldersRecursive(child));
            }
            return result;
        }
        #endregion
    }
}
