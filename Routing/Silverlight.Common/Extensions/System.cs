using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System
{
    public static class System_Extensions
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> source)
        {
            foreach (var item in source)
                collection.Add(item);
        }

        public static ObservableCollection<T> To_ObservableCollection<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }

        public static Queue<T> To_Queue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }
    }
}
