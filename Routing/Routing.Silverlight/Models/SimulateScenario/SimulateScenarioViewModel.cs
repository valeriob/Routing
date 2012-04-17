using System;
using System.Net;
using System.Linq;
using ReactiveUI;
using Routing.Silverlight.ServiceReferences;
using System.Collections.ObjectModel;
using Routing.Silverlight.ScenarioService;
using maps = Silverlight.Common.Maps;

namespace Routing.Silverlight.Models.SimulateScenario
{
    public class SimulateScenarioViewModel : ReactiveObject
    {
        public string ScenarioId { get; set; }
        public ObservableCollection<SimulationDto> Simulations { get; protected set; }
        public ObservableCollection<RouteViewModel> Routes { get; set; }

        private ScenarioDto _Scenario;
        public ScenarioDto Scenario
        {
            get { return _Scenario; }
            set { _Scenario = value; this.RaisePropertyChanged(s => s.Scenario); }
        }

        private SimulationDto _SelectedSimulation;
        public SimulationDto SelectedSimulation
        {
            get { return _SelectedSimulation; }
            set { _SelectedSimulation = value; this.RaisePropertyChanged(s => s.SelectedSimulation); }
        }
        

        public SimulateScenarioViewModel()
        {
            BuildDesignData();
        }
        

        public SimulateScenarioViewModel(string scenarioId)
        {
            Simulations = new ObservableCollection<SimulationDto>();
            Routes = new ObservableCollection<RouteViewModel>();
            ScenarioId = scenarioId;

            Refresh_Scenario();
        }

        protected void Refresh_Scenario()
        {
            var service = ServiceHelper.Scenario_Client();
            service.Get_ScenarioCompleted += (sender, e) => 
            {
                Scenario = e.Result;
                Simulations.Clear();
                Simulations.AddRange( Scenario.Simulations);
                SelectedSimulation = Scenario.Simulations.FirstOrDefault();

                Refresh_Routes();
            };
            service.Get_ScenarioAsync(ScenarioId);
        }

        protected void Refresh_Routes_Old()
        {
            if (SelectedSimulation == null)
            {
                SelectedSimulation = new SimulationDto {  Voyages = new ObservableCollection<VoyageDto>() };
                SelectedSimulation.Starting_Address = "Home";
                SelectedSimulation.Returning_Address = "Home";

                SelectedSimulation.Starting_Latitude =   44.1383781433105;
                SelectedSimulation.Starting_Longitude =   12.2387800216675;
                SelectedSimulation.Returning_Latitude =  44.1383781433105;
                SelectedSimulation.Returning_Longitude = 12.2387800216675;
            

                for (int i = 0; i < 1; i++)
                {
                    var v = new VoyageDto() { Orders = new ObservableCollection<int>() };
                    foreach(var order in Scenario.Orders)
                        v.Orders.Add(order.Number);

                    v.Departing = DateTime.Today.AddDays(1);
                    v.Exitmated_Time = TimeSpan.FromDays(1);
                    v.Extimated_Lenght_Km = 12;
                    
                    SelectedSimulation.Voyages.Add(v);
                }
                Simulations.Add(SelectedSimulation);
            }


            var service = maps.ServiceHelper.GetRouteService();
            service.CalculateRouteCompleted += (sender, e) => 
            {
                var voyage = e.UserState as VoyageDto;

                Routes.Add(new RouteViewModel(voyage, e.Result.Result.RoutePath.Points));        
            };

            foreach (var route in SelectedSimulation.Voyages)
            {
                var wp = route.Orders.Select(v => new maps.RouteService.Waypoint
                    {
                        Description = v + "",
                        Location = new maps.RouteService.Location { Latitude = GetOrder(v).Latitude, Longitude = GetOrder(v).Longitude }
                    }).To_ObservableCollection();

                wp.Add(new maps.RouteService.Waypoint { Description = SelectedSimulation.Starting_Address, Location = new maps.RouteService.Location { Latitude = SelectedSimulation.Starting_Latitude, Longitude= SelectedSimulation.Starting_Longitude } });
                //wp.Add(new maps.RouteService.Waypoint { Description = SelectedSimulation.Returning_Address, Location = new maps.RouteService.Location { Latitude = SelectedSimulation.Returning_Latitude, Longitude =SelectedSimulation.Returning_Longitude } });

                var request = new maps.RouteService.RouteRequest
                {
                    Credentials = new maps.RouteService.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                    Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                    Waypoints = wp

                };
                request.Options = new maps.RouteService.RouteOptions();
                request.Options.RoutePathType = maps.RouteService.RoutePathType.Points;

                service.CalculateRouteAsync(request, route);
            }
        }

        protected void Refresh_Routes()
        {
            if (SelectedSimulation == null)
            {
                SelectedSimulation = new SimulationDto {  Voyages = new ObservableCollection<VoyageDto>() };
                SelectedSimulation.Starting_Address = "Home";
                SelectedSimulation.Returning_Address = "Home";

                SelectedSimulation.Starting_Latitude =   44.1383781433105;
                SelectedSimulation.Starting_Longitude =   12.2387800216675;
                SelectedSimulation.Returning_Latitude =  44.1383781433105;
                SelectedSimulation.Returning_Longitude = 12.2387800216675;
            

                for (int i = 0; i < 1; i++)
                {
                    var v = new VoyageDto() { Orders = new ObservableCollection<int>() };
                    foreach(var order in Scenario.Orders)
                        v.Orders.Add(order.Number);

                    v.Departing = DateTime.Today.AddDays(1);
                    v.Exitmated_Time = TimeSpan.FromDays(1);
                    v.Extimated_Lenght_Km = 12;
                    
                    SelectedSimulation.Voyages.Add(v);
                }
                Simulations.Add(SelectedSimulation);
            }

            var calculator = new BingRouteCalculator();
            calculator.Set_Starting_Point(SelectedSimulation.Starting_Latitude, SelectedSimulation.Starting_Longitude);
            calculator.Set_Ending_Point(SelectedSimulation.Returning_Latitude, SelectedSimulation.Returning_Longitude);

            foreach (var spot in SelectedSimulation.Voyages)
            {
                foreach(var orderId in spot.Orders)
                {
                    var order = GetOrder(orderId);
                    calculator.Add_Tour(order.Latitude, order.Longitude);
                }
            }

            var scheduler = System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext();

            calculator.Calculate_Routes()
                .ContinueWith((t) => 
                {
                    var results = calculator.Result.ToList();


                    Routes.Add(new RouteViewModel(null, results.Select(r => new maps.RouteService.Location { Latitude = r.Latitude, Longitude = r.Longitude })));
                }, scheduler);


        }

        protected OrderDto GetOrder(int number)
        {
            return Scenario.Orders.Single(o => o.Number == number);

        }

        //private maps.RouteService.Waypoint GeocodeResultToWaypoint(maps.RouteService.GeocodeResult result)
        //{
        //    PlatformServices.Waypoint waypoint = new PlatformServices.Waypoint();
        //    waypoint.Description = result.DisplayName;
        //    waypoint.Location = new Location();
        //    waypoint.Location.Latitude = result.Locations[0].Latitude;
        //    waypoint.Location.Longitude = result.Locations[0].Longitude;
        //    return waypoint;
        //}


        public void BuildDesignData()
        {
            Simulations = new ObservableCollection<SimulationDto>();
            ScenarioId = "scenario1";

            for (int i = 0; i < 10; i++)
            {
                var simul = new SimulationDto { Number = i, Name="Simulazione "+i, Created = DateTime.Now.AddDays(-i) };
                Simulations.Add(simul);
            }

            Routes = new ObservableCollection<RouteViewModel>();

            for (int i = 0; i < 10; i++)
            {
                //var route = new RouteViewModel() { };

                //Routes.Add(route);
            }
        }
    }
}
