using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Unclutter.Modules
{
    public class ModuleProvider : IModuleProvider
    {
        /* Fields */
        private readonly IModuleManager _moduleManager;
        private readonly IContainerExtension _containerRegistry;
        private readonly List<IModuleEntry> _moduleEntries = new List<IModuleEntry>();

        /* Properties */
        public IEnumerable<IModuleEntry> Modules => _moduleEntries;
        public event Action<IModuleEntry> ModuleAdded;

        /* Constructors */
        public ModuleProvider(IModuleManager moduleManager, IContainerExtension containerExtension)
        {
            _moduleManager = moduleManager;
            _containerRegistry = containerExtension;
            moduleManager.LoadModuleCompleted += OnModuleLoadCompleted;
            GetModules();
        }

        private void OnModuleLoadCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                InternalAddModule(e.ModuleInfo);
            }
        }

        /* Helpers */
        private void GetModules()
        {
            foreach (var moduleInfo in _moduleManager.Modules)
            {
                InternalAddModule(moduleInfo);
            }
        }

        private void InternalAddModule(IModuleInfo moduleInfo)
        {
            var module = Type.GetType(moduleInfo.ModuleType).GetCustomAttributes<ModuleMetadataAttribute>().FirstOrDefault();
            if (module == null) return;

            var entry = CreateModuleEntry(moduleInfo, module);
            RegisterModuleViews(entry);
            _moduleEntries.Add(entry);
            ModuleAdded?.Invoke(entry);
        }

        private ModuleEntry CreateModuleEntry(IModuleInfo moduleInfo, IModuleMetadata metadata)
        {
            return new ModuleEntry
            {
                ModuleLocation = moduleInfo.Ref,
                Metadata = metadata,
                Views = GetViews<ExportModuleViewAttribute>(moduleInfo)
            };
        }

        private void RegisterModuleViews(IModuleEntry module)
        {
            foreach (var view in module.Views)
            {
                _containerRegistry.RegisterForNavigation(view.ViewType, view.ViewType.GUID.ToString());
            }
        }

        private IEnumerable<IModuleView> GetViews<T>(IModuleInfo moduleInfo) where T : Attribute, IModuleView
        {
            var views = new List<IModuleView>();
            try
            {
                var moduleType = Type.GetType(moduleInfo.ModuleType);
                var moduleViews = moduleType.GetCustomAttributes<T>()
                    .Where(t => t.ViewType != null)
                    .Select(v => new ModuleView
                    {
                        ViewType = v.ViewType,
                        IconRef = v.IconRef,
                        Label = v.Label,
                        Priority = v.Priority
                    });
                views.AddRange(moduleViews);
            }
            catch (Exception)
            {
                return Enumerable.Empty<IModuleView>();
            }
            return views;
        }
    }
}
