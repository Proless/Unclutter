using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Unclutter.NexusAPI.DataModels;

namespace Unclutter.NexusAPI.Internals
{
    internal static class Extensions
    {

        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer { DefaultValueHandling = DefaultValueHandling.Ignore };

        internal static Uri AddQuery(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

        internal static async Task<T> DeserializeContent<T>(this HttpContent httpContent)
        {
            await using var content = await httpContent.ReadAsStreamAsync();
            using var reader = new StreamReader(content);
            using var jsonReader = new JsonTextReader(reader);

            return _jsonSerializer.Deserialize<T>(jsonReader);
        }

        internal static string GetTimePeriod(this NexusTimePeriod timePeriod)
        {
            return timePeriod switch
            {
                NexusTimePeriod.Day => "1d",
                NexusTimePeriod.Week => "1w",
                _ => "1m"
            };
        }
    }
}
