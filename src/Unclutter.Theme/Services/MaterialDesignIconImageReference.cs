using System.Collections.Generic;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Unclutter.SDK.Images;

namespace Unclutter.Theme.Services
{
    public class MaterialDesignIconImageReference : TypedImageReference
    {
        public MaterialDesignIconImageReference(PackIconKind kind) 
            : this(kind, -1) { }

        public MaterialDesignIconImageReference(PackIconKind kind, Brush background = null, Brush foreground = null) 
            : this(kind, -1, background, foreground) { }

        public MaterialDesignIconImageReference(PackIconKind kind, int size, Brush background = null, Brush foreground = null) 
            : base(typeof(PackIcon))
        {
            PropsMap = new Dictionary<string, object>
            {
                ["Kind"] = kind
            };

            if (size >= 0)
            {
                PropsMap["Width"] = size;
                PropsMap["Height"] = size;
            }

            if (background != null)
            {
                PropsMap["Background"] = background;
            }

            if (foreground != null)
            {
                PropsMap["Foreground"] = foreground;
            }
        }
    }
}
