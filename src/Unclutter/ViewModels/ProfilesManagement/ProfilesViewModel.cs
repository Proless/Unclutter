using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unclutter.Modules.Commands;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.Events;
using Unclutter.SDK.Models;
using Unclutter.SDK.Services;
using Unclutter.Services.Profiles;

namespace Unclutter.ViewModels.ProfilesManagement
{
    public class ProfilesViewModel : BaseViewModel, IHandler<ProfileCreatedEvent>
    {
        /* Services */
        private readonly IProfilesManager _profilesManager;
        private readonly IDialogProvider _dialogProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;

        /* Fields */
        private SynchronizedObservableCollection<IUserProfile> _profiles;

        /* Properties */
        public SynchronizedObservableCollection<IUserProfile> Profiles
        {
            get => _profiles;
            set => SetProperty(ref _profiles, value);
        }
        public HandlerOptions HandlerOptions => new HandlerOptions();

        /* Commands */
        public DelegateCommand<IUserProfile> DeleteProfileCommand => new AsyncDelegateCommand<IUserProfile>(DeleteProfile);
        public DelegateCommand<IUserProfile> LoadProfileCommand => new DelegateCommand<IUserProfile>(LoadProfile);

        /* Constructor */
        public ProfilesViewModel(IProfilesManager profilesManager, IDialogProvider dialogProvider, ILogger logger, IEventAggregator eventAggregator)
        {
            _profilesManager = profilesManager;
            _dialogProvider = dialogProvider;
            _eventAggregator = eventAggregator;
            _logger = logger;

            Profiles = new SynchronizedObservableCollection<IUserProfile>();
            Populate();
        }

        /* Methods */
        public void Populate()
        {
            Profiles.Clear();
            Profiles.AddRange(_profilesManager.EnumerateProfiles());
        }

        public async Task DeleteProfile(IUserProfile profile)
        {
            if (profile.Id == _profilesManager.CurrentProfile?.Id)
            {
                await _dialogProvider
                    .Message("", LocalizationProvider.GetLocalizedString(ResourceKeys.Profile_Msg_Delete_Error, profile.Name))
                    .LeftButton(LocalizationProvider.GetLocalizedString(ResourceKeys.OK))
                    .Icon(IconType.Warning)
                    .Create()
                    .ShowDialogAsync();

                return;
            }

            await _dialogProvider
                .Message("", LocalizationProvider.GetLocalizedString(ResourceKeys.Profile_Msg_Delete_Confirm, profile.Name))
                .LeftButton(LocalizationProvider.GetLocalizedString(ResourceKeys.Yes))
                .RightButton(LocalizationProvider.GetLocalizedString(ResourceKeys.No))
                .Icon(IconType.Question)
                .Create()
                .ShowDialogAsync((_, action) =>
                {
                    if (action != DialogAction.LeftButton) return;

                    try
                    {
                        _profilesManager.Delete(profile);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Error encountered deleting the profile {profile.Name}");
                    }
                    finally
                    {
                        Profiles.Remove(profile);
                    }
                });
        }

        public void LoadProfile(IUserProfile profile)
        {
            if (profile != null && profile.IsValid())
            {
                var oldProfile = _profilesManager.CurrentProfile;
                _profilesManager.LoadProfile(profile);
                _eventAggregator.PublishOnCurrentThread(new CloseStartupDialogEvent(new DialogResult(ButtonResult.OK)));
                _eventAggregator.PublishOnCurrentThread(new ProfileLoadedEvent(profile, oldProfile));
            }
        }

        public void Handle(ProfileCreatedEvent @event)
        {
            Populate();
        }
    }
}
