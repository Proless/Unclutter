using System;
using System.ComponentModel.Composition;

namespace Unclutter.SDK.Plugins
{
    public interface IViewModelProcessor
    {
        void ProcessViewModel(object viewmodel, object view);
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportViewModelProcessorAttribute : ExportAttribute
    {
        public ExportViewModelProcessorAttribute() : base(typeof(IViewModelProcessor)) { }
    }
}
