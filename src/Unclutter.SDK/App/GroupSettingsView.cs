using System;
using System.Collections.Generic;
using Unclutter.SDK.Services;
using Unclutter.SDK.Settings;

namespace Unclutter.SDK.App
{
    public class GroupSettingsView : SettingsView
    {
        private readonly List<SettingsView> _views = new List<SettingsView>();
        public IEnumerable<SettingsView> Views => OrderHelper.GetOrdered(_views);

        public GroupSettingsView(Type viewType, string label, ISettings settings)
            : base(viewType, label, settings) { }

        public void AddSubView(SettingsView view)
        {
            if (view != null)
            {
                _views.Add(view);
            }
        }
    }
}
