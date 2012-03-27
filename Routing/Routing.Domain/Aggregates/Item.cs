using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Aggregates
{
    public class Item : INamedDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
    }
}
