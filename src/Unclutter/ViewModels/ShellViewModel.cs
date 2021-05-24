using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unclutter.Modules.Helpers;
using Unclutter.Modules.Plugins;
using Unclutter.Modules.ViewModels;
using Unclutter.SDK;
using Unclutter.SDK.Common;
using Unclutter.SDK.Events;
using Unclutter.SDK.IModels;
using Unclutter.SDK.Loader;
using Unclutter.SDK.Plugins;
using Unclutter.Services.Localization;

namespace Unclutter.ViewModels
{
    public class ShellViewModel : ViewModelBase, ILoader, IPluginConsumer, IHandler<ProfileChangedEvent>
    {
        /* Fields */
        private SynchronizedObservableCollection<IShellCommandAction> _shellCommandActions;
        private IUserProfile _selectedProfile;

        /* Properties */
        [ImportMany]
        public SynchronizedObservableCollection<IShellCommandAction> ShellCommandActions
        {
            get => _shellCommandActions;
            set => SetProperty(ref _shellCommandActions, value);
        }
        public IUserProfile SelectedProfile
        {
            get => _selectedProfile;
            set => SetProperty(ref _selectedProfile, value);
        }
        public ImportOptions Options => new ImportOptions() { ImportThread = ThreadOption.UIThread };
        public bool AutoSubscribe => true;

        /* Constructor */
        public ShellViewModel()
        {
        }

        /* Methods */
        public void OnImportsSatisfied()
        {
            var shellCommands = _shellCommandActions.ToArray();
            foreach (var shellCommand in shellCommands)
            {
                shellCommand.Initialize();
            }
            ShellCommandActions = new SynchronizedObservableCollection<IShellCommandAction>(shellCommands.OrderBy(a => a.Priority));
        }
        public Task HandleAsync(ProfileChangedEvent @event, CancellationToken cancellationToken)
        {
            if (@event.NewProfile != SelectedProfile)
            {
                SelectedProfile = @event.NewProfile;
                Title = $"{Constants.Unclutter} - {SelectedProfile.Name}";
            }
            return Task.CompletedTask;
        }

        #region ILoader
        public event Action<ProgressReport> ProgressChanged;
        public LoadOptions LoaderOptions => new LoadOptions();
        public async Task Load()
        {
            for (var i = 0; i < 100; i++)
            {
                await Task.Delay(1);
                ProgressChanged?.Invoke(new ProgressReport(string.Format(LocalizationProvider.Instance.GetLocalizedString(ResourceKeys.Loading_Status), SelectedProfile?.Name), i));
            }
        }
        #endregion
    }
}
