using Prism.Commands;
using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.API;
using Unclutter.Modules.Commands;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Models;
using Unclutter.SDK.Services;
using Unclutter.SDK.Validation;
using Unclutter.Services.Authentication;
using Unclutter.Services.Images;

namespace Unclutter.ViewModels.ProfilesManagement
{
    public class AuthenticationViewModel : BaseValidationViewModel
    {
        public const string ApplicationReference = "vortex";

        /* Services */
        private readonly ILogger _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IImageProvider _imageProvider;
        private readonly IClientServices _clientServices;

        /* Fields */
        private string _apiKey;
        private string _waitingMessage;
        private bool _isWaiting;
        private CancellationTokenSource _cts;

        /* Properties */
        [OnlyAscii(ErrorMessageKey = ResourceKeys.API_Key_Msg_Invalid_Error)]
        public string APIKey
        {
            get => _apiKey;
            set
            {
                SetProperty(ref _apiKey, value);
                if (!HasErrors)
                {
                    ValidateKey().ConfigureAwait(false);
                }
            }
        }
        public string WaitingMessage
        {
            get => _waitingMessage;
            set => SetProperty(ref _waitingMessage, value);
        }
        public bool IsWaiting
        {
            get => _isWaiting;
            set => SetProperty(ref _isWaiting, value);
        }

        /* Commands */
        public DelegateCommand AuthorizeCommand => new AsyncDelegateCommand(Authorize);
        public DelegateCommand CancelCommand => new DelegateCommand(Cancel);
        public DelegateCommand<string> BrowseUrlCommand => new DelegateCommand<string>(BrowseUrl);

        /* Constructor */
        public AuthenticationViewModel(IAuthenticationService authenticationService,
            ILogger logger,
            IImageProvider imageProvider,
            IClientServices clientServices)
        {
            _authenticationService = authenticationService;
            _imageProvider = imageProvider;
            _clientServices = clientServices;
            _logger = logger;

            IsWaiting = false;
            WaitingMessage = "";
        }

        /* Methods */
        public void BrowseUrl(string url)
        {
            _clientServices.OpenInDefaultApp(url);
        }

        public void Cancel()
        {
            _cts.Cancel();
        }

        public async Task Authorize()
        {
            await Validate(ct => _authenticationService.RequestAPIKey(ApplicationReference, ct)
                , LocalizationProvider.GetLocalizedString(ResourceKeys.Connecting));
        }

        public async Task ValidateKey()
        {
            if (!string.IsNullOrWhiteSpace(APIKey))
            {
                await Validate(ct => _authenticationService.ValidateAPIKey(APIKey, ct)
                    , LocalizationProvider.GetLocalizedString(ResourceKeys.Connecting));
            }
        }

        public async Task Validate(Func<CancellationToken, Task<IUserDetails>> validateFunc, string waitMessage)
        {
            if (IsWaiting)
            {
                Cancel();
            }

            _cts = new CancellationTokenSource();
            IsWaiting = true;
            WaitingMessage = waitMessage;
            try
            {
                var user = await validateFunc(_cts.Token);
                await _imageProvider.DownloadImageFor(user);
                EventAggregator.PublishOnBackgroundThread(new UserSelectedEvent(user));
                WaitingMessage = LocalizationProvider.GetLocalizedString(ResourceKeys.API_Key_Msg_Validating_Success, new object[] { user.Name, user.Email, user.IsPremium ? LocalizationProvider.GetLocalizedString(ResourceKeys.Yes) : LocalizationProvider.GetLocalizedString(ResourceKeys.No) });
            }
            catch (APIException ex)
            {
                WaitingMessage = LocalizationProvider.GetLocalizedString(ResourceKeys.API_Key_Msg_Validating_Error, ex.Message);
            }
            catch (TaskCanceledException)
            {
                IsWaiting = false;
                WaitingMessage = "";
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is WebSocketException)
            {
                WaitingMessage = LocalizationProvider.GetLocalizedString(ResourceKeys.Server_Connection_Error);
                _logger.Error(ex, ex.Message);

            }
            catch (Exception ex)
            {
                WaitingMessage = LocalizationProvider.GetLocalizedString(ResourceKeys.Server_Connection_Error_Internal);
                _logger.Error(ex, ex.Message);
            }
            finally
            {
                APIKey = "";
                await Task.Delay(4000);
                WaitingMessage = "";
                IsWaiting = false;
            }
        }
    }
}
