using System;
using System.Threading.Tasks;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Progress;

namespace Unclutter.Services.Loader
{
    public interface ILoaderService
    {
        event Action<ProgressReport> LoadProgressed;
        event Action LoadFinished;
        bool IsLoading { get; }
        bool IsLoaded { get; }
        Task Load();
        Task Load(ILoader loader);
    }
}
