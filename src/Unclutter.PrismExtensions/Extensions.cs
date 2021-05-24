using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Unclutter.PrismExtensions
{
    public static class Extensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>();
        }
        public static void AutoWireViewModel(this object viewOrViewModel)
        {
            if (viewOrViewModel is FrameworkElement { DataContext: null } view && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }
        }
    }
}
