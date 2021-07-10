using Prism.Commands;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace Unclutter.Modules.Commands
{
    public class AsyncDelegateCommand : DelegateCommand
    {
        /* Fields */
        protected Func<Task> ExecuteAsync;
        protected bool ContinueOnCapturedContext;
        protected bool Executing;

        /* Constructors */
        public AsyncDelegateCommand(Func<Task> executeAsync) :
            this(executeAsync, () => true, false)
        {

        }
        public AsyncDelegateCommand(Func<Task> executeAsync, Func<bool> canExecuteMethod) :
            this(executeAsync, canExecuteMethod, false)
        {

        }
        public AsyncDelegateCommand(Func<Task> executeAsync, Func<bool> canExecuteMethod, bool continueOnCapturedContext) : base(() => { }, canExecuteMethod)
        {
            ExecuteAsync = executeAsync;
            ContinueOnCapturedContext = continueOnCapturedContext;
            Executing = false;
        }

        /* Methods */
        public new AsyncDelegateCommand ObservesProperty<T>(Expression<Func<T>> propertyExpression)
        {
            base.ObservesProperty(propertyExpression);
            return this;
        }
        public new AsyncDelegateCommand ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
        {
            base.ObservesCanExecute(canExecuteExpression);
            return this;
        }
        protected override async void Execute(object parameter)
        {
            if (Executing) return;

            try
            {
                Executing = true;
                await Application.Current.Dispatcher.InvokeAsync(() => ExecuteAsync()).Task.Unwrap();
            }
            catch (Exception ex)
            {
                ThrowOnException(ex);
            }
            finally
            {
                Executing = false;
            }
        }
        protected virtual void ThrowOnException(Exception ex)
        {
            throw ex;
        }
    }
}
