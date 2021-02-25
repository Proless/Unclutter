using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using Unclutter.SDK.IServices;

namespace Unclutter.Services.WPF
{
    public class DirectoryService : IDirectoryService
    {
        /** Fields **/
        private bool _isInitialized;
        private readonly string[] _dirs = new string[10];

        /** Properties **/
        public string WorkingDirectory
        {
            get => _dirs[0];
            private set => _dirs[0] = value;
        }
        public string LocalsDirectory
        {
            get => _dirs[1];
            private set => _dirs[1] = value;
        }
        public string ExtensionsDirectory
        {
            get => _dirs[2];
            private set => _dirs[2] = value;
        }
        public string DataDirectory
        {
            get => _dirs[3];
            private set => _dirs[3] = value;
        }

        /** Constructors **/
        public DirectoryService()
        {
            WorkingDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            LocalsDirectory = Path.Combine(WorkingDirectory, "locals");
            ExtensionsDirectory = Path.Combine(WorkingDirectory, "extensions");
            DataDirectory = Path.Combine(WorkingDirectory, "data");
        }

        /** Methods **/
        public void InitializeDirectories()
        {
            if (_isInitialized) return;

            // Checking each directory for existing and read/write permissions.
            var errors = new List<string>();
            foreach (var dir in _dirs.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                var hasAccess = EnsureExist(dir) && EnsureRWPermissions(dir);
                if (!hasAccess)
                {
                    errors.Add(dir);
                }
            }

            // Throw an exception if any errors occurred
            if (errors.Any())
            {
                var errorMessage = "Unable to access the following directories:";
                foreach (var error in errors)
                {
                    errorMessage += $"{Environment.NewLine}{error}";
                }
                throw new UnauthorizedAccessException(errorMessage);
            }

            _isInitialized = true;
        }
        public void EnsureDirectoryAccess(string dir)
        {
            var hasAccess = EnsureExist(dir) && EnsureRWPermissions(dir);
            if (!hasAccess)
            {
                throw new UnauthorizedAccessException($"Unable to access the following directory{Environment.NewLine}{dir}");
            }
        }

        /** Helpers **/
        public static bool EnsureRWPermissions(string dir)
        {
            return EnsureReadPermissions(dir) && EnsureWritePermissions(dir);
        }
        public static bool EnsureExist(string dir)
        {
            var output = true;
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception)
            {
                output = false;
            }
            return output;
        }
        public static bool EnsureReadPermissions(string dir)
        {
            return HasPermission(dir, FileSystemRights.Read);
        }
        public static bool EnsureWritePermissions(string dir)
        {
            return HasPermission(dir, FileSystemRights.Write);
        }
        public static bool HasPermission(string dir, FileSystemRights permission)
        {
            var hasAccess = false;
            try
            {
                var dirInfo = new DirectoryInfo(dir);
                var accessControl = dirInfo.GetAccessControl();
                var collection = accessControl.GetAccessRules(true, true, typeof(NTAccount));
                if (collection.Cast<FileSystemAccessRule>().Any(rule => (rule.FileSystemRights & permission) > 0))
                {
                    hasAccess = true;
                }
            }
            catch (Exception)
            {
                hasAccess = false;
            }
            return hasAccess;
        }
    }
}