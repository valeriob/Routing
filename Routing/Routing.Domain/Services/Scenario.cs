using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Dto.Command;
using Raven.Client;
using Routing.Domain.Aggregates;
using Routing.Domain.ValueObjects;
using Routing.Domain.Dto.Events;


namespace Routing.Domain.Services
{
    public class ScenarioService
    {
        IDocumentSession DocumentSession;
        

        public ScenarioService(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;
           
        }


        // meglio fare solo insert.
        public void Create_Scenario(Create_Scenario cmd)
        {
            if (!string.IsNullOrWhiteSpace(cmd.ScenarioId_To_Be_Deleted))
            {
                var scenarioToBeDeleted = DocumentSession.Query<Scenario>().SingleOrDefault(s => s.Id == cmd.ScenarioId_To_Be_Deleted);
                scenarioToBeDeleted.Delete();
                //Bus.Publish(new Scenario_Deleted { ScenarioId = cmd.ScenarioId_To_Be_Deleted });
            }

            var scenarioDto = cmd.Scenario;

            // Aggiornamento anafrafiche?, da rivedere
   
            var destinationIds = scenarioDto.Orders.Select(o=> o.DestinationId);
            var destinations = DocumentSession.Advanced.LuceneQuery<Destination>().WhereIn("Id", destinationIds).ToList();

            foreach (var order in scenarioDto.Orders)
            {
                if (order.DestinationExternalId.IsNullOrEmpty())
                    continue;
                var destination = new Destination
                {
                    Id = order.DestinationId,
                    Name = "None",
                    ExternalId = order.DestinationExternalId,
                    UserId = scenarioDto.UserId,
                    Location = new Location(order.Latitude, order.Longitude),
                    Address = Address.Parse(order.Address),
                };
                //DocumentSession.Store(destination);
                destinations.Add(destination);
            }


            var scenario = new Scenario
            { 
                Id = scenarioDto.Id,
                UserId = scenarioDto.UserId,
                Created = DateTime.Now,
                Date = scenarioDto.Date,
                Name = scenarioDto.Name,
                Orders = scenarioDto.Orders.Select(o => new Order
                {
                    ExternalId = o.ExternalId, 
                    Destination = destinations.Where(d=> d.Id == o.DestinationId).DefaultIfEmpty(Destination.Empty).SingleOrDefault(),

                    Volume = new Quantity(o.Volume, new Unit(o.Volume_Unit)),
                    Weight = new Quantity(o.Weight, new Unit(o.Weight_Unit)), 
                    Location = new Location(o.Latitude, o.Longitude), 
                    Address = Address.Parse(o.Address),
                }).ToList(),

                Distances = scenarioDto.Distances.Select(d=> new Distance
                {
                    From = new Location(d.From_Latitide, d.From_Longitude), 
                    To = new Location(d.To_Latitide, d.To_Longitude), 
                    Km = d.Km, 
                    Time= TimeSpan.FromSeconds(d.TimeInSeconds)
                }).ToList(),

            };

            DocumentSession.Store(scenario);

            DocumentSession.SaveChanges();
        }

   
    }
}
