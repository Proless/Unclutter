using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Unclutter.SDK.Common;

namespace Unclutter.Services.Loader
{
    public class LoaderScreenManager
    {
        public static LoaderScreenManager Create<T>(Action<ILoaderScreen> setDefaultsAction = null) where T : Window, ILoaderScreen, new()
        {
            return new LoaderScreenManager(() => new T(), setDefaultsAction);
        }

        /* Fields */
        private readonly Func<ILoaderScreen> _screenFactoryMethod;
        private readonly Action<ILoaderScreen> _setDefault;
        private Thread _loaderThread;
        private ManualResetEvent _initializedEvent;
        private ILoaderScreen _loaderScreen;
        private bool _isClosed;

        /* Constructor */
        private LoaderScreenManager(Func<ILoaderScreen> screenFactoryMethod, Action<ILoaderScreen> setDefault)
        {
            _screenFactoryMethod = screenFactoryMethod ?? throw new ArgumentNullException(nameof(screenFactoryMethod));
            _setDefault = setDefault;
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
                _loaderScreen.Show();
            }
        }

        public void Hide()
        {
            if (_isClosed) return;

            _loaderScreen.Hide();
        }

        public void Close()
        {
            _initializedEvent.Dispose();
            _loaderScreen.Close();
        }

        public void ReportProgress(string message)
        {
            ReportProgress(new ProgressReport(message, 0d));
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

                _setDefault?.Invoke(_loaderScreen);

                _loaderScreen.Closed += (_, _) =>
                {
                    _isClosed = true;
                    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                };

                _loaderScreen.Show();

                _initializedEvent.Set();

                Dispatcher.Run();
            });

            _loaderThread.SetApartmentState(ApartmentState.STA);
            _loaderThread.IsBackground = true;
            _loaderThread.Name = "LoaderScreenThread";
        }
    }
}
