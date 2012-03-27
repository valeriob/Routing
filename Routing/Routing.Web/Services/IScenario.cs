using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Routing.Domain.Dto;
using Routing.Domain.Dto.Command;

namespace Routing.Web.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReferences" in both code and config file together.
    [ServiceContract]
    public interface IScenario
    {
        [OperationContract]
        void Create_Scenario(Create_Scenario command);

        [OperationContract]
        ScenarioDto Get_Scenario(string id);

        //[OperationContract]
        //SimulationDto Get_Simulation(string scenarioId, int number);
    }
}
