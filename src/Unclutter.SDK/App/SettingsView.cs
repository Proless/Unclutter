using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Settings;

namespace Unclutter.SDK.App
{
    public class SettingsView : IDisplayItem, IOrderedObject
    {
        public string Identifier { get; }
        public Type ViewType { get; }
        public ISettings Settings { get; }
        public string Label { get; }

        public ImageReference Icon { get; set; }
        public string Hint { get; set; }
        public double? Order { get; set; }

        public SettingsView(Type viewType, string label, ISettings settings)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException(nameof(label));
            }

            ViewType = viewType ?? throw new ArgumentNullException(nameof(viewType));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Label = label;
            Identifier = ViewType.GUID.ToString();
        }
    }
}
