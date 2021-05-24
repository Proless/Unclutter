using Prism.Ioc;
using Prism.Modularity;
using Unclutter.SDK.Plugins;

namespace Unclutter.Modules
{
    public class ModuleInitializerProcessor : ModuleInitializer, IModuleInitializer
    {
        /* Fields */
        private IModule _currentModuleInstance;
        private readonly IPluginProvider _pluginProvider;

        /// <summary>
        /// <inheritdoc cref="ModuleInitializer"/>
        /// </summary>
        public ModuleInitializerProcessor(IContainerExtension containerExtension, IPluginProvider pluginProvider) : base(containerExtension)
        {
            _pluginProvider = pluginProvider;
        }

        /// <summary>
        /// <inheritdoc cref="ModuleInitializer.Initialize"/>
        /// </summary>
        void IModuleInitializer.Initialize(IModuleInfo moduleInfo)
        {
            Initialize(moduleInfo);
            ProcessModule(moduleInfo, _currentModuleInstance);
            _currentModuleInstance = null;
        }

        /// <summary>
        /// <inheritdoc cref="ModuleInitializer.CreateModule(IModuleInfo)"/>
        /// </summary>
        protected override IModule CreateModule(IModuleInfo moduleInfo)
        {
            _currentModuleInstance = base.CreateModule(moduleInfo);
            return _currentModuleInstance;
        }

        /// <summary>
        /// Executes the module processors on a module.
        /// 
        /// </summary>
        /// <param name="moduleInfo">The module to create.</param>
        /// <param name="instance">The created <see cref="IModule"/> instance used during initialization.</param>
        protected void ProcessModule(IModuleInfo moduleInfo, IModule instance)
        {
            if (_currentModuleInstance != null)
            {
                foreach (var processor in _pluginProvider.Container.GetExportedValues<IModuleProcessor>())
                {
                    processor.ProcessModule(moduleInfo, instance);
                }
            }
        }
    }
}
