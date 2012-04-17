using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Dto.Command;
using Raven.Client;
using Raven.Client.Linq;
using Routing.Domain.Aggregates;
using Routing.Domain.Dto;
using Routing.Domain.Dto.Abstracts;

namespace Routing.Domain.ReadModel
{
    public class Scenario_ReadModel
    {
        IDocumentSession Session;
        public Scenario_ReadModel(IDocumentSession documentSession)
        {
            Session = documentSession;
        }

        public ScenarioDto Get_Scenario(string id)
        {
            var scenario = Try_Get_Scenario(id);

            var dto= new ScenarioDto
            {
                Id = scenario.Id,
                Date = scenario.Date,
                Name = scenario.Name,
                UserId = scenario.UserId,
                Deliveries = scenario.Deliveries.Select(o => new DeliveryDto 
                { 
                   Number = o.Number, 
                   Latitude = o.Location.Latitude, 
                   Longitude = o.Location.Longitude,
                }).ToList(),  
                Simulations = scenario.Simulations.Select(s=> new SimulationDto
                {
                    Created = s.Created,
                    Number = s.Number,
                    Name = s.Name,
                    Voyages = s.Voyages.Select(v => new VoyageDto
                    {
                        Departing = v.Departing,
                        Exitmated_Time = v.Exitmated_Time,
                        Extimated_Lenght_Km = v.Extimated_Lenght_Km,
                        Orders = v.Orders
                    }).ToList()
                }).ToList()
            };

            return dto;
        }

 
        //public SimulationDto Get_Simulation(string scenarioId, int number)
        //{
        //    var scenario = Try_Get_Scenario(scenarioId);

        //    var simulation = scenario.Simulations.SingleOrDefault(s => s.Number == number);
        //    if(simulation == null)
        //        throw new Exception(string.Format("Scenario {0} has no smulation {1} !", scenarioId, number));

        //    return new SimulationDto
        //    {
        //        Created = simulation.Created,
        //        Number = simulation.Number,
        //        Name = simulation.Name,
        //        Voyages = simulation.Voyages.Select(v => new VoyageDto
        //        {   
        //             Departing = v.Departing, 
        //             Exitmated_Time = v.Exitmated_Time, 
        //             Extimated_Lenght_Km = v.Extimated_Lenght_Km, 
        //             Orders = v.Orders
        //        })
        //    };
        //}

        protected Scenario Try_Get_Scenario(string id)
        {
            var scenario = Session.Query<Scenario>().Where(s => s.Id == id).SingleOrDefault();
            if (scenario == null)
                throw new Exception(string.Format("Scenario {0} not found !", id));
            return scenario;
        }
    }

    
}
