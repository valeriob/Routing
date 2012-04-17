using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Aggregates;

namespace Routing.Domain.Services
{
    public abstract class Routing_Algorithm
    {
        protected Scenario Scenario { get; private set; }
        public Simulation Result { get; protected set; }

        public Routing_Algorithm(Scenario scenario)
        {
            Scenario = scenario;
        }

        public abstract void Execute_Simulation();

    }

   
}
