using Prism.Commands;
using System;
using System.Threading.Tasks;
using Unclutter.Modules.Commands;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;

namespace Modules.Profiles.ViewModels
{
    public class ProfilesViewModel : ViewModelBase
    {
        /* Services */
        private readonly ILogger _logger;
        private readonly IDialogProvider _dialogProvider;

        /* Data fields */
        private SynchronizedObservableCollection<IUserProfile> _profiles;

        /* Properties */
        public SynchronizedObservableCollection<IUserProfile> Profiles
        {
            get => _profiles;
            set => SetProperty(ref _profiles, value);
        }

        /* Commands */
        public DelegateCommand<IUserProfile> DeleteProfileCommand => new AsyncDelegateCommand<IUserProfile>(DeleteProfile);

        /* Constructors */
        public ProfilesViewModel(ILoggerProvider loggerProvider, IDialogProvider dialogProvider)
        {
            _logger = loggerProvider.GetInstance();
            _dialogProvider = dialogProvider;
        }

        /* Methods */

        /* Helpers */
        private async Task DeleteProfile(IUserProfile profile)
        {
            await _dialogProvider
                .Message("Confirm profile deletion", $"Are you sure you want to delete the profile {profile.Name}")
                .LeftButton("Yes")
                .RightButton("No")
                .Create()
                .ShowDialogAsync((_, action) =>
                {
                    if (action != DialogAction.LeftButton) return;

                    try
                    {

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
    }
}