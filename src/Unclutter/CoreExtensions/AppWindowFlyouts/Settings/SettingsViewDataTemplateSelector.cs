using System.Windows;
using System.Windows.Controls;
using Unclutter.SDK.App;

namespace Unclutter.CoreExtensions.AppWindowFlyouts.Settings
{
    public class SettingsViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var templateName = item switch
            {
                GroupSettingsView => "GroupSettingsView",
                SettingsView => "SettingsView",
                _ => ""
            };

            if (templateName == "")
            {
                return null;
            }

            var element = (FrameworkElement)container;
            return element.FindResource(templateName) as DataTemplate;
        }
    }
}
