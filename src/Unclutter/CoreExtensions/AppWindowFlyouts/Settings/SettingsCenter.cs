using Prism.Commands;
using Prism.Regions;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.Plugins.AppWindowBar;
using Unclutter.Modules.Plugins.AppWindowFlyout;
using Unclutter.SDK;
using Unclutter.SDK.App;
using Unclutter.SDK.Services;

namespace Unclutter.CoreExtensions.AppWindowFlyouts.Settings
{
    [ExportAppWindowFlyout]
    [Export(typeof(ISettingsCenter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SettingsCenter : BaseAppWindowFlyout, ISettingsCenter
    {
        /* Fields */
        private readonly IAppNavigator _appNavigator;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly IRegionManager _regionManager;
        private IRegion _settingsRegion;
        private SettingsView _selectedSettingsView;
        private SynchronizedObservableCollection<SettingsView> _settingsViews;

        /* Properties */
        public override string Name => LocalIdentifiers.Flyouts.Settings;
        public SynchronizedObservableCollection<SettingsView> SettingsViews
        {
            get => _settingsViews;
            set => SetProperty(ref _settingsViews, value);
        }
        public SettingsView SelectedSettingsView
        {
            get => _selectedSettingsView;
            set => SetProperty(ref _selectedSettingsView, value);
        }

        /* Commands */
        public DelegateCommand<SettingsView> SelectedSettingsViewCommand =>
            new DelegateCommand<SettingsView>(OnSelectedSettingsViewChanged);

        public DelegateCommand RestoreAllDefaultSettingsCommand => new DelegateCommand(RestoreAllDefaultSettings);

        /* Constructor */
        [ImportingConstructor]
        public SettingsCenter(IAppNavigator appNavigator, ILocalizationProvider localizationProvider, IRegionManager regionManager)
        {
            _appNavigator = appNavigator;
            _localizationProvider = localizationProvider;
            _regionManager = regionManager;
        }

        /* Methods */
        public override void Initialize()
        {
            VerticalContentAlignment = VerticalAlignment.Stretch;
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            Position = AppWindowFlyoutPosition.Right;
            Content = new SettingsCenterView(_regionManager)
            {
                DataContext = this
            };
            ShowCloseButton = true;
            Title = _localizationProvider.GetLocalizedString(ResourceKeys.Settings);

            _settingsRegion = _regionManager.Regions[LocalIdentifiers.Regions.Settings];

            PopulateSettingsViews();
        }
        public override void OnClosed()
        {
            foreach (var view in SettingsViews)
            {
                view.Settings.Save();
            }
        }
        public override void OnOpened()
        {
        }

        /* Helpers */
        private void RestoreAllDefaultSettings()
        {
            foreach (var settingsView in SettingsViews)
            {
                settingsView.Settings.Reset();
            }
            Refresh();
        }
        private void PopulateSettingsViews()
        {
            var views = _appNavigator.SettingsViews.ToArray();
            var singleSettingsViews = OrderHelper.GetOrdered(views.Where(v => v.GetType() != typeof(GroupSettingsView)));
            var groupSettingsViews = OrderHelper.GetOrdered(views.OfType<GroupSettingsView>());

            var settingsViews = singleSettingsViews.Concat(groupSettingsViews).ToArray();

            SettingsViews = new SynchronizedObservableCollection<SettingsView>(OrderHelper.GetOrdered(settingsViews));
        }
        private void OnSelectedSettingsViewChanged(SettingsView view)
        {
            if (view != null)
            {
                _regionManager.RequestNavigate(LocalIdentifiers.Regions.Settings, view.Identifier);
            }
        }
        private void Refresh()
        {
            foreach (var activeView in _settingsRegion.ActiveViews)
            {
                _settingsRegion.Deactivate(activeView);
            }

            _settingsRegion.RemoveAll();
            OnSelectedSettingsViewChanged(SelectedSettingsView);
        }
        public void Open()
        {
            IsOpen = true;
        }
        public void Close()
        {
            IsOpen = false;
        }
        public void NavigateToSettingsView(SettingsView view)
        {
            // TODO:
        }
    }
}
