using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Aggregates
{
    public class Destination : INamedDocument
    {
        public static readonly Destination Empty = new Destination();

        public Destination()
        {
            Location = Location.Empty;
            Address = Address.Empty;
        }

        public string Id { get; set; }
        public string ExternalId { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }
        public Location Location { get; set; }
        public Address Address { get; set; }
    }
}
