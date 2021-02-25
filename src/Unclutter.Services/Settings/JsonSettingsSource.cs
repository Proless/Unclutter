using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Unclutter.Services.Settings
{
    public class JsonSettingsSource : ISettingsSource
    {

        /* Fields */
        private readonly JsonSerializer _jsonSerializer;
        private readonly string _file;
        private JsonSettingsProvider _provider;

        /* Properties */
        public Uri Location { get; }

        /* Constructors */
        public JsonSettingsSource(string file, JsonSerializer jsonSerializer)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentNullException(nameof(file));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));

            if (!File.Exists(file))
                File.WriteAllText(file, "{}");

            _file = Path.GetFullPath(file);

            Location = new Uri(_file, UriKind.Absolute);
        }

        /* Methods */
        public ISettingsProvider Load()
        {
            if (!File.Exists(_file)) throw new InvalidOperationException($"The json file: {_file} doesn't exist, or it was deleted.");

            var json = File.ReadAllText(_file, Encoding.UTF8);

            _provider = new JsonSettingsProvider(json, _jsonSerializer);

            return _provider;
        }

        public void Persist()
        {
            File.WriteAllText(_file, _provider.Document.ToString(Formatting.Indented), Encoding.UTF8);
        }
    }
}
