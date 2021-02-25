using System.Net;
using System.Net.Http;
using System.Threading;
using Unclutter.NexusAPI.DataModels;
using Unclutter.NexusAPI.Exceptions;

namespace Unclutter.NexusAPI.Internals.Handlers
{
    internal class NexusErrorsHandler : MessageProcessingHandler
    {
        /* Constructors */
        public NexusErrorsHandler() { }
        public NexusErrorsHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        /* Methods */
        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return request;
        }
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var responseMessage = response.Content.DeserializeContent<NexusMessage>().Result;
            throw response.StatusCode switch
            {
                HttpStatusCode.Forbidden => new ForbiddenException(responseMessage.Message, response.StatusCode),
                HttpStatusCode.Unauthorized => new UnauthorizedException(responseMessage.Message, response.StatusCode),
                (HttpStatusCode)429 => new LimitsExceededException(responseMessage.Message, response.StatusCode, LimitType.API),
                _ => new APIException(responseMessage.Message, response.StatusCode)
            };
        }
    }
}
