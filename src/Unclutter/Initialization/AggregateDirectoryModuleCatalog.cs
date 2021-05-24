using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.IO;

namespace Unclutter.Initialization
{
    public class AggregateDirectoryModuleCatalog : ModuleCatalog
    {
        private readonly DirectoryInfo _modulesDirectoryInfo;
        public string ModulesDirectory { get; }

        public AggregateDirectoryModuleCatalog(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentException("The directory path can't be empty, whitespace or null", nameof(directory));

            _modulesDirectoryInfo = new DirectoryInfo(Path.GetFullPath(directory));
            if (!_modulesDirectoryInfo.Exists) _modulesDirectoryInfo.Create();

            ModulesDirectory = _modulesDirectoryInfo.FullName;
            PopulateModules();
        }

        private void PopulateModules()
        {
            var dirs = GetDirectories();

            foreach (var dir in dirs)
            {
                var dirCatalog = new DirectoryModuleCatalog { ModulePath = dir.FullName };
                try
                {
                    dirCatalog.Initialize();
                    foreach (var moduleInfo in dirCatalog.Modules)
                    {
                        AddModule(moduleInfo);
                    }
                }
                catch
                {
                    // Skip faulty modules
                }
            }
        }

        private IEnumerable<DirectoryInfo> GetDirectories()
        {
            // Add the root modules directory
            var dirs = new List<DirectoryInfo> { _modulesDirectoryInfo };

            // Add all top level subdirectories inside the root modules directory
            dirs.AddRange(_modulesDirectoryInfo.EnumerateDirectories("*.*", SearchOption.TopDirectoryOnly));

            return dirs;
        }
    }
}
