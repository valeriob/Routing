using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Dto.Validation;

namespace Routing.Domain.Dto.Command
{
    public class Create_Scenario
    {
        public ScenarioDto Scenario { get; set; }
        public string ScenarioId_To_Be_Deleted { get; set; }

        public bool IsValid()
        {
            return !Scenario.Orders.Any(o => o.DestinationExternalId.IsNullOrEmpty() && o.Latitude == 0 && o.Longitude == 0);
        }
    }

    public class ScenarioDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public List<OrderDto> Orders { get; set; }

        public List<SimulationDto> Simulations { get; set; }

        public List<DistanceDto> Distances { get; set; }
    }

}
