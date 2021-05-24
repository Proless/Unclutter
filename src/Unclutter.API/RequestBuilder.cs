using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;

namespace Unclutter.API
{
    public class RequestBuilder
    {
        #region static
        public static RequestBuilder Get(string url)
        {
            return new RequestBuilder(url, HttpMethod.Get);
        }
        public static RequestBuilder Post(string url)
        {
            return new RequestBuilder(url, HttpMethod.Post);
        }
        public static RequestBuilder Delete(string url)
        {
            return new RequestBuilder(url, HttpMethod.Delete);
        }
        #endregion

        /* Fields */
        private readonly HttpRequestMessage _requestMessage;
        private readonly UriBuilder _uriBuilder;

        /* Properties */
        public Uri Uri => _uriBuilder.Uri;

        /* Constructor */
        private RequestBuilder(string url, HttpMethod method)
        {
            _uriBuilder = new UriBuilder($"{NexusClient.API}{url}");
            _requestMessage = new HttpRequestMessage
            {
                Method = method
            };
        }

        /* Methods */
        public HttpRequestMessage Create()
        {
            _requestMessage.RequestUri = _uriBuilder.Uri;
            return _requestMessage;
        }

        public RequestBuilder WithQueryParameter(string paramName, string paramValue)
        {
            var query = HttpUtility.ParseQueryString(_uriBuilder.Query);
            query[paramName] = paramValue;
            _uriBuilder.Query = query.ToString() ?? _uriBuilder.Query;
            return this;
        }

        public RequestBuilder WithQueryParameter(IDictionary<string, string> dictionary)
        {
            if (dictionary is null) return this;

            foreach (var (key, value) in dictionary)
            {
                WithQueryParameter(key, value);
            }
            return this;
        }

        public RequestBuilder WithContent(IDictionary<string, string> dictionary)
        {
            _requestMessage.Content = new FormUrlEncodedContent(dictionary);
            return this;
        }

        public RequestBuilder WithContent(HttpContent content)
        {
            _requestMessage.Content = content;
            return this;
        }

        public RequestBuilder WithAPIKey(string key)
        {
            _requestMessage.Headers.Remove(NexusClient.APIKeyHeaderName);
            _requestMessage.Headers.Add(NexusClient.APIKeyHeaderName, key);
            return this;
        }
    }
}
