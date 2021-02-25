using Prism.Commands;
using Unclutter.Modules.ViewModels;
using Unclutter.Theme;

namespace Unclutter.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        /* Fields */
        private readonly IThemeProvider _themeProvider;

        /* Properties */
        public DelegateCommand<bool?> SwitchThemeModeCommand => new(SwitchThemeMode);

        /* Constructor */
        public ShellViewModel(IThemeProvider themeProvider)
        {
            _themeProvider = themeProvider;
        }

        /* Helpers */
        private void SwitchThemeMode(bool? isDarkTheme)
        {
            switch (isDarkTheme)
            {
                case null:
                    return;
                case true:
                    _themeProvider.SetThemeMode(ThemeMode.Dark);
                    break;
                default:
                    _themeProvider.SetThemeMode(ThemeMode.Light);
                    break;
            }
        }
    }
}
