using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Unclutter.Services.Settings
{
    public class JsonSettingsSection : ISettingsSection
    {
        /* Fields */
        private readonly Dictionary<string, ISettingsSection> _settingsSections;
        private readonly JsonSerializer _jsonSerializer;

        /* Properties */
        public JObject Node { get; }
        public IEnumerable<ISettingsSection> Sections => _settingsSections.Values;
        public string Key => PathHelpers.GetKey(Path);
        public string Path => Node.Path;
        public string Value => Node.ToString();
        public int Count => Node.Children<JProperty>().Count(p => p.Value.Type != JTokenType.Object);

        /* Indexer */
        public object this[string key]
        {
            get => InternalGet(key);
            set => InternalSet(key, value);
        }

        /* Constructors */
        public JsonSettingsSection(JsonSettingsProvider settingsProvider, JsonSerializer jsonSerializer)
            : this(settingsProvider.Document, jsonSerializer) { }
        private JsonSettingsSection(JObject token, JsonSerializer jsonSerializer)
        {
            _settingsSections = new Dictionary<string, ISettingsSection>();
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));

            Node = token ?? throw new ArgumentNullException(nameof(token));

            PopulateSections();
        }

        /* Methods */
        public ISettingsSection GetSection(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            return _settingsSections.TryGetValue(key, out var existingSection) ? existingSection : InternalGetSection(key);
        }
        public T GetSection<T>(string key, T defaultValue = default) where T : class, new()
        {
            if (!(GetSection(key) is JsonSettingsSection section))
                return defaultValue;

            if (section.Node.HasValues) return section.Get<T>();

            // At this point we know that this is an new empty section
            // we then serialize an object of the type argument
            // to json and store that json as the new section structure.
            _settingsSections.Remove(key);
            Node.Remove(key);

            var instance = defaultValue ?? Activator.CreateInstance<T>();
            var newSectionObject = JObject.FromObject(instance, _jsonSerializer);
            Node[key] = newSectionObject;
            _settingsSections[key] = new JsonSettingsSection(newSectionObject, _jsonSerializer);
            return instance;
        }
        public T Get<T>() where T : class, new()
        {
            return Node.ToObject<T>(_jsonSerializer);
        }
        public T Get<T>(string key, T defaultValue = default)
        {
            if (!(InternalGet(key) is string)) return defaultValue;

            var property = Node.Children<JProperty>().FirstOrDefault(p => p.Name == key);

            return property != null ? property.Value.ToObject<T>(_jsonSerializer) : defaultValue;
        }

        /* Helpers */
        private void PopulateSections()
        {
            _settingsSections.Clear();

            foreach (var property in Node.Children<JProperty>().Where(p => p.Value.Type == JTokenType.Object))
            {
                var section = (JObject)property.Value;
                _settingsSections[property.Name] = new JsonSettingsSection(section, _jsonSerializer);
            }
        }
        private JsonSettingsSection InternalGetSection(string key)
        {
            if (Node.Children<JProperty>().Any(p => p.Name == key && p.Value.Type != JTokenType.Object))
            {
                // We return null, because the key doesn't correspond to a sub-section but rather a setting value
                return null;
            }

            var existingSection = Node.Children<JProperty>()
                .FirstOrDefault(p => p.Value.Type == JTokenType.Object && p.Name == key);

            JsonSettingsSection newSection;
            if (existingSection is null)
            {
                var newSectionObject = JObject.Parse("{}");
                Node[key] = newSectionObject;
                newSection = new JsonSettingsSection(newSectionObject, _jsonSerializer);
            }
            else
            {
                newSection = new JsonSettingsSection((JObject)existingSection.Value, _jsonSerializer);
            }

            _settingsSections[key] = newSection;
            return newSection;
        }

        private void InternalSet(string key, object value)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            // remove the setting if the value passed in is null,
            // regardless whether it is a section or not.
            if (value is null)
            {
                Node.Remove(key);
                _settingsSections.Remove(key);
                return;
            }

            // we remove the property regardless because we need to replace it
            // either with a section or a single setting value (primitive type)
            Node.Children<JProperty>()
               .FirstOrDefault(p => p.Name == key)?.Remove();

            // parse the value that we want to set to a JToken to determine it's type
            var valueToken = JToken.FromObject(value, _jsonSerializer);


            // differentiate between JObject and other JValues
            if (valueToken.Type == JTokenType.Object)
            {
                var newSection = new JsonSettingsSection((JObject)valueToken, _jsonSerializer);
                _settingsSections[key] = newSection;
            }
            else
            {
                // make sure to remove the value if it was a section,
                // because we replaced it with this JValue
                _settingsSections.Remove(key);
            }

            // finally we only need to set the value.
            Node[key] = valueToken;
        }
        private object InternalGet(string key)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            return Node.Children<JProperty>()
                .Where(p => p.Name == key)
                .Select(p => p.Value.ToString())
                .FirstOrDefault();
        }
    }
}
