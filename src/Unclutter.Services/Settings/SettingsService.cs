using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Settings;
using Unclutter.Services.Profiles;

namespace Unclutter.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        /* Fields */
        private readonly IContainerExtension _containerExtension;
        private readonly IJsonService _jsonService;
        private readonly IProfilesManager _profilesManager;
        private readonly string _settingsDirectory;
        private readonly Dictionary<string, SettingsRegistryEntry> _settingsRegistry = new Dictionary<string, SettingsRegistryEntry>();
        private readonly Dictionary<string, ISettings> _settings = new Dictionary<string, ISettings>();

        /* Constructor */
        public SettingsService(IContainerExtension containerExtension, IJsonService jsonService, IDirectoryService directoryService, IProfilesManager profilesManager)
        {
            _containerExtension = containerExtension;
            _jsonService = jsonService;
            _profilesManager = profilesManager;
            _settingsDirectory = Path.Combine(directoryService.WorkingDirectory, "settings");
            _profilesManager.ProfileChanged += OnProfileChanged;
            directoryService.EnsureDirectoryAccess(_settingsDirectory);
        }

        /* Methods */
        public void RegisterSettings<T>(string name, Action<T> defaults) where T : class, new()
        {
            InternalRegisterSettings(name, defaults, false);
        }

        public void RegisterProfileSettings<T>(string name, Action<T> defaults) where T : class, new()
        {
            InternalRegisterSettings(name, defaults, true);
        }

        public ISettings<T> GetSettings<T>(string name) where T : class, new()
        {
            return _settingsRegistry.TryGetValue(name, out var entry) ? ResolveSettingsInstance<T>(entry) : null;
        }

        /* Helpers */
        private void InternalRegisterSettings<T>(string name, Action<T> defaults, bool isProfileSpecific) where T : class, new()
        {
            ValidateName(name);

            // Create a new entry.
            var entry = new SettingsRegistryEntry(name, typeof(T), isProfileSpecific, defaults);
            _settingsRegistry[name] = entry;

            // Register the settings type as a singleton in the IoC-Container
            _containerExtension.RegisterSingleton(typeof(ISettings<T>), () => ResolveSettingsInstance<T>(_settingsRegistry[name]));
        }

        private ISettings<T> ResolveSettingsInstance<T>(SettingsRegistryEntry entry) where T : class, new()
        {
            if (entry == null) return null;

            if (_settings.TryGetValue(entry.Name, out var instance))
            {
                return instance.Get<T>();
            }

            return CreateSettingsInstance<T>(entry);
        }

        private ISettings<T> CreateSettingsInstance<T>(SettingsRegistryEntry entry) where T : class, new()
        {
            if (entry.IsProfileSpecific && _profilesManager.CurrentProfile == null) return null;

            var file = GetSettingsFile(entry);
            var settings = new Settings<T>(file, entry.DefaultsAction as Action<T>, _jsonService);
            _settings[entry.Name] = settings;

            return settings;
        }

        private void OnProfileChanged(ProfileChangedArgs args)
        {
            foreach (var entry in _settingsRegistry.Values.Where(e => e.IsProfileSpecific))
            {
                _settings.Remove(entry.Name);
            }
        }

        private string GetSettingsFile(SettingsRegistryEntry entry)
        {
            if (entry.IsProfileSpecific)
            {
                return Path.Combine(_profilesManager.ProfilesDirectory, _profilesManager.CurrentProfile.Name, $"{entry.Name}.json");
            }
            return Path.Combine(_settingsDirectory, $"{entry.Name}.json");
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                throw new ArgumentException("The passed name is null or is not a valid file name !", nameof(name));
        }
    }
}
