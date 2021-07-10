namespace Unclutter.SDK.App
{
    public interface ISettingsCenter
    {
        void Open();
        void Close();
        void NavigateToSettingsView(SettingsView view);
    }
}
