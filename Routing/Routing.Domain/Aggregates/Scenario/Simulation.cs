using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Aggregates
{
    public class Simulation
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public DateTime Created { get; set; }
     
        // parametri vari di simulazione e risultati
        public Location Starting_Location { get; set; }
        public Location Returning_Location { get; set; }

        //public DenormalizedReference<Scenario> Scenario { get; set; }

        public IEnumerable<Voyage> Voyages { get; set; }
    }

}
