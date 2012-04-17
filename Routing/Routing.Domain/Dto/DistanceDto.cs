using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto
{
    public class DistanceDto
    {
        public double From_Latitide { get; set; }
        public double From_Longitude { get; set; }

        public double To_Latitide { get; set; }
        public double To_Longitude { get; set; }

        public double Km { get; set; }
        public double TimeInSeconds { get; set; }
    }
}
