using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Aggregates
{
    public class Identity
    {
        public string Provider { get; set; }
        public string Id { get; set; }
        public string Display_Name { get; set; }
    }
}
