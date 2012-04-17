using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Aggregates;

namespace Routing.Domain.Services
{
    public class Algorithm_Dummy : Routing_Algorithm
    {
        public Algorithm_Dummy(Scenario scenario) : base(scenario)
        {

        }


        public override void Execute_Simulation()
        {
            var simulation = new Simulation 
            { 
                Created = DateTime.Now, Name = "asd", 
            };

            var voyage = new Voyage
            {  
                Departing = DateTime.Today.AddDays(1),
                Orders = Scenario.Deliveries.Select(d=> d.Number).ToList()
            };
            simulation.Append_Voyage(voyage);

            var from = Scenario.Deliveries.First().Location;
            var to = Scenario.Deliveries.Last().Location;

            var current = from;
            var path = new List<Distance>();
            foreach (var delivery in Scenario.Deliveries.Skip(1))
            {
                if (delivery.Location == to)
                    break;

                var next = Scenario.Distances.Single(d => d.From == from && d.To == delivery.Location);
                path.Add(next);
                current = delivery.Location;
            }

            voyage.Extimated_Lenght_Km = path.Sum(p => p.Km);
            voyage.Exitmated_Time = path.Select(p => p.Time).Aggregate((a, b) => a + b);
            
        }


    }
}
