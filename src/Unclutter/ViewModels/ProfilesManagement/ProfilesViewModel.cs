using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Unclutter.Modules.Commands;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;
using Unclutter.Services.Localization;
using Unclutter.Services.Profiles;

namespace Unclutter.ViewModels.ProfilesManagement
{
    public class ProfilesViewModel : ViewModelBase
    {
        /* Services */
        private readonly IProfilesManager _profilesManager;
        private readonly IDialogProvider _dialogProvider;
        private readonly ILogger _logger;

        /* Fields */
        private SynchronizedObservableCollection<IUserProfile> _profiles;

        /* Properties */
        public SynchronizedObservableCollection<IUserProfile> Profiles
        {
            get => _profiles;
            set => SetProperty(ref _profiles, value);
        }

        /* Commands */
        public DelegateCommand<IUserProfile> DeleteProfileCommand => new AsyncDelegateCommand<IUserProfile>(DeleteProfile);
        public DelegateCommand<IUserProfile> SelectProfileCommand => new DelegateCommand<IUserProfile>(SelectProfile);

        /* Constructor */
        public ProfilesViewModel(IProfilesManager profilesManager, IDialogProvider dialogProvider, ILoggerProvider loggerProvider)
        {
            _profilesManager = profilesManager;
            _dialogProvider = dialogProvider;
            _logger = loggerProvider.GetInstance();

            Profiles = new SynchronizedObservableCollection<IUserProfile>();

            EventAggregator.GetEvent<ProfileCreatedEvent>().Subscribe(Populate);
        }

        /* Methods */
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Populate();
        }

        public async Task DeleteProfile(IUserProfile profile)
        {
            if (profile.Id == _profilesManager.CurrentProfile?.Id)
            {
                await _dialogProvider
                    .Message("", string.Format(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Profile_Msg_Delete_Error), profile.Name))
                    .LeftButton(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.OK))
                    .Icon(DialogIcon.Warning)
                    .Create()
                    .ShowDialogAsync();

                return;
            }


            await _dialogProvider
                .Message("", string.Format(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Profile_Msg_Delete_Confirm), profile.Name))
                .LeftButton(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Yes))
                .RightButton(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.No))
                .Icon(DialogIcon.Question)
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

        public void SelectProfile(IUserProfile profile)
        {
            if (profile != null && profile.IsValid())
            {
                _profilesManager.ChangeProfile(profile);
            }
        }

        /* Helpers */
        private void Populate()
        {
            Profiles.Clear();
            Profiles.AddRange(_profilesManager.EnumerateProfiles());
        }
    }
}
