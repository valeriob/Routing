using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Query
{
    public class NearByPagedQuery : Paging
    {
        public double? NearBy_Latitude { get; set; }
        public double? NearBy_Longitude { get; set; }
        public double? NearBy_Radius { get; set; }

        public bool HasValidLocation()
        {
            return NearBy_Latitude.HasValue && NearBy_Longitude.HasValue;
        }
    }
}
