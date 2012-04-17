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


namespace System
{
    public static class UriExtensions
    {
        public static string GetValue(this Uri uri, string parameterName)
        {
            try
            {
                string query = null;
                if (uri.IsAbsoluteUri)
                    query = uri.Query;
                else
                    query = uri.ToString().Split('?')[1];

                foreach (var parameter in query.Split('&'))
                {
                    var tokens = parameter.Split('=');
                    var name = tokens[0];
                    var value = tokens[1];
                    if (name == parameterName)
                        return value;
                }
            }
            catch (Exception)
            {
            }
            return "";
        }
    }
}
