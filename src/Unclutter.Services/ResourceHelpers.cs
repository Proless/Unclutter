using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Unclutter.Services
{
    public static class ResourceHelpers
    {
        public static Assembly DefaultResourceAssembly => Assembly.GetExecutingAssembly();

        public static string GetResourceName(string fileName, Assembly assembly = null)
        {
            assembly ??= DefaultResourceAssembly;

            return assembly
                .GetManifestResourceNames()
                .Single(n => n.EndsWith(fileName));
        }

        public static BitmapImage GetImage(string resourceName, Assembly assembly = null)
        {
            assembly ??= DefaultResourceAssembly;

            var imgStream = assembly.GetManifestResourceStream(resourceName);

            var imgSource = new BitmapImage();
            imgSource.BeginInit();
            imgSource.StreamSource = imgStream;
            imgSource.CacheOption = BitmapCacheOption.OnLoad;
            imgSource.EndInit();
            imgSource.Freeze();

            return imgSource;
        }
    }
}
