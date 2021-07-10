using Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Input;
using Unclutter.Modules.Commands;
using Unclutter.SDK.Services;

namespace Unclutter.Modules.Plugins.AppWindowCommand
{
    public abstract class BaseAppWindowCommand : BindableBase, IAppWindowCommand
    {
        /* Fields */
        private bool _isVisible;
        private string _hint;
        private ICommand _command;
        private object _control;

        /* Properties */
        public abstract double? Order { get; }
        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }
        public virtual string Hint
        {
            get => _hint;
            set => SetProperty(ref _hint, value);
        }
        public virtual ICommand Command
        {
            get => _command;
            set => SetProperty(ref _command, value);
        }
        public object Content
        {
            get => _control;
            set => SetProperty(ref _control, value);
        }
        [Import]
        public ILocalizationProvider LocalizationProvider { get; protected set; }

        /* Constructor */
        protected BaseAppWindowCommand()
        {
            _command = new AsyncDelegateCommand(OnClicked);
            IsVisible = true;
        }

        /* Methods */
        public abstract void Initialize();
        protected abstract Task OnClicked();
    }
}
