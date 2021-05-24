using System;
using System.Threading.Tasks;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Loader
{
    public interface ILoader
    {
        event Action<ProgressReport> ProgressChanged;
        LoadOptions LoaderOptions { get; }
        Task Load();
    }
}
