using Prism.Commands;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;

namespace Unclutter.Modules.Commands
{
    public class AsyncDelegateCommand<T> : DelegateCommand<T>
    {
        /* Fields */
        protected Func<T, Task> ExecuteAsync;
        protected bool ContinueOnCapturedContext;
        protected bool Executing;

        /* Constructors */
        public AsyncDelegateCommand(Func<T, Task> executeAsync) :
            this(executeAsync, p => true, false)
        {

        }
        public AsyncDelegateCommand(Func<T, Task> executeAsync, Func<T, bool> canExecuteMethod) :
            this(executeAsync, canExecuteMethod, false)
        {

        }
        public AsyncDelegateCommand(Func<T, Task> executeAsync, Func<T, bool> canExecuteMethod, bool continueOnCapturedContext) :
            base(p => { }, canExecuteMethod)
        {
            ExecuteAsync = executeAsync;
            ContinueOnCapturedContext = continueOnCapturedContext;
            Executing = false;
        }

        /* Methods */
        public new AsyncDelegateCommand<T> ObservesProperty<TType>(Expression<Func<TType>> propertyExpression)
        {
            base.ObservesProperty(propertyExpression);
            return this;
        }
        public new AsyncDelegateCommand<T> ObservesCanExecute(Expression<Func<bool>> canExecuteExpression)
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
                await Application.Current.Dispatcher.InvokeAsync(() => ExecuteAsync((T)parameter)).Task.Unwrap();
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            finally
            {
                Executing = false;
            }
        }
        protected virtual void OnException(Exception ex)
        {
            throw ex;
        }
    }
}
