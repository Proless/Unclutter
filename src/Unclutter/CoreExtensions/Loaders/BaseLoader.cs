using System;
using System.Threading.Tasks;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Progress;

namespace Unclutter.CoreExtensions.Loaders
{
    public abstract class BaseLoader : ILoader
    {
        public double? Order { get; set; }
        public event Action<ProgressReport> ProgressChanged;
        public LoadOptions LoaderOptions { get; set; }
        public abstract Task Load();
        protected void OnProgressChanged(ProgressReport report)
        {
            ProgressChanged?.Invoke(report);
        }
    }
}
