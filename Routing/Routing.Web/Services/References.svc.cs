using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
using Routing.Domain.Dto;
using Routing.Domain.Dto.Query;
using Routing.Domain.ReadModel;
using System.Security.Permissions;
using Routing.Domain.Dto.Abstracts;

namespace Routing.Web.Services
{
    [AspNetCompatibilityRequirements( RequirementsMode= AspNetCompatibilityRequirementsMode.Required)]
    public class References : IReferences
    {
        References_ReadModel ReadModel;
        public References(References_ReadModel readModel)
        {
            ReadModel = readModel;
        }

        //[PrincipalPermission(SecurityAction.Demand)]
        public IEnumerable<DestinationDto> Known_Destinations(SearchDestinations query)
        {
            
            return ReadModel.Known_Destinations(query);
        }

        public IEnumerable<AbstractScenarioDto> Search_Scenarios(SearchScenarios query)
        {
            return ReadModel.Search_Scenarios(query);
        }

        public AbstractScenarioDto Get_AbstractScenario(string id)
        {
            return ReadModel.Get_AbstractScenario(id);
        }

        public IEnumerable<AbstractSimulationDto> Get_AbstractSiumulations(string scenarioId)
        {
            return ReadModel.Get_Siumulations(scenarioId);
        }
    }
}
