using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Unclutter.SDK.Progress;

namespace Unclutter.Services.Loader
{
    public class LoaderScreenManager
    {
        public static LoaderScreenManager Create<T>(Action<ILoaderScreen> configure = null) where T : Window, ILoaderScreen, new()
        {
            return new LoaderScreenManager(() => new T(), configure);
        }

        /* Fields */
        private bool _isClosed;
        private readonly Func<ILoaderScreen> _screenFactoryMethod;
        private readonly Action<ILoaderScreen> _configure;
        private Thread _loaderThread;
        private ManualResetEvent _initializedEvent;
        private ILoaderScreen _loaderScreen;

        /* Properties */
        public Window ScreenWindow => _loaderScreen as Window;

        /* Constructor */
        private LoaderScreenManager(Func<ILoaderScreen> screenFactoryMethod, Action<ILoaderScreen> configure)
        {
            _screenFactoryMethod = screenFactoryMethod ?? throw new ArgumentNullException(nameof(screenFactoryMethod));
            _configure = configure;
            SetupBackgroundThread();
        }

        /* Methods */
        public void Show()
        {
            if (_isClosed) return;

            if (_loaderThread.ThreadState != ThreadState.Background)
            {
                _loaderThread.Start();
                _initializedEvent.WaitOne();
            }
            else
            {
                _loaderScreen?.Show();
            }
        }

        public void Hide()
        {
            if (_isClosed) return;

            _loaderScreen?.Hide();
        }

        public void Close()
        {
            _initializedEvent.Dispose();
            _loaderScreen?.Close();
            _loaderScreen = null;
        }

        public void ReportProgress(string message)
        {
            ReportProgress(new ProgressReport(message));
        }

        public void ReportProgress(ProgressReport report)
        {
            _loaderScreen?.ReportProgress(report);

        }

        /* Helpers */
        private void SetupBackgroundThread()
        {
            _initializedEvent = new ManualResetEvent(false);

            _loaderThread = new Thread(() =>
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));

                _loaderScreen = _screenFactoryMethod();

                _configure?.Invoke(_loaderScreen);

                _loaderScreen.Closed += (_, _) =>
                {
                    _isClosed = true;
                    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                };

                _loaderScreen?.Show();

                _initializedEvent.Set();

                Dispatcher.Run();
            });

            _loaderThread.SetApartmentState(ApartmentState.STA);
            _loaderThread.IsBackground = true;
            _loaderThread.Name = "LoaderScreenThread";
        }
    }
}
