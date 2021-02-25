using System;
using Unclutter.NexusAPI.Inquirers;

namespace Unclutter.NexusAPI.Internals
{
    /// <summary>
    /// A base class for all Inquirer, which is used internally
    /// </summary>
    public class InquirerBase : IInquirerRateManager
    {
        /* Fields */
        protected readonly INexusAPIClient Client;

        /* Properties */
        public IRateManager RateManager => Client.RateManager;

        /* Constructors */
        protected internal InquirerBase(INexusAPIClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /* Methods */
        protected Uri ConstructRequestUri(string route, params string[] routeParams)
        {
            var formattedRoute = string.Format(route, routeParams);
            var output = new Uri(new Uri(Routes.BaseApiUrl), formattedRoute);
            return output;
        }
    }
}
