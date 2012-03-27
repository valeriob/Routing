using System;
using System.Net;
using System.Linq;
using Routing.Silverlight.ServiceReferences;
using common=Silverlight.Common;

namespace Routing.Silverlight.Models.References
{
    public static class Extensions
    {
        public static void Set_Paging<T>(this Paging destination, common.PagedSortableCollectionView<T> source)
        {
            destination.PageIndex = source.PageIndex;
            destination.PageSize = source.PageSize;

            var sorting = source.SortDescriptions.FirstOrDefault();
            if (source != null)
            {
                destination.OrderBy = sorting.PropertyName;
                destination.Descending = sorting.Direction == System.ComponentModel.ListSortDirection.Descending;
            }
        }
    }
}
