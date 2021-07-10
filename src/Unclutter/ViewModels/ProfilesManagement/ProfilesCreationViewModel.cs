using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
using Unclutter.SDK.Validation;
using Unclutter.Services.Profiles;

namespace Unclutter.ViewModels.ProfilesManagement
{
    public class ProfilesCreationViewModel : BaseValidationViewModel, IHandler<UserSelectedEvent>, IHandler<GameSelectedEvent>
    {
        /* Services */
        private readonly IProfilesManager _profilesManager;
        private readonly IDirectoryService _directoryService;
        private readonly IDialogProvider _dialogProvider;
        private readonly IClientServices _clientServices;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly ILogger _logger;
        private IRegionNavigationService _navigationService;

        /* Fields */
        private IUserDetails _selectedUser;
        private IGameDetails _selectedGame;
        private string _profileName;
        private string _downloadsLocation;
        private bool _isAuthenticationPopupOpen;
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

        public bool IsAuthenticationPopupOpen
        {
            get => _isAuthenticationPopupOpen;
            set => SetProperty(ref _isAuthenticationPopupOpen, value);
        }

        public HandlerOptions HandlerOptions => new HandlerOptions();

        /* Commands */
        public DelegateCommand OpenAuthenticationPopupCommand => new DelegateCommand(OpenAuthenticationPopup);
        public DelegateCommand BrowseFolderCommand => new DelegateCommand(BrowseFolder);
        public DelegateCommand CreateProfileCommand => new AsyncDelegateCommand(CreateProfile, CanCreateProfile)
            .ObservesProperty(() => ProfileName)
            .ObservesProperty(() => DownloadsLocation)
            .ObservesProperty(() => IsUserSelected)
            .ObservesProperty(() => HasErrors)
            .ObservesProperty(() => IsGameSelected);

        /* Constructor */
        public ProfilesCreationViewModel(IProfilesManager profilesManager,
            IDirectoryService directoryService,
            ILogger logger,
            IDialogProvider dialogProvider,
            IClientServices clientServices,
            ILocalizationProvider localizationProvider)
        {
            _profilesManager = profilesManager;
            _directoryService = directoryService;
            _dialogProvider = dialogProvider;
            _clientServices = clientServices;
            _localizationProvider = localizationProvider;
            _logger = logger;

            Users = new SynchronizedObservableCollection<IUserDetails>();
        }

        /* Methods */
        private void OpenAuthenticationPopup()
        {
            IsAuthenticationPopupOpen = !IsAuthenticationPopupOpen;
        }

        public void BrowseFolder()
        {
            var selectedPath = _clientServices.OpenFolderSelectionDialog(_localizationProvider.GetLocalizedString(ResourceKeys.DFolder_Msg_Select));

            if (!string.IsNullOrWhiteSpace(selectedPath) && Directory.Exists(selectedPath))
            {
                DownloadsLocation = selectedPath;
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

                var msg = LocalizationProvider.GetLocalizedString(ResourceKeys.Folder_Msg_Creation_Error, DownloadsLocation);

                _logger.Error(ex, msg);

                await _dialogProvider.Message(LocalizationProvider.GetLocalizedString(ResourceKeys.Error), msg)
                    .Icon(IconType.Error)
                    .LeftButton(LocalizationProvider.GetLocalizedString(ResourceKeys.OK))
                    .Create()
                    .ShowDialogAsync();
                return;
            }

            _profilesManager.Create(ProfileName, DownloadsLocation, SelectedGame, SelectedUser);

            _navigationService?.Journal.GoBack();

            EventAggregator.PublishOnUIThread(new ProfileCreatedEvent());
        }

        public override Task OnViewLoaded()
        {
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.Games, LocalIdentifiers.Views.GamesSelection);
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.Authentication, LocalIdentifiers.Views.Authentication);
            return Task.CompletedTask;

        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Users.Clear();
            Users.AddRange(_profilesManager.EnumerateUsers());
            _navigationService = navigationContext.NavigationService;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            KeepAlive = false;
        }

        public void Handle(UserSelectedEvent @event)
        {
            var u = @event.UserDetails;
            var existingUser = Users.FirstOrDefault(x => x.Id == u.Id);
            if (existingUser != null)
            {
                Users.Remove(existingUser);
            }
            Users.Add(u);
            SelectedUser = u;
            RaisePropertyChanged(nameof(IsUserSelected));
        }

        public void Handle(GameSelectedEvent @event)
        {
            SelectedGame = @event.GameDetails;
            RaisePropertyChanged(nameof(IsGameSelected));
        }
    }
}
