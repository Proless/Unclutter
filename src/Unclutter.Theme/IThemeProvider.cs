namespace Unclutter.Theme
{
    public interface IThemeProvider
    {
        ApplicationTheme Current { get; }
        void SetThemeMode(ThemeMode mode);
    }
}
