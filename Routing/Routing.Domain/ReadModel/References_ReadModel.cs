using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Routing.Domain.Dto;
using Raven.Client;
using Routing.Domain.Aggregates;
using System.Linq.Expressions;
using Routing.Domain.Dto.Query;
using Raven.Client.Linq;
using Routing.Domain.Dto.Abstracts;

namespace Routing.Domain.ReadModel
{
    public class References_ReadModel
    {
        IDocumentSession Session;
        public References_ReadModel(IDocumentSession documentSession)
        {
            Session = documentSession;
        }

        public IEnumerable<DestinationDto> Known_Destinations(SearchDestinations query)
        {
            RavenQueryStatistics stats;

            var source = Session.Advanced.LuceneQuery<Destination>()
                .Statistics(out stats);

            if (query.Name.IsNotEmpty())
                source = source.WhereEquals("Name", query.Name);

            if(query.HasValidLocation())
                source = source.WithinRadiusOf(10, query.NearBy_Latitude.Value, query.NearBy_Longitude.Value);

            if(query.Address.IsNotEmpty())
                source = source.Search("Address.City", query.Address);

            var results = source.To_DestinationDto().ToList();
            query.TotalResults = stats.TotalResults;
            return results;
        }


        public IEnumerable<AbstractScenarioDto> Search_Scenarios(SearchScenarios query)
        {
            RavenQueryStatistics stats;

            var source = Session.Query<Scenario>()
              .Statistics(out stats)
              .Where(q=> q.UserId == query.UserId);

            if (query.From.HasValue)
                source = source.Where(s => s.Date >= query.From);

            if (query.To.HasValue)
                source = source.Where(s => s.Date <= query.To);
            
            var results = source.To_AbstractScenarioDto().Apply_Sort_And_Paging(query, s=> s.UserId).ToList();
            query.TotalResults = stats.TotalResults;

            return results;
        }

        public AbstractScenarioDto Get_AbstractScenario(string id)
        {
            RavenQueryStatistics stats;

            var source = Session.Query<Scenario>()
              .Statistics(out stats)
              .Where(q => q.Id == id);

            var scenario = source.To_AbstractScenarioDto().SingleOrDefault();
            if (scenario == null)
                throw new Exception(string.Format("Scenario {0} not found !", id));
            return scenario;
        }

        public IEnumerable<AbstractSimulationDto> Get_Siumulations(string scenarioId)
        {
            var scenario = Session.Query<Scenario>().Include(u => u.Owner).SingleOrDefault(s => s.Id == scenarioId);
            if (scenario == null)
                throw new Exception(string.Format("Scenario {0} not found !", scenarioId));

            var simulations = scenario.Simulations.Select(s => new AbstractSimulationDto
            {
                Name = s.Name,
                Number = s.Number,
                Created = s.Created,
            });
            return simulations;
        }






    }
}
