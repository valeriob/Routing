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
using Routing.Domain.Services;
using Routing.Domain.Dto.Command;

namespace Routing.Web.Services
{
    [AspNetCompatibilityRequirements( RequirementsMode= AspNetCompatibilityRequirementsMode.Required)]
    public class Scenario : IScenario
    {
        ScenarioService Service;
        Scenario_ReadModel ReadModel;
        public Scenario(ScenarioService service, Scenario_ReadModel readModel)
        {
            Service = service;
            ReadModel = readModel;
        }

        //[PrincipalPermission(SecurityAction.Demand)]
        public void Create_Scenario(Create_Scenario command)
        {
            Service.Create_Scenario(command);
        }

        public ScenarioDto Get_Scenario(string id)
        {
            return ReadModel.Get_Scenario(id);
        }

        public void Execute_Simulation(Execute_Simulation cmd)
        {
            Service.Execute_Simulation(cmd);
        }

    }
}
