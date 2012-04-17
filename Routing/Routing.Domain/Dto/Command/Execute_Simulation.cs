using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Command
{
    public class Execute_Simulation
    {
        public string ScenarioId { get; set; }
        public Algorithm Algorithm { get; set; }
    }

    public enum Algorithm { Dummy, Random, Clark_Wright, Sweep }
}
