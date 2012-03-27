using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Silverlight.Common.Helpers
{
    public static class ControlsHelper
    {
        public static List<T> GetChildObjects<T>(this DependencyObject obj, string name)
        {
            var retVal = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                object c = VisualTreeHelper.GetChild(obj, i);
                if (c.GetType().FullName == typeof(T).FullName && (String.IsNullOrEmpty(name) || ((FrameworkElement)c).Name == name))
                {
                    retVal.Add((T)c);
                }
                var gc = ((DependencyObject)c).GetChildObjects<T>(name);
                if (gc != null)
                    retVal.AddRange(gc);
            }

            return retVal;
        }

        public static T GetChildObject<T>(this DependencyObject obj, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                object c = VisualTreeHelper.GetChild(obj, i);
                if (c.GetType().FullName == typeof(T).FullName && (String.IsNullOrEmpty(name) || ((FrameworkElement)c).Name == name))
                {
                    return (T)c;
                }
                object gc = ((DependencyObject)c).GetChildObject<T>(name);
                if (gc != null)
                    return (T)gc;
            }

            return null;
        }

    }
}
