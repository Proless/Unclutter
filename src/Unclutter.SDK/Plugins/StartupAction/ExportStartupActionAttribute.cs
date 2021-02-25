using System;
using System.ComponentModel.Composition;

namespace Unclutter.SDK.Plugins.StartupAction
{
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ExportStartupActionAttribute : ExportAttribute, IStartupActionMetadata
    {
        public string Label { get; }
        public string HintLabel { get; }
        public string IconId { get; }
        public string GroupName { get; }
        public double ItemPriority { get; }

        public ExportStartupActionAttribute(string label, string iconId, string hintLabel = "", string groupName = "", double itemPriority = double.MaxValue) : base(typeof(IStartupAction))
        {
            Label = label;
            IconId = iconId;
            GroupName = groupName;
            ItemPriority = itemPriority;
            HintLabel = hintLabel;
        }
    }
}
