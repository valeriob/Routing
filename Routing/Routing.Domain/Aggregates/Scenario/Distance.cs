using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Aggregates
{
    public class Distance
    {
        public Location From { get; set; }
        public Location To { get; set; }

        public double Km { get; set; }
        public TimeSpan Time { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", From, To);
        }
    }
}
