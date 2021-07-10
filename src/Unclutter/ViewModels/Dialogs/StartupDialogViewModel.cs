using Prism.Services.Dialogs;
using System;
using System.ComponentModel.Composition;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Events;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Services;

namespace Unclutter.ViewModels.Dialogs
{
    public class StartupDialogViewModel : BaseViewModel, IDialogAware, IPluginConsumer, IHandler<CloseStartupDialogEvent>
    {
        /* Fields */
        private SynchronizedObservableCollection<IStartupAction> _startupAction;

        /* Properties */
        public override string Title => LocalizationProvider.GetLocalizedString(ResourceKeys.Unclutter_Startup);
        public HandlerOptions HandlerOptions => new HandlerOptions { AutoSubscribeThread = ThreadOption.UIThread };
        public ImportOptions Options => new ImportOptions();
        [ImportMany]
        public SynchronizedObservableCollection<IStartupAction> StartupActions
        {
            get => _startupAction;
            set => SetProperty(ref _startupAction, value);
        }

        /* Methods */
        public void Handle(CloseStartupDialogEvent @event)
        {
            RequestClose?.Invoke(@event.Result);
        }
        public void OnImportsSatisfied()
        {
            StartupActions = new SynchronizedObservableCollection<IStartupAction>(OrderHelper.GetOrdered(StartupActions));
            foreach (var startupAction in StartupActions)
            {
                startupAction.Initialize();
            }
        }

        #region IDialogAware
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {
            RegionManager.Regions.Remove(CommonIdentifiers.Regions.Startup);
            RegionManager.Regions.Remove(LocalIdentifiers.Regions.Games);
            RegionManager.Regions.Remove(LocalIdentifiers.Regions.Authentication);
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            RegionManager.RequestNavigate(CommonIdentifiers.Regions.Startup, LocalIdentifiers.Views.Profiles);
        }
        #endregion
    }
}
