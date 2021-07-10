using System;
using System.ComponentModel.Composition;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Plugins
{
    public interface ICommandLineArgumentsHandler : IOrderedObject
    {
        void Handle(string[] args);
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportCommandLineArgumentsHandlerAttribute : ExportAttribute
    {
        public ExportCommandLineArgumentsHandlerAttribute() : base(typeof(ICommandLineArgumentsHandler)) { }
    }
}
