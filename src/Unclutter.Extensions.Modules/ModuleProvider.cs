using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using Unclutter.Extensions.Modules.Attributes;

namespace Unclutter.Extensions.Modules
{
    public class ModuleProvider : IModuleProvider
    {
        /* Fields */
        private readonly IModuleManager _moduleManager;
        private readonly IContainerExtension _containerRegistry;
        private readonly List<IModuleEntry> _moduleEntries = new List<IModuleEntry>();

        /* Properties */
        public IEnumerable<IModuleEntry> Modules => _moduleEntries;

        /* Constructors */
        public ModuleProvider(IModuleManager moduleManager, IContainerExtension containerExtension)
        {
            _moduleManager = moduleManager;
            _containerRegistry = containerExtension;
            GetModules();
        }

        /* Methods */
        public void AddModule(IModuleEntry module)
        {
            if (module == null) return;

            RegisterModuleViews(module);
            _moduleEntries.Add(module);
        }
        public object ResolveView(IModuleView moduleView)
        {
            return _containerRegistry.Resolve(moduleView.ViewType);
        }
        public void RegisterView(IModuleView moduleView)
        {
            InternalRegisterView(moduleView);
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
        }
        private ModuleEntry CreateModuleEntry(IModuleInfo moduleInfo, IModuleMetadata metadata)
        {
            return new ModuleEntry()
            {
                Name = metadata.Name,
                Author = metadata.Author,
                Description = metadata.Description,
                Version = metadata.Version,
                ModuleLocation = moduleInfo.Ref,
                Views = GetViews<ModuleViewAttribute>(moduleInfo),
                SettingViews = GetViews<ModuleSettingsViewAttribute>(moduleInfo)
            };
        }
        private void RegisterModuleViews(IModuleEntry module)
        {
            var views = module.Views;
            var settings = module.SettingViews;
            foreach (var view in views.Union(settings))
            {
                InternalRegisterView(view);
            }
        }
        private void InternalRegisterView(IModuleView view)
        {
            if (view is null) return;
            _containerRegistry.RegisterForNavigation(view.ViewType, view.ViewType.GUID.ToString());
        }
        private IEnumerable<IModuleView> GetViews<T>(IModuleInfo moduleInfo) where T : Attribute, IModuleView
        {
            var views = new List<IModuleView>();
            try
            {
                var moduleType = Type.GetType(moduleInfo.ModuleType);
                var moduleViews = moduleType.GetCustomAttributes<T>()
                    .Where(t => t.ViewType != null)
                    .Select(v => new ModuleView(v.ViewType, v.Label, v.IconId, v.GroupName, v.ItemPriority));
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
