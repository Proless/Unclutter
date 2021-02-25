using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Unclutter.NexusAPI.Exceptions;

namespace Unclutter.NexusAPI.Internals.Handlers
{
    internal class APILimitsHandler : MessageProcessingHandler
    {
        /* Fields */
        private readonly Action<HttpResponseMessage> _responseCallback;
        private readonly IRateManager _rateManager;

        /* Constructors */
        public APILimitsHandler(Action<HttpResponseMessage> responseCallback, IRateManager rateManager)
        {
            _responseCallback = responseCallback;
            _rateManager = rateManager;
        }
        public APILimitsHandler(HttpMessageHandler handler, Action<HttpResponseMessage> responseCallback, IRateManager rateManager) : base(handler)
        {
            _responseCallback = responseCallback;
            _rateManager = rateManager;
        }

        /* Methods */
        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_rateManager.CustomDailyLimitExceeded() && _rateManager.CustomHourlyLimitExceeded())
            {
                throw new LimitsExceededException((HttpStatusCode)429, LimitType.Custom);
            }

            return request;
        }
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            _responseCallback(response);
            return response;
        }
    }
}
