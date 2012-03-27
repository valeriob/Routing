using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Routing.Silverlight.SimulationService;
using Routing.Silverlight.ServiceReferences;
using Routing.Silverlight.ScenarioService;

namespace Routing.Silverlight
{
    public static class ServiceHelper
    {
        public static SimulationServiceClient Simulation_Client()
        {
            return new SimulationServiceClient();
        }

        public static ScenarioClient Scenario_Client()
        {
            return new ScenarioClient();
        }

        public static ReferencesClient References_Client()
        {
            return new  ReferencesClient();
        }
    }
}
