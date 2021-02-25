using Prism.Services.Dialogs;
using System;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK.IServices;

namespace Unclutter.ViewModels.Startup
{
    public class StartupViewModel : ViewModelBase, IDialogAware
    {
        /* Event */
        public event Action<IDialogResult> RequestClose;

        /* Constructor */
        public StartupViewModel(IProfileProvider profileProvider)
        {
            Title = "Unclutter - Startup";
            profileProvider.Changed += _ =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            };
        }

        /* Methods */
        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            RegionManager.Regions.Remove(LocalIdentifiers.Regions.Startup);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            RegionManager.RequestNavigate(LocalIdentifiers.Regions.Startup, LocalIdentifiers.Views.Startup);
        }
    }
}
