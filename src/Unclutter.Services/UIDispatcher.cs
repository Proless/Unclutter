using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Unclutter.Services
{
    public static class UIDispatcher
    {
        public static Dispatcher Dispatcher => Application.Current.Dispatcher;

        public static void VerifyAccess() => Dispatcher.VerifyAccess();

        public static bool CheckAccess() => Dispatcher.CheckAccess();

        /* Async */
        public static Task OnUIThreadAsync(Func<Task> action)
        {
            return Dispatcher.InvokeAsync(action).Task.Unwrap();
        }
        public static Task OnUIThreadAsync(Func<Task> action, DispatcherPriority priority)
        {
            return Dispatcher.InvokeAsync(action, priority).Task.Unwrap();
        }
        public static Task BeginOnUIThread(Action action, DispatcherPriority priority)
        {
            return Dispatcher.BeginInvoke(action, priority).Task;
        }
        public static Task OnUIThreadAsync(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return Dispatcher.InvokeAsync(action, priority).Task;
        }
        public static Task BeginOnUIThread(Action action)
        {
            return Dispatcher.BeginInvoke(action).Task;
        }

        /* Sync */
        public static void OnUIThread(Action action)
        {
            if (CheckAccess())
            {
                action();
            }
            else
            {
                Exception exception = null;

                void ActionMethod()
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }

                Dispatcher.Invoke(ActionMethod);
                if (exception != null)
                    throw new TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
            }
        }
    }
}
