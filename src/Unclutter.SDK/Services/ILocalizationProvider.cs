namespace Unclutter.SDK.Services
{
    public interface ILocalizationProvider
    {
        string DefaultDictionaryName { get; }
        string DefaultAssemblyName { get; }
        T GetLocalizedValue<T>(string key);
        T GetLocalizedValue<T>(string key, string assemblyName, string dictionaryName);
        string GetLocalizedString(string key);
        string GetLocalizedString(string key, params object[] @params);
        string GetLocalizedString(string key, string assemblyName, string dictionaryName);
        string GetLocalizedString(string key, string assemblyName, string dictionaryName, params object[] @params);
        void SetLanguage(Language language);
    }

    public enum Language
    {
        System,
        English,
        German,
        Arabic
    }
}
