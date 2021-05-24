using Prism.Ioc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Unclutter.SDK.Common;
using Unclutter.SDK.IServices;
using Unclutter.SDK.Loader;

namespace Unclutter.Services.Loader
{
    public class LoaderService : ILoaderService
    {
        /* Fields */
        private readonly IContainerExtension _containerExtension;
        private readonly ILogger _logger;
        private readonly Dictionary<ILoader, bool> _loaderFlags = new Dictionary<ILoader, bool>(); // memory leak
        private readonly ConcurrentQueue<ILoader> _loadersQueue = new ConcurrentQueue<ILoader>();

        /* Events */
        public event Action<ProgressReport> ProgressChanged;
        public event Action Finished;

        /* Properties */
        public bool IsLoading { get; private set; }

        /* Constructor */
        public LoaderService(IContainerExtension containerExtension, ILoggerProvider loggerProvider)
        {
            _containerExtension = containerExtension;
            _logger = loggerProvider.GetInstance();
        }

        /* Methods */
        public async Task Load()
        {
            if (IsLoading) return;

            IsLoading = true;

            while (!_loadersQueue.IsEmpty)
            {
                if (_loadersQueue.TryDequeue(out var loader))
                {
                    await InternalLoad(loader);
                    loader.ProgressChanged -= OnLoaderProgressChanged;
                }
            }

            _loaderFlags.Clear();

            IsLoading = false;

            Finished?.Invoke();
        }

        public async Task Load(ILoader loader)
        {
            loader.ProgressChanged += OnLoaderProgressChanged;
            await InternalLoad(loader);
            loader.ProgressChanged -= OnLoaderProgressChanged;
        }

        public void RegisterLoader(ILoader loader)
        {
            if (loader == null || _loaderFlags.ContainsKey(loader)) return;

            loader.ProgressChanged += OnLoaderProgressChanged;
            _loadersQueue.Enqueue(loader);
            _loaderFlags.Add(loader, false);
        }

        public void RegisterLoader<T>() where T : ILoader
        {
            try
            {
                var instance = _containerExtension.Resolve<T>();
                RegisterLoader(instance);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unable to register Loader of Type {LoaderType}.", typeof(T).FullName);
            }
        }

        /* Helpers */
        private async Task InternalLoad(ILoader loader)
        {
            // if Load method was already called on this ILoader instance, just return.
            if (_loaderFlags.TryGetValue(loader, out var loaded) && loaded) return;

            try
            {
                var options = loader.LoaderOptions ?? new LoadOptions();
                if (options.AutoLoad)
                {
                    if (options.LoadThread == ThreadOption.BackgroundThread)
                    {
                        await Task.Run(loader.Load);
                    }
                    else
                    {
                        await Application.Current.Dispatcher.InvokeAsync(loader.Load);
                    }
                }

                _loaderFlags[loader] = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while loading {Loader}...", loader.GetType().FullName);
            }
        }

        private void OnLoaderProgressChanged(ProgressReport report)
        {
            ProgressChanged?.Invoke(report);
        }
    }
}
