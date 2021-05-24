using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Unclutter.Modules.Commands;
using Unclutter.Modules.ViewModels;

namespace Unclutter.Modules.Plugins
{
    [ExportShellCommandAction]
    public abstract class ShellCommandActionBase : ViewModelBase, IShellCommandAction
    {
        /* Fields */
        private double _priority;
        private string _hint;
        private ICommand _action;
        private Control _control;

        /* Properties */
        public double Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }
        public virtual string Hint
        {
            get => _hint;
            set => SetProperty(ref _hint, value);
        }
        public virtual ICommand Action
        {
            get => _action;
            set => SetProperty(ref _action, value);
        }
        public Control Control
        {
            get => _control;
            set
            {
                if (value != null) value.DataContext = this;
                SetProperty(ref _control, value);
            }
        }

        /* Properties */
        protected ShellCommandActionBase()
        {
            _action = new AsyncDelegateCommand(ExecuteAction);
        }

        /* Methods */
        protected abstract Task ExecuteAction();
        public abstract void Initialize();
    }
}
