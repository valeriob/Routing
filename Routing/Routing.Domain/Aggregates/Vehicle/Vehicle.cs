using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Aggregates
{
    public class Vehicle : INamedDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }

        public IEnumerable<Quantity> Limits { get; set; }
    }
}
