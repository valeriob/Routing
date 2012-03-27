using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Routing.Domain.Dto;
using Routing.Domain.Dto.Query;
using Routing.Domain.Dto.Abstracts;

namespace Routing.Web.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReferences" in both code and config file together.
    [ServiceContract]
    public interface IReferences
    {
        [OperationContract]
        IEnumerable<DestinationDto> Known_Destinations(SearchDestinations query);

        [OperationContract]
        IEnumerable<AbstractScenarioDto> Search_Scenarios(SearchScenarios query);

        [OperationContract]
        AbstractScenarioDto Get_AbstractScenario(string id);

        [OperationContract]
        IEnumerable<AbstractSimulationDto> Get_AbstractSiumulations(string scenarioId);
    }
}
