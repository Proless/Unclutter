using DryIoc;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows;

namespace Unclutter.Prism
{
    public abstract class PrismApp : PrismApplication
    {
        public const string GenericMessage = "Loading...";

        public event Action<StartupProgressEventArgs> StartupProgressChanged;

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            OnStartupProgressChanged(GenericMessage);
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            OnStartupProgressChanged("Loading modules...");
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            OnStartupProgressChanged(GenericMessage);
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override void ConfigureViewModelLocator()
        {
            OnStartupProgressChanged(GenericMessage);
            base.ConfigureViewModelLocator();
        }
        
        protected override IContainerExtension CreateContainerExtension()
        {
            OnStartupProgressChanged(GenericMessage);
            return base.CreateContainerExtension();
        }

        protected override Rules CreateContainerRules()
        {
            OnStartupProgressChanged(GenericMessage);
            return base.CreateContainerRules();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            OnStartupProgressChanged(GenericMessage);
            return base.CreateModuleCatalog();
        }

        protected override Window CreateShell()
        {
            OnStartupProgressChanged(GenericMessage);
            throw new NotImplementedException();
        }

        protected override void Initialize()
        {
            OnStartupProgressChanged(GenericMessage);
            base.Initialize();
        }

        protected override void InitializeModules()
        {
            OnStartupProgressChanged("Loading modules...");
            base.InitializeModules();
        }

        protected override void InitializeShell(Window shell)
        {
            OnStartupProgressChanged(GenericMessage);
            base.InitializeShell(shell);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            OnStartupProgressChanged(GenericMessage);
            Dispatcher.Invoke(() => base.OnStartup(e));
        }

        /* Helpers */
        protected virtual void OnStartupProgressChanged(string message)
        {
            StartupProgressChanged?.Invoke(new StartupProgressEventArgs(message, 0d));
        }
    }
}
