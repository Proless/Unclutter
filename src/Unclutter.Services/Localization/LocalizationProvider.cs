using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unclutter.SDK;
using Unclutter.SDK.Services;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;
using WPFLocalizeExtension.Providers;
using ILocalizationProvider = Unclutter.SDK.Services.ILocalizationProvider;

namespace Unclutter.Services.Localization
{
    public class LocalizationProvider : ILocalizationProvider
    {
        /* Static */
        public static LocalizationProvider Instance { get; } = new LocalizationProvider();

        /* Fields */
        private bool _isInitialized;

        /* Properties */
        public string DefaultDictionaryName => ResourceKeys.DefaultDictionaryName;
        public string DefaultAssemblyName => ResourceKeys.DefaultAssemblyName;
        public LocalizeDictionary LocalizeDictionary => LocalizeDictionary.Instance;
        public ResxLocalizationProvider ResxLocalizationProvider => ResxLocalizationProvider.Instance;

        /* Methods */
        public T GetLocalizedValue<T>(string key)
        {
            return LocExtension.GetLocalizedValue<T>($"{DefaultAssemblyName}:{DefaultDictionaryName}:{key}");
        }
        public T GetLocalizedValue<T>(string key, string assemblyName, string dictionaryName)
        {
            return LocExtension.GetLocalizedValue<T>($"{assemblyName}:{dictionaryName}:{key}");
        }
        public string GetLocalizedString(string key)
        {
            return GetLocalizedValue<string>(key);
        }
        public string GetLocalizedString(string key, params object[] @params)
        {
            return string.Format(GetLocalizedValue<string>(key), @params);
        }
        public string GetLocalizedString(string key, string assemblyName, string dictionaryName)
        {
            return GetLocalizedValue<string>(key, assemblyName, dictionaryName);
        }
        public string GetLocalizedString(string key, string assemblyName, string dictionaryName, params object[] @params)
        {
            return string.Format(GetLocalizedValue<string>(key, assemblyName, dictionaryName), @params);
        }
        public void SetLanguage(Language language)
        {
            LocalizeDictionary.Instance.Culture = language switch
            {
                Language.German => CultureInfo.GetCultureInfo("de"),
                Language.Arabic => CultureInfo.GetCultureInfo("ar"),
                Language.System => GetWindowsUICulture(),
                Language.English => CultureInfo.InvariantCulture,
                _ => CultureInfo.InvariantCulture
            };
        }
        public void Configure()
        {
            if (_isInitialized) return;

            LocalizeDictionary.SetCurrentThreadCulture = true;

            ResxLocalizationProvider.SearchCultures = new List<CultureInfo>
            {
                CultureInfo.GetCultureInfo("de"),
                CultureInfo.GetCultureInfo("ar"),
                CultureInfo.GetCultureInfo("en")
            };
            ResxLocalizationProvider.FallbackAssembly = DefaultAssemblyName;
            ResxLocalizationProvider.FallbackDictionary = DefaultDictionaryName;
            ResxLocalizationProvider.IgnoreCase = true;
            ResxLocalizationProvider.UpdateCultureList(DefaultAssemblyName, DefaultDictionaryName);
            _isInitialized = true;
        }

        /* Helpers */
        private CultureInfo GetWindowsUICulture()
        {
            var installedCulture = CultureInfo.InstalledUICulture;
            return ResxLocalizationProvider.AvailableCultures.Any(c => c.Equals(installedCulture))
                ? installedCulture
                : CultureInfo.InvariantCulture;
        }
    }
}
