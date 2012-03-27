using System;
using System.Net;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace Silverlight.Common
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToCollection<T>(this IEnumerable<T> query)
        {
            return new ObservableCollection<T>(query);
        }
    }
}
