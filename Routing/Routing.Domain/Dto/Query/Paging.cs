using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Query
{
    public class Paging
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string OrderBy { get; set; }
        public bool Descending { get; set; }

        public int TotalResults { get; set; }

        public bool Must_Sort()
        {
            return !string.IsNullOrEmpty(OrderBy);
        }

        public string Get_Ordering_String()
        { 
            var query = OrderBy;
            if (Descending)
                query += " desc";
            return query;
        }
    }
}
