using System;
using System.Collections.Generic;
using System.IO;
using Prism.Modularity;

namespace Unclutter.Prism
{
    public class AggregateDirectoryCatalog : ModuleCatalog
    {
        private readonly DirectoryInfo _modulesDirectoryInfo;
        public string ModulesDirectory { get; }

        public AggregateDirectoryCatalog(string directory)
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
                var dirCatalog = new DirectoryModuleCatalog() { ModulePath = dir.FullName };
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
            var dirs = new List<DirectoryInfo>();
            var options = new EnumerationOptions()
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false,
                ReturnSpecialDirectories = false
            };

            // Add the root modules directory
            dirs.Add(_modulesDirectoryInfo);

            // Add all top level subdirectories inside the root directory
            dirs.AddRange(_modulesDirectoryInfo.EnumerateDirectories("*", options));

            return dirs;
        }
    }
}
