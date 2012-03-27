using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routing.Domain.Infrastructure;
using Autofac;
using Routing.Domain.Services;

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
    }
}
