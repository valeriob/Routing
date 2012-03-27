using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Aggregates
{
    public class Order
    {
        public int Number { get; set; }
        public string ExternalId { get; set; }

        public Address Address { get; set; }
        public Location Location { get; set; }

        public DenormalizedReference<Destination> Destination { get; set; }

        public Quantity Volume { get; set; }
        public Quantity Weight { get; set; }
    }
}
