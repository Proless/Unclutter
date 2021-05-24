using Modules.Profiles.Validation;
using Prism.Commands;
using Prism.Regions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Modules.Profiles.Services.Authentication;
using Unclutter.Modules.Commands;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.Common;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Plugins.StartupAction;

namespace Modules.Profiles.ViewModels
{
    public class NewProfileViewModel : ValidationViewModelBase, IStartupViewAction
    {
        private readonly IAuthenticationService _authenticationService;

        /* Services */
        private readonly ILogger _logger;

        /* Fields */
        private IUserDetails _userDetails;
        private IGameDetails _gameDetails;
        private string _profileName;
        private string _downloadsLocation;
        private string _apiKey;

        /* Properties */
        [Required(AllowEmptyStrings = false, ErrorMessage = "A profile name is required !")]
        [FileName(ErrorMessage = "The profile name contains invalid characters !")]
        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The Downloads folder is required !")]
        public string DownloadsLocation
        {
            get => _downloadsLocation;
            set => SetProperty(ref _downloadsLocation, value);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The API Key is required !")]
        [OnlyAscii(ErrorMessage = "The API Key contains invalid characters !")]
        public string APIKey
        {
            get => _apiKey;
            set => SetProperty(ref _apiKey, value);
        }

        public bool IsAuthorized => _userDetails != null;
        public bool IsGameSelected => _gameDetails != null;

        /* Commands */
        public DelegateCommand<string> BrowseCommand => new DelegateCommand<string>(Browse);
        public DelegateCommand ChooseGameCommand => new DelegateCommand(ChooseGame);
        public DelegateCommand BrowseFolderCommand => new DelegateCommand(BrowseFolder);
        public DelegateCommand AuthorizeAppCommand => new AsyncDelegateCommand(AuthorizeApp);

        /* Constructor */
        public NewProfileViewModel(IAuthenticationService authenticationService, ILoggerProvider loggerProvider)
        {
            _authenticationService = authenticationService;
            _logger = loggerProvider.GetInstance();
        }

        /* Methods */
        public void Browse(string url)
        {
            var psi = new ProcessStartInfo { UseShellExecute = true, FileName = url };
            Process.Start(psi);
        }
        public void ChooseGame()
        {
            var parameters = new NavigationParameters();
            var hasErrors = TryGetErrors(nameof(APIKey), out _);
            if (!hasErrors && !string.IsNullOrWhiteSpace(APIKey))
            {
                parameters.Add(LocalIdentifiers.Parameters.APIKey, APIKey);
            }
            RegionManager.RequestNavigate(CommonIdentifiers.Regions.Startup, LocalIdentifiers.Views.GameSelection, parameters);
        }
        public void BrowseFolder()
        {
        }
        public async Task AuthorizeApp()
        {
        }

        /* Helpers */
        #region IStartupViewAction
        public string Label => "Create";
        public string Hint => "";
        public string IconReference => "";
        public ICommand ActionCommand => new AsyncDelegateCommand(Execute, CanExecute)
            .ObservesProperty(() => ProfileName)
            .ObservesProperty(() => DownloadsLocation)
            .ObservesProperty(() => APIKey)
            .ObservesProperty(() => IsAuthorized)
            .ObservesProperty(() => IsGameSelected);
        public Task Execute()
        {
            return Task.CompletedTask;
        }
        private bool CanExecute()
        {
            return !HasErrors
                   && !string.IsNullOrWhiteSpace(APIKey)
                   && !string.IsNullOrWhiteSpace(ProfileName)
                   && !string.IsNullOrWhiteSpace(DownloadsLocation)
                   && IsGameSelected;
        }
        #endregion
    }
}
