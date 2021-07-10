using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Unclutter.SDK.Common;
using Unclutter.SDK.Progress;

namespace Unclutter.SDK.Plugins
{
    public interface ILoader : IOrderedObject
    {
        event Action<ProgressReport> ProgressChanged;
        LoadOptions LoaderOptions { get; }
        Task Load();
    }

    public class LoadOptions
    {
        public bool AutoLoad { get; set; } = true;
        public ThreadOption LoadThread { get; set; } = ThreadOption.BackgroundThread;
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportLoaderAttribute : ExportAttribute
    {
        public ExportLoaderAttribute() : base(typeof(ILoader)) { }
    }
}
