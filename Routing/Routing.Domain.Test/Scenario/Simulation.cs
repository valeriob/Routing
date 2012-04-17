using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routing.Domain.Infrastructure;
using Autofac;
using Routing.Domain.Services;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Routing.Domain.Dto.Command;
using Raven.Client;

namespace Routing.Domain.Test.Create_Simulation
{
    [TestClass]
    public class Simulation
    {
        [TestMethod]
        public void Init_New_Simulation()
        {
            var container = Container.Init_Container();
            var service = container.Resolve<SimulationService>();

            

        }

        [TestMethod]
        public void Test_Algorithm_Random()
        {
            var container = Container.Init_Container().Configure_Raven_For_Testing();

            var service = container.Resolve<ScenarioService>();
            var documentStore = container.Resolve<IDocumentSession>();

            var cmd = new Create_Scenario 
            {
                Scenario = Test_Helper.Build_Random_Scenario()
            };

            service.Create_Scenario(cmd);

            var scenario = documentStore.Query<Routing.Domain.Aggregates.Scenario>().First();

            service.Execute_Simulation(new Execute_Simulation { Algorithm = Algorithm.Random, ScenarioId = scenario.Id });
        }

         

    }
}
