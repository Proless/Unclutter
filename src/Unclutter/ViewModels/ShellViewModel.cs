using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.Plugins;
using Unclutter.Modules.Plugins.AppWindowCommand;
using Unclutter.Modules.Plugins.AppWindowFlyout;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.App;
using Unclutter.SDK.Events;
using Unclutter.SDK.Models;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Services;

namespace Unclutter.ViewModels
{
    public class ShellViewModel : BaseViewModel, IPluginConsumer, IAppWindowServices, IHandler<ProfileLoadedEvent>
    {
        /* Fields */
        private SynchronizedObservableCollection<IAppWindowCommand> _appWindowCommands;
        private SynchronizedObservableCollection<IAppWindowFlyout> _appWindowFlyouts;
        private IUserProfile _selectedProfile;

        /* Properties */
        [ImportMany]
        public SynchronizedObservableCollection<IAppWindowCommand> AppWindowCommands
        {
            get => _appWindowCommands;
            set => SetProperty(ref _appWindowCommands, value);
        }
        [ImportMany]
        public SynchronizedObservableCollection<IAppWindowFlyout> AppWindowFlyouts
        {
            get => _appWindowFlyouts;
            set => SetProperty(ref _appWindowFlyouts, value);
        }
        public IUserProfile SelectedProfile
        {
            get => _selectedProfile;
            set => SetProperty(ref _selectedProfile, value);
        }
        public ImportOptions Options => new ImportOptions();
        public HandlerOptions HandlerOptions => new HandlerOptions();

        /* Constructor */
        public ShellViewModel()
        {
            AppWindowCommands = new SynchronizedObservableCollection<IAppWindowCommand>();
            AppWindowFlyouts = new SynchronizedObservableCollection<IAppWindowFlyout>();
        }

        /* Methods */
        public void OnImportsSatisfied()
        {
            var appWindowPlugins = new List<IAppWindowPlugin>();

            appWindowPlugins.AddRange(_appWindowCommands);
            appWindowPlugins.AddRange(_appWindowFlyouts);

            foreach (var appWindowPlugin in appWindowPlugins)
            {
                appWindowPlugin.Initialize();
            }

            AppWindowCommands = new SynchronizedObservableCollection<IAppWindowCommand>(OrderHelper.GetOrderedDescending(appWindowPlugins.OfType<IAppWindowCommand>()));
            AppWindowFlyouts = new SynchronizedObservableCollection<IAppWindowFlyout>(appWindowPlugins.OfType<IAppWindowFlyout>());
        }
        public void Handle(ProfileLoadedEvent @event)
        {
            SetProfile(@event.NewProfile);
        }

        /* Helpers */
        private void SetProfile(IUserProfile profile)
        {
            if (profile == SelectedProfile) return;

            SelectedProfile = profile;
            Title = $"{Constants.Unclutter} - {SelectedProfile.Name}";
        }

        #region IAppWindowServices
        public bool IsFlyoutOpen(string name)
        {
            var flyout = AppWindowFlyouts.FirstOrDefault(f => f.Name == name);
            if (flyout != null)
            {
                return flyout.IsOpen;
            }

            return false;
        }
        public void OpenFlyout(string name)
        {
            var flyout = AppWindowFlyouts.FirstOrDefault(f => f.Name == name);
            if (flyout != null)
            {
                flyout.IsOpen = true;
            }
        }
        public void CloseFlyout(string name)
        {
            var flyout = AppWindowFlyouts.FirstOrDefault(f => f.Name == name);
            if (flyout != null)
            {
                flyout.IsOpen = false;
            }
        }
        public void NavigateToAppView(AppView view)
        {
            // TODO:
        }
        #endregion
    }
}
