using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Aggregates;

namespace Routing.Domain.Services
{
    public class Algorithm_Random : Routing_Algorithm
    {
        public Algorithm_Random(Scenario scenario)
            : base(scenario)
        {

        }

        static Random random = new Random();
        public override void Execute_Simulation()
        {
            var simulation = new Simulation 
            { 
                Created = DateTime.Now, Name = "asd", 
            };
          

            var deliveries = Scenario.Deliveries.To_Queue();

            while(deliveries.Count() >0)
            {
                var usedDeliveries = deliveries.Dequeue(random.Next(Scenario.Deliveries.Count() % 4) + 1).ToList();
               
    
                var voyage = new Voyage
                {
                    Departing = DateTime.Today.AddDays(1),
                    Orders = usedDeliveries.Select(d => d.Number).ToList()
                };
                simulation.Append_Voyage(voyage);

                var from = usedDeliveries.First().Location;
                var to = usedDeliveries.Last().Location;

                var current = from;
                var path = new List<Distance>();
                if (usedDeliveries.Count == 1)
                    path.Add(new Distance { From= from, To = to });
                else
                    foreach (var delivery in usedDeliveries.Skip(1))
                    {
                        var next = Scenario.Distances.First(d => (d.From == from && d.To == delivery.Location) || (d.To == from && d.From == delivery.Location));
                        path.Add(next);
                        current = delivery.Location;

                        if (delivery.Location == to)
                            break;
                    }

                voyage.Extimated_Lenght_Km = path.Sum(p => p.Km);
                voyage.Exitmated_Time = path.Select(p => p.Time).Aggregate((a, b) => a + b);
            }
          
            
        }

 


    }
}
