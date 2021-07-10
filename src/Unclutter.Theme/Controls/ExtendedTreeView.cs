using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Unclutter.Theme.Controls
{
    public class ExtendedTreeView : TreeView
    {
        public ExtendedTreeView()
        {
            SelectedItemChanged += ExtendedTreeView_SelectedItemChanged;
        }

        private void ExtendedTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            BindableSelectedItem = e.NewValue;
            if (SelectedItemCommand != null)
            {
                if (SelectedItemCommand.CanExecute(e.NewValue))
                {
                    SelectedItemCommand.Execute(e.NewValue);
                }
            }
        }


        public object BindableSelectedItem
        {
            get => GetValue(BindableSelectedItemProperty);
            set => SetValue(BindableSelectedItemProperty, value);
        }
        public static readonly DependencyProperty BindableSelectedItemProperty =
            DependencyProperty.Register(nameof(BindableSelectedItem),
                typeof(object),
                typeof(ExtendedTreeView), new FrameworkPropertyMetadata(new object(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public ICommand SelectedItemCommand
        {
            get => (ICommand)GetValue(SelectedItemCommandProperty);
            set => SetValue(SelectedItemCommandProperty, value);
        }
        public static readonly DependencyProperty SelectedItemCommandProperty =
            DependencyProperty.Register(
                nameof(SelectedItemCommand),
                typeof(ICommand),
                typeof(ExtendedTreeView),
                new FrameworkPropertyMetadata(null));
    }
}
