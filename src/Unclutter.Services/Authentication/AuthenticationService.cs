using System;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.NexusAPI;
using Unclutter.NexusAPI.Exceptions;
using Unclutter.NexusAPI.Inquirers;
using Unclutter.SDK.Common;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;
using Unclutter.Services.Profiles;

namespace Unclutter.Services.Authentication
{

    public class AuthenticationService : IAuthenticationService
    {
        public const string NexusModsSSOServer = "wss://sso.nexusmods.com";
        private readonly IJSONService _jSONService;
        private readonly INexusAPIClientFactory _clientFactory;
        private readonly ILogger _log;

        public AuthenticationService(IJSONService jSONService, ILoggerProvider loggerProvider, INexusAPIClientFactory clientFactory)
        {
            _jSONService = jSONService;
            _clientFactory = clientFactory;
            _log = loggerProvider.GetAppLogger();
        }
        public async Task<IUserDetails> RequestAPIKey(string applicationReference, CancellationToken cancellationToken)
        {
            // TODO: refactor this mess
            var uuid = Guid.NewGuid().ToString();

            var initialPayload = _jSONService.Serialize(new { id = uuid, protocol = 2, token = "" });

            var responseData = new { success = false, data = new { api_key = "", connection_token = "" }, error = "" };

            using var socket = new ClientWebSocket();

            // Configure the socket
            socket.Options.KeepAliveInterval = TimeSpan.FromSeconds(45);

            // Connecting to the server
            await socket.ConnectAsync(new Uri(NexusModsSSOServer), cancellationToken);

            // Sending first request to get a connection_token, the token with implementation is not needed.
            await socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(initialPayload)), WebSocketMessageType.Text, true, cancellationToken);

            // Receiving the connection_token.
            var responseBuffer = new ArraySegment<byte>(new byte[512]);
            await socket.ReceiveAsync(responseBuffer, cancellationToken);

            // Store the connection_token.
            responseData = _jSONService.Deserialize(Encoding.UTF8.GetString(responseBuffer), responseData);

            if (!responseData.success) throw new APIException(responseData.error, HttpStatusCode.ServiceUnavailable);

            // Open the browser to prompt the user to authorize the request via the nexus mods website (SSO Service).
            OpenBrowser(@"https://www.nexusmods.com/sso?id=" + uuid + "&application=" + applicationReference);

            // Receiving the API key.
            responseBuffer = new ArraySegment<byte>(new byte[512]);
            await socket.ReceiveAsync(responseBuffer, cancellationToken);

            // Store the API Key.
            responseData = _jSONService.Deserialize(Encoding.UTF8.GetString(responseBuffer), responseData);

            if (!responseData.success) throw new APIException(responseData.error, HttpStatusCode.ServiceUnavailable);

            // Checking to make sure the API Key was received, otherwise throw an exception.
            if (string.IsNullOrWhiteSpace(responseData.data.api_key))
            {
                throw new APIException(
                    $"Unable to receive the API Key,{Environment.NewLine}",
                    HttpStatusCode.ServiceUnavailable);
            }

            var userDetails = await ValidateAPIKey(responseData.data.api_key, cancellationToken);
            return userDetails;
        }
        public async Task<IUserDetails> ValidateAPIKey(string key, CancellationToken cancellationToken)
        {
            using var client = _clientFactory.CreateClient(key);
            var inquirer = new UserInquirer(client);
            var user = await inquirer.GetUserAsync(cancellationToken);
            return new UserDetails
            {
                Email = user.Email,
                Key = user.Key,
                Name = user.Name,
                IsPremium = user.IsPremium,
                IsSupporter = user.IsSupporter,
                ProfileUrl = user.ProfileAvatarUrl,
                Id = user.UserId
            };
        }

        /* Helpers */
        private void OpenBrowser(string url)
        {
            var processContext = new ProcessStartInfo { UseShellExecute = true, FileName = url };
            try
            {
                Process.Start(processContext);
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
            }
        }
    }
}
