using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Plugins
{
    public interface IStartupAction : IDisplayItem, IOrderedObject
    {
        ICommand Action { get; }
        void Initialize();
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportStartupActionAttribute : ExportAttribute
    {
        public ExportStartupActionAttribute() : base(typeof(IStartupAction)) { }
    }
}
