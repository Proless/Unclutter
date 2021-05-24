using System;
using System.ComponentModel.Composition;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Plugins
{
    public interface IEventSource
    {
        event EventHandler<object> EventSourceChanged;
        ThreadOption PublishThread { get; }
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportEventSourceAttribute : ExportAttribute
    {
        public ExportEventSourceAttribute() : base(typeof(IEventSource))
        {
        }
    }
}
