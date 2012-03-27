using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Aggregates
{
    public class Voyage
    {
        public DateTime Departing { get; set; }
        public TimeSpan Exitmated_Time { get; set; }
        public double Extimated_Lenght_Km { get; set; }

        public List<int> Orders { get; set; }
        //public IEnumerable<Order> Orders { get; set; }

    }

 

}
