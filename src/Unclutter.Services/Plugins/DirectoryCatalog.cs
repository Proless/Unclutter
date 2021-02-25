using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Unclutter.Services.Plugins
{
    public class DirectoryCatalog
    {
        /* Fields */
        private readonly string _path;
        private FileSystemWatcher _watcher;
        private List<Assembly> _foundAssemblies = new List<Assembly>();

        /* Events */
        public event EventHandler<PluginsChangedEventArgs> PluginsChanged;

        /* Properties */
        public IEnumerable<Assembly> Assemblies => _foundAssemblies;

        /* Constructors */
        public DirectoryCatalog(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException(nameof(path));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _path = path;

            Initialize();
        }

        /* Methods */
        public void Refresh()
        {
            _watcher.EnableRaisingEvents = false;
            _foundAssemblies = new List<Assembly>();
            Initialize();
        }

        protected void OnPluginsChanged(PluginsChangedEventArgs e)
        {
            PluginsChanged?.Invoke(this, e);
        }

        /* Helpers */
        private void Initialize()
        {
            foreach (var file in Directory.EnumerateFiles(_path, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var assemblyFullName = AssemblyName.GetAssemblyName(file).FullName;
                    _foundAssemblies.Add(Assembly.Load(assemblyFullName));
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            InitializeWatcher();
        }

        private void InitializeWatcher()
        {
            _watcher?.Dispose();
            _watcher = new FileSystemWatcher(_path, "*.dll")
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.Size
            };

            _watcher.Error += OnError;
            _watcher.Changed += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Renamed += OnRenamed;

            _watcher.EnableRaisingEvents = true;
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {

        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {

        }

        private void OnError(object sender, ErrorEventArgs e)
        {

        }
    }

    public class PluginsChangedEventArgs : EventArgs
    {
        public IEnumerable<string> Added { get; }
        public IEnumerable<string> Removed { get; }

        public PluginsChangedEventArgs(IEnumerable<string> added, IEnumerable<string> removed)
        {
            Added = added;
            Removed = removed;
        }
    }
}
