namespace Unclutter.SDK.App
{
    public interface IAppWindowServices
    {
        bool IsFlyoutOpen(string name);
        void OpenFlyout(string name);
        void CloseFlyout(string name);

        void NavigateToAppView(AppView view);
    }
}
