using System;
using System.Net;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using maps = Silverlight.Common.Maps;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;
using System.Windows.Controls.Primitives;
using Routing.Silverlight.ServiceReferences;
using Routing.Silverlight.Address_Validation;
using System.Windows.Controls;
using Routing.Silverlight.ScenarioService;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Routing.Silverlight.Models
{
    public class ScenarioViewModel : ReactiveObject
    {
        private ObservableCollection<LocationViewModel> _Points;
        public ObservableCollection<LocationViewModel> Points
        {
            get { return _Points ?? (_Points = new ObservableCollection<LocationViewModel>()); }
            set { _Points = value; this.RaisePropertyChanged(v=>v.Points );}
        }

        private LocationViewModel _SelectedPoint;
        public LocationViewModel SelectedPoint
        {
            get { return _SelectedPoint; }
            set { _SelectedPoint = value; this.RaisePropertyChanged(v=>v.SelectedPoint );}
        }

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { _Id = value; this.RaisePropertyChanged(v=>v.SelectedPoint );}
        }

        
        
        
        public ScenarioViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
                return;

            var service = ServiceHelper.Simulation_Client();

            //Points.Add(new LocationViewModel { Search_Address = "Faenza" });
            //Points.Add(new LocationViewModel { Search_Address = "Russi" });
            //Points.Add(new LocationViewModel { Search_Address = "Modigliana" });
            //Points.Add(new LocationViewModel { Search_Address = "Lugo" });
            //Points.Add(new LocationViewModel { Search_Address = "York" });

            for (int i = 0; i < 2; i++)
            {
                Points.Add(new LocationViewModel {  });
            }

            //Validate_All();
        }

        protected void Find_Address(Location_Reference location)
        {
            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.ReverseGeocodeCompleted += (sender, e) =>
            {
                var correlated = e.UserState as Location_Reference;
                if (e.Result.Results.Count == 1)
                    correlated.Resolve_Address(e.Result.Results.Select(s=> s.Address).First());
                    //Correct_Position(correlated, e.Result.Results.Single());
                else
                    correlated.Invalidate();
            };
            geocoding.ReverseGeocodeAsync(new maps.GeocodeService.ReverseGeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Location = location.Location
            }, location);
        }

        public void F2_Destination(LocationViewModel sender, bool silent, string text)
        {
            Action onNoResults = () =>
            {
                sender.Remove_Destination();
            };
            Action onCancel = () =>
            {
            };
            Action<DestinationDto> onOneResult = (destination) =>
            {
                sender.Change_Destination(destination.ExternalId, destination.Name, new Location(destination.Latitude, destination.Longitude));
                
                Find_Address(sender.Destination);
            };
            if ((silent && string.IsNullOrWhiteSpace(text)))
            {
                onNoResults();
                return;
            }

            var helper = new Search_Destination_Helper();
            helper.ViewModel.Name = text;
            helper.Validate(this, onOneResult, onNoResults, onCancel, silent);
        }




        public void Carica()
        {
            var service = new ScenarioClient();
            service.Get_ScenarioCompleted += (sender, e) =>
            {
            };
            service.Get_ScenarioAsync("123");
        }
        public void Importa()
        {
            Calcola_Percorsi_Intermedi().ContinueWith(t => 
            {
                var service = new ScenarioClient();
                service.Create_ScenarioCompleted += (sender, e) =>
                {
                    //TODO 
                };
                var command = new Create_Scenario
                {
                    Scenario = new ScenarioDto
                    {
                        Id = Id,
                        Date = DateTime.Now,
                        Name = "asd",
                        UserId = RoutingViewModel.Instance.UserId,

                        Orders = Points.Select(p => new OrderDto
                        {
                            ExternalId = p.ExternalReference,
                            Delivering = p.Shipping,

                            DestinationId = p.Destination.Id,
                            DestinationExternalId = p.Destination.Id,

                            Latitude = p.Destination.Location.Latitude,
                            Longitude = p.Destination.Location.Longitude,
                            Volume = p.Amount.Value,
                            Volume_Unit = p.Amount.Unit,
                            Description = "nessuna ",

                            Address = p.Destination.Resolved_Address.FormattedAddress
                        }) .To_ObservableCollection(),

                        Distances = t.Result.Select(d => new DistanceDto 
                        { 
                            From_Latitide= d.From.Latitude, 
                            From_Longitude = d.From.Longitude, 
                            To_Latitide = d.To.Latitude, 
                            To_Longitude = d.To.Longitude,

                            Km= d.Km,
                            TimeInSeconds = d.TimeInSeconds
                        }).To_ObservableCollection(),
                    }
                };

                service.Create_ScenarioAsync(command);
            });
           
        }

        public Task<IEnumerable<DistanzaStimata>> Calcola_Percorsi_Intermedi()
        {
            return Task.Factory.StartNew(() =>
            {
                var tasks = new List<Task<DistanzaStimata>>();

                var punti = Points.Select(p => p.Destination.Location).Distinct(new Location_Comparer()).ToList();

                foreach (var combinazione in new Utilities.Combinatorics.Combinations<Location>(punti, 2))
                {
                    var da = combinazione[0];
                    var a = combinazione[1];
                    var task = Calcola_Distanza(da, a);
                    tasks.Add(task);
                    task.Start();
                }

                Task.WaitAll(tasks.ToArray());
                return tasks.Select(t => t.Result);
            });
        }

        public Task<DistanzaStimata> Calcola_Distanza(Location da, Location a)
        {
            var points = new[] { da, a }.Select(l => new maps.RouteService.Waypoint 
            { 
                Location = new maps.RouteService.Location { Latitude = l.Latitude, Longitude = l.Longitude }
            }).To_ObservableCollection();

            var tcs = new TaskCompletionSource<DistanzaStimata>();
            
            var route = maps.ServiceHelper.GetRouteService();

            route.CalculateRouteCompleted += (sender, e) => 
            {
                var d = e.UserState as DistanzaStimata;
                if (e.Error != null || d == null)
                    tcs.TrySetException(e.Error);

                d.Km = e.Result.Result.Summary.Distance;
                d.TimeInSeconds = e.Result.Result.Summary.TimeInSeconds;
                
                tcs.TrySetResult(d);
            };

            var request = new maps.RouteService.RouteRequest
            {
                Credentials = new maps.RouteService.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Options = new maps.RouteService.RouteOptions { Optimization = maps.RouteService.RouteOptimization.MinimizeDistance },
                Waypoints = points
            };

            route.CalculateRouteAsync(request, new DistanzaStimata { From= da, To = a });
  
            return tcs.Task;
        }

    }

    public class DistanzaStimata
    {
        public Location From { get; set; }
        public Location To { get; set; }

        public double Km { get; set; }
        public long TimeInSeconds { get; set; }
    }
   
}
