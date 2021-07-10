using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Services;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Progress;

namespace Unclutter.Services.Loader
{
    public class LoaderService : ILoaderService
    {
        /* Fields */
        private readonly ILogger _logger;
        private readonly IPluginProvider _pluginProvider;
        private List<ILoader> _loaders = new List<ILoader>();
        private readonly ConcurrentQueue<ILoader> _registeredLoaders = new ConcurrentQueue<ILoader>();

        /* Events */
        public event Action<ProgressReport> LoadProgressed;
        public event Action LoadFinished;

        /* Properties */
        public bool IsLoading { get; private set; }
        public bool IsLoaded { get; private set; }

        /* Constructor */
        public LoaderService(ILogger logger, IPluginProvider pluginProvider)
        {
            _logger = logger;
            _pluginProvider = pluginProvider;
        }

        /* Methods */
        public async Task Load()
        {
            if (IsLoading)
            {
                throw new InvalidOperationException("Can't call ILoaderService.Load() recursively !");
            }

            if (IsLoaded) return;

            IsLoading = true;
            foreach (var loader in _loaders)
            {
                await InternalPrepareLoad(loader);
            }

            while (!_registeredLoaders.IsEmpty)
            {
                if (_registeredLoaders.TryDequeue(out var loader))
                {
                    await InternalPrepareLoad(loader);
                }
            }

            foreach (var loader in _registeredLoaders)
            {
                await InternalPrepareLoad(loader);
            }
            IsLoading = false;

            _loaders = new List<ILoader>();

            IsLoaded = true;

            LoadFinished?.Invoke();
        }

        public async Task Load(ILoader loader)
        {
            await InternalPrepareLoad(loader);
        }

        public void Initialize()
        {
            if (IsLoaded) return;

            var loaders = _pluginProvider.Container.GetExportedValues<ILoader>().ToList();

            _loaders = new List<ILoader>(OrderHelper.GetOrdered(loaders));
        }

        /// <summary>
        /// This method will ignore the <see cref="ILoader.Order"/> value and queue the registered ILoader in a FIFO Queue
        /// </summary>
        /// <param name="loader">The ILoader instance to load after all other exported ILoaders have finished loading</param>
        public void RegisterLoader(ILoader loader)
        {
            if (loader != null)
            {
                _registeredLoaders.Enqueue(loader);
            }
        }

        /* Helpers */
        private async Task InternalPrepareLoad(ILoader loader)
        {
            try
            {
                var options = loader.LoaderOptions ?? new LoadOptions();
                if (options.AutoLoad)
                {
                    await InternalLoad(loader, options.LoadThread);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error encountered while loading {Loader}...", loader.GetType().FullName);
            }
        }

        private async Task InternalLoad(ILoader loader, ThreadOption thread)
        {
            loader.ProgressChanged += OnLoaderProgressChanged;
            switch (thread)
            {
                case ThreadOption.BackgroundThread:
                    await Task.Factory.StartNew(loader.Load, default, TaskCreationOptions.None, TaskScheduler.Default).Unwrap();
                    break;
                default:
                    await UIDispatcher.OnUIThreadAsync(loader.Load);
                    break;
            }
            loader.ProgressChanged -= OnLoaderProgressChanged;
        }

        private void OnLoaderProgressChanged(ProgressReport report)
        {
            LoadProgressed?.Invoke(report);
        }
    }
}
