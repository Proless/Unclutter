using Prism.Modularity;
using System;
using System.ComponentModel.Composition;

namespace Unclutter.Modules
{
    public interface IModuleProcessor
    {
        void ProcessModule(IModuleInfo moduleInfo, IModule instance);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportModuleProcessorAttribute : ExportAttribute
    {
        public ExportModuleProcessorAttribute() : base(typeof(IModuleProcessor)) { }
    }
}
