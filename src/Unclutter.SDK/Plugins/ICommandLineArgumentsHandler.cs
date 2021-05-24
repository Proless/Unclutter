using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Unclutter.SDK.Plugins
{
    public interface ICommandLineArgumentsHandler
    {
        double Priority { get; }
        Task HandleAsync(string[] args);
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportCommandLineArgumentsHandlerAttribute : ExportAttribute
    {
        public ExportCommandLineArgumentsHandlerAttribute() : base(typeof(ICommandLineArgumentsHandler)) { }
    }
}
