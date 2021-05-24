using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Unclutter.API
{
    public static class Extensions
    {
        public static async Task<T> DeserializeAsync<T>(this HttpContent content, JsonSerializerOptions options, CancellationToken cancellationToken)
        {
            var stream = await content.ReadAsStreamAsync(cancellationToken);
            var output = await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
            return output;
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list is null || items is null) return;

            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
