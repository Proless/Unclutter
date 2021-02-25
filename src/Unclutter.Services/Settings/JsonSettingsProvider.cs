using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Unclutter.Services.Settings
{
    public class JsonSettingsProvider : ISettingsProvider
    {
        /* Fields */
        private readonly JsonSerializer _jsonSerializer;
        private JsonSettingsSection _root;

        /* Properties */
        public JObject Document { get; }
        public IEnumerable<KeyValuePair<string, string>> Settings => GetSettings();

        /* Constructor */
        public JsonSettingsProvider(string json, JsonSerializer jsonSerializer)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentException(nameof(json));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));

            Document = JObject.Parse(json, new JsonLoadSettings
            {
                CommentHandling = CommentHandling.Ignore,
                LineInfoHandling = LineInfoHandling.Ignore,
                DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace
            });
        }

        /* Properties */
        public int Count => GetCount();

        /* Methods */
        public ISettingsSection GetRootSection()
        {
            if (!(_root is null)) return _root;

            _root = new JsonSettingsSection(this, _jsonSerializer);
            return _root;
        }


        /* Helpers */
        private IEnumerable<KeyValuePair<string, string>> GetSettings()
        {
            var list = new List<KeyValuePair<string, string>>();

            Traverse(t =>
            {
                list.Add(new KeyValuePair<string, string>(t.Path, t.ToString()));
            });

            return list;
        }
        private int GetCount()
        {
            var count = 0;
            Traverse(token =>
            {
                count++;
            });
            return count;
        }
        private void Traverse(Action<JToken> action)
        {
            var tokens = new Queue<JToken>();
            tokens.Enqueue(Document);

            while (tokens.Count > 0)
            {
                var currentToken = tokens.Dequeue();
                if (currentToken.HasValues)
                {
                    foreach (var childToken in currentToken)
                    {
                        tokens.Enqueue(childToken);
                    }
                }
                else
                {
                    action?.Invoke(currentToken);
                }
            }
        }
    }
}
