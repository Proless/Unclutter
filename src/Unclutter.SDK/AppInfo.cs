using System.Reflection;

namespace Unclutter.SDK
{
    public static class AppInfo
    {
        /* Fields */
        private static readonly AssemblyName _assemblyName;

        /* Properties */
        public static string Name => Constants.Unclutter;
        public static string Version => _assemblyName.Version?.ToString(2);

        /* Constructor */
        static AppInfo()
        {
            _assemblyName = Assembly.GetEntryAssembly()?.GetName();
        }
    }
}
