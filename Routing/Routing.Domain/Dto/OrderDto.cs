using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto
{
    public class OrderDto
    {
        public int Number { get; set; }

        public string ExternalId { get; set; }

        public DateTime Delivering { get; set; }
        public string Description { get; set; }

        public string DestinationId { get; set; }
        public string DestinationExternalId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }

        public double Volume { get; set; }
        public string Volume_Unit { get; set; }

        public double Weight { get; set; }
        public string Weight_Unit { get; set; }

        // Volume, weight ?

    }
}
