using System;
using System.Net;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace System.Windows
{
    public static class Extensions
    {
        public static void Focus(this UIElement element)
        {
            var control = element as Control;
            if (control != null)
                control.Focus();
        }
    }
}

namespace System.Collection.ObjectModel
{
    public static class Extensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> source)
        {
            foreach (var item in source)
                collection.Add(item);
        }
    }
}
