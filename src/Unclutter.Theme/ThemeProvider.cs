using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Windows.Media;

namespace Unclutter.Theme
{
    public class ThemeProvider : IThemeProvider
    {
        public ApplicationTheme Current { get; }

        public ThemeProvider()
        {
            Current = new ApplicationTheme
            {
                Mode = ThemeMode.Dark,
                Primary = Colors.Blue,
                Secondary = Colors.Cyan
            };
        }

        public void SetThemeMode(ThemeMode mode)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(mode switch
            {
                ThemeMode.Light => MaterialDesignThemes.Wpf.Theme.Light,
                ThemeMode.Dark => MaterialDesignThemes.Wpf.Theme.Dark,
                ThemeMode.Sync => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            });
            paletteHelper.SetTheme(theme);
            Current.Mode = mode;
        }

        /* Helpers */
        private ThemeMode GetAppMode()
        {
            var defaultValue = ((int)Current.Mode).ToString();
            try
            {
                var value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1")?.ToString();
                return (ThemeMode)int.Parse(value ?? defaultValue);

            }
            catch (Exception)
            {
                return ThemeMode.Light;
            }
        }
    }
}
