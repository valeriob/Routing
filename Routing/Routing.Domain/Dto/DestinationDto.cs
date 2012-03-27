using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto
{
    public class DestinationDto
    {
        public string Id { get; set; }
        public string ExternalId { get; set; }

        public string Address { get; set; }
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }
    }
}
