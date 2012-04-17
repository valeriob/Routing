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
    [ServiceContract]
    public interface IScenario
    {
        [OperationContract]
        void Create_Scenario(Create_Scenario command);

        [OperationContract]
        ScenarioDto Get_Scenario(string id);


        [OperationContract]
        void Execute_Simulation(Execute_Simulation cmd);
    }
}
