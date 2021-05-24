using Ookii.Dialogs.Wpf;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unclutter.Modules.Commands;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Validation;
using Unclutter.Services.Profiles;

namespace Unclutter.ViewModels.ProfilesManagement
{
    public class ProfilesCreationViewModel : ValidationViewModelBase
    {
        /* Services */
        private readonly IProfilesManager _profilesManager;
        private readonly IDirectoryService _directoryService;
        private readonly IDialogProvider _dialogProvider;
        private readonly ILogger _logger;

        /* Fields */
        private IUserDetails _selectedUser;
        private IGameDetails _selectedGame;
        private string _profileName;
        private string _downloadsLocation;
        private SynchronizedObservableCollection<IUserDetails> _users;

        /* Properties */
        [Required(AllowEmptyStrings = false, ErrorMessageKey = ResourceKeys.Profile_Name_Msg_Required)]
        [FileName(ErrorMessageKey = ResourceKeys.Profile_Name_Msg_Invalid)]
        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }

        [Required(AllowEmptyStrings = false, ErrorMessageKey = ResourceKeys.DFolder_Msg_Required)]
        [FilePath(ErrorMessageKey = ResourceKeys.DFolder_Msg_Invalid)]
        public string DownloadsLocation
        {
            get => _downloadsLocation;
            set => SetProperty(ref _downloadsLocation, value);
        }

        public SynchronizedObservableCollection<IUserDetails> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public IUserDetails SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }
        public IGameDetails SelectedGame
        {
            get => _selectedGame;
            set => SetProperty(ref _selectedGame, value);
        }

        public bool IsUserSelected => _selectedUser != null;
        public bool IsGameSelected => _selectedGame != null;

        /* Commands */
        public DelegateCommand<string> BrowseUrlCommand => new DelegateCommand<string>(BrowseUrl);
        public DelegateCommand BrowseFolderCommand => new DelegateCommand(BrowseFolder);
        public DelegateCommand CreateProfileCommand => new AsyncDelegateCommand(CreateProfile, CanCreateProfile)
            .ObservesProperty(() => ProfileName)
            .ObservesProperty(() => DownloadsLocation)
            .ObservesProperty(() => IsUserSelected)
            .ObservesProperty(() => IsGameSelected);

        /* Constructor */
        public ProfilesCreationViewModel(IProfilesManager profilesManager, IDirectoryService directoryService, ILoggerProvider loggerProvider, IDialogProvider dialogProvider)
        {
            _profilesManager = profilesManager;
            _directoryService = directoryService;
            _dialogProvider = dialogProvider;
            _logger = loggerProvider.GetInstance();

            Users = new SynchronizedObservableCollection<IUserDetails>();

            EventAggregator.GetEvent<GameSelectedEvent>().Subscribe(g =>
            {
                SelectedGame = g;
                RaisePropertyChanged(nameof(IsGameSelected));
            });
            EventAggregator.GetEvent<UserSelectedEvent>().Subscribe(u =>
            {
                var existingUser = Users.FirstOrDefault(x => x.Id == u.Id);
                if (existingUser != null)
                {
                    Users.Remove(existingUser);
                }
                Users.Add(u);
                SelectedUser = u;
                RaisePropertyChanged(nameof(IsUserSelected));
            });
        }

        /* Methods */
        public void BrowseUrl(string url)
        {
            var psi = new ProcessStartInfo { UseShellExecute = true, FileName = url };
            Process.Start(psi);
        }

        public void BrowseFolder()
        {
            var browseDlg = new VistaFolderBrowserDialog
            {
                Description = LocalizationProvider.GetLocalizedString(ResourceKeys.DFolder_Msg_Select),
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true
            };

            if (browseDlg.ShowDialog().GetValueOrDefault() && Directory.Exists(browseDlg.SelectedPath))
            {
                DownloadsLocation = browseDlg.SelectedPath;
            }
        }

        public bool CanCreateProfile()
        {
            return !HasErrors
                   && !string.IsNullOrWhiteSpace(ProfileName)
                   && !string.IsNullOrWhiteSpace(DownloadsLocation)
                   && IsGameSelected
                   && IsUserSelected;
        }

        public async Task CreateProfile()
        {
            try
            {
                _directoryService.EnsureDirectoryAccess(DownloadsLocation);
            }
            catch (Exception ex)
            {

                var msg = string.Format(LocalizationProvider.GetLocalizedString(ResourceKeys.Folder_Msg_Creation_Error), DownloadsLocation);

                _logger.Error(ex, msg);

                await _dialogProvider.Message(LocalizationProvider.GetLocalizedString(ResourceKeys.Error), msg)
                    .Icon(DialogIcon.Error)
                    .LeftButton(LocalizationProvider.GetLocalizedString(ResourceKeys.OK))
                    .Create()
                    .ShowDialogAsync();
                return;
            }

            _profilesManager.Create(ProfileName, DownloadsLocation, SelectedGame, SelectedUser);

            EventAggregator.GetEvent<ProfileCreatedEvent>().Publish();
            Navigate();
        }

        public override void Navigate(string uri = "")
        {
            KeepAlive = false;
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.ProfilesManagement, LocalIdentifiers.Views.Profiles);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.Games, LocalIdentifiers.Views.GamesSelection);
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.Authentication, LocalIdentifiers.Views.Authentication);
            Task.Run(Populate);
        }

        /* Helpers */
        private void Populate()
        {
            Users.Clear();
            Users.AddRange(_profilesManager.EnumerateUsers());
        }
    }
}
