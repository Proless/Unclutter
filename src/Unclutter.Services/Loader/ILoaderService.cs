using System;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Loader;

namespace Unclutter.Services.Loader
{
    public interface ILoaderService
    {
        event Action<ProgressReport> ProgressChanged;
        event Action Finished;
        bool IsLoading { get; }
        Task Load();
        Task Load(ILoader loader);
        void RegisterLoader(ILoader loader);
        void RegisterLoader<T>() where T : ILoader;
    }
}
