using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Plugins;

namespace Unclutter.ViewModels
{
    public class StartupActionsViewModel : ViewModelBase, IPluginConsumer
    {
        /* Properties */
        public SynchronizedObservableCollection<IStartupAction> StartupActions { get; set; }

        public ImportOptions Options => new ImportOptions();

        [ImportMany(typeof(IStartupAction))]
        public IEnumerable<IStartupAction> StartupPlugins { get; set; }

        /* Constructor */
        public StartupActionsViewModel() { }

        /* Methods */
        public void OnImportsSatisfied()
        {
            StartupActions = new SynchronizedObservableCollection<IStartupAction>(StartupPlugins.OrderBy(p => p.Priority));

            RaisePropertyChanged(nameof(StartupActions));
        }
    }
}
