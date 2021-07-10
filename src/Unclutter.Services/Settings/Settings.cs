using System;
using System.IO;
using Unclutter.SDK.Services;
using Unclutter.SDK.Settings;

namespace Unclutter.Services.Settings
{
    internal class Settings<T> : ISettings<T> where T : class, new()
    {
        /* Fields */
        private readonly string _file;
        private readonly Action<T> _defaultsAction;
        private readonly IJsonService _jsonService;

        /* Properties */
        public T Instance { get; private set; }
        public string Name { get; }
        public string Location { get; }
        public Type SettingsType { get; }

        /* Constructor */
        public Settings(string file, Action<T> defaultsAction, IJsonService jsonService)
        {
            _file = file;
            _defaultsAction = defaultsAction;
            _jsonService = jsonService;

            Name = Path.GetFileNameWithoutExtension(file);
            Location = Path.GetFullPath(file);
            SettingsType = typeof(T);

            Initialize();
        }

        /* Methods */
        public ISettings<TSettings> GetSettings<TSettings>() where TSettings : class, new()
        {
            if (typeof(TSettings) != SettingsType)
            {
                throw new ArgumentException("Typed mismatch exception!", nameof(TSettings));
            }
            return this as ISettings<TSettings>;
        }

        public void Save()
        {
            _jsonService.SerializeToFile(Instance, _file);
        }

        public void Reload()
        {
            Instance = Load();
        }

        public void Reset()
        {
            Instance = Activator.CreateInstance<T>();
            _defaultsAction?.Invoke(Instance);
        }

        /* Helpers */
        private void Initialize()
        {
            Instance = File.Exists(_file) ? Load() : Create();
        }

        private T Load()
        {
            return _jsonService.DeserializeFromFile<T>(_file);
        }

        private T Create()
        {
            var instance = Activator.CreateInstance<T>();
            _defaultsAction?.Invoke(instance);
            _jsonService.SerializeToFile(instance, _file);

            return instance;
        }
    }
}
