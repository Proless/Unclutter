using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Unclutter.Prism;
using Unclutter.Views;

namespace Unclutter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApp
    {
        /* Methods */
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterRequirements();
        }
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new AggregateDirectoryCatalog(".\\extensions");
        }
    }
}
