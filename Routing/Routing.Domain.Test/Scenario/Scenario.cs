using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routing.Domain.Infrastructure;
using Autofac;
using Routing.Domain.Services;
using Routing.Domain.Dto.Command;


namespace Routing.Domain.Test.Create_Simulation
{
    [TestClass]
    public class Scenario
    {
        [TestMethod]
        public void Insert_New_Scenario()
        {
            var container = Container.Init_Container();
            var service = container.Resolve<ScenarioService>();


            var cmd = new Create_Scenario 
            { 
             
            };
            service.Create_Scenario(cmd);
        }

 
    }
}
