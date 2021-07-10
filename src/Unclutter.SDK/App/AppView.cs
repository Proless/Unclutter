using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;

namespace Unclutter.SDK.App
{
    public class AppView : IDisplayItem, IOrderedObject
    {
        public string Identifier { get; }
        public string Label { get; }
        public Type ViewType { get; }

        public string Hint { get; set; }
        public ImageReference Icon { get; set; }
        public double? Order { get; set; }

        public AppView(Type viewType, string label)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException(nameof(label));
            }

            ViewType = viewType ?? throw new ArgumentNullException(nameof(viewType));
            Label = label;

            Identifier = ViewType.GUID.ToString();
        }
    }
}
