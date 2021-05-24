using System.Windows;
using System.Windows.Controls;

namespace Unclutter.Theme.Controls
{
    [TemplatePart(Name = PART_Icon, Type = typeof(ContentControl))]
    [TemplatePart(Name = PART_TitleBar, Type = typeof(UIElement))]
    [TemplatePart(Name = PART_CloseButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_MaxButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_MinButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_Content, Type = typeof(ContentPresenter))]
    public partial class CustomWindow : Window
    {
        #region TemplateParts
        private const string PART_Icon = "PART_Icon";
        private const string PART_TitleBar = "PART_TitleBar";
        private const string PART_CloseButton = "PART_CloseButton";
        private const string PART_MaxButton = "PART_MaxButton";
        private const string PART_MinButton = "PART_MinButton";
        private const string PART_Content = "PART_Content";
        #endregion

        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }
    }
}
