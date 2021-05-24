using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Events;
using Unclutter.SDK.Plugins;
using Unclutter.Services;
using Unclutter.Services.Localization;

namespace Unclutter.ViewModels.Dialogs
{
    public class StartupDialogViewModel : ViewModelBase, IDialogAware, IHandler<ProfileChangedEvent>
    {
        /* Fields */
        private IStartupAction _currentStartupAction;
        private IRegion _region;
        private bool _canGoBack;

        /* Properties */
        public override string Title { get; set; } = LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Unclutter_Startup);
        public bool AutoSubscribe => true;
        public IRegion Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }
        public bool CanGoBack
        {
            get => _canGoBack;
            set => SetProperty(ref _canGoBack, value);
        }
        public IStartupAction CurrentStartupAction
        {
            get => _currentStartupAction;
            set => SetProperty(ref _currentStartupAction, value);
        }

        /* Commands */
        public DelegateCommand NavigateBackwardsCommand => new DelegateCommand(NavigateBackwards);

        /* Methods */
        public void NavigateBackwards()
        {
            Region.NavigationService.Journal.GoBack();
        }

        public Task HandleAsync(ProfileChangedEvent @event, CancellationToken cancellationToken)
        {
            var result = ButtonResult.Cancel;

            if (@event.NewProfile != null)
            {
                result = ButtonResult.OK;
            }

            return UIDispatcher.OnUIThreadAsync(() => RequestClose?.Invoke(new DialogResult(result)));
        }

        #region IDialogAware
        public event Action<IDialogResult> RequestClose;
        public bool CanCloseDialog() => true;
        public void OnDialogClosed()
        {
            RegionManager.Regions.Remove(CommonIdentifiers.Regions.Startup);
            RegionManager.Regions.Remove(LocalIdentifiers.Regions.ProfilesManagement);
            RegionManager.Regions.Remove(LocalIdentifiers.Regions.StartupActions);
            RegionManager.Regions.Remove(LocalIdentifiers.Regions.Games);
            RegionManager.Regions.Remove(LocalIdentifiers.Regions.Authentication);
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Region = RegionManager.Regions[CommonIdentifiers.Regions.Startup];
            Region.NavigationService.Navigated += (_, _) =>
            {
                CanGoBack = Region.NavigationService.Journal.CanGoBack;
            };

            RegionManager.RequestNavigate(CommonIdentifiers.Regions.Startup, LocalIdentifiers.Views.Startup);
        }
        #endregion

    }
}
