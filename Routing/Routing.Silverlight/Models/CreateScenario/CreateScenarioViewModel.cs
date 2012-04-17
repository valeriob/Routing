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
using System.Diagnostics;
using System.Threading;


namespace Routing.Silverlight.Models.CreateScenario
{
    public class CreateScenarioViewModel : ReactiveObject
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

        
        
        
        public CreateScenarioViewModel()
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


        public void Test_Calcolo_Distanze()
        {
            var punti = Points.Select(p => p.Destination.Location).Distinct(new Location_Comparer()).ToList();

            punti.Clear();
            for (int i = 0; i < 100; i++)
                punti.Add(new Location { Latitude = 44 + 0.0001 * i, Longitude = 12 + 0.0001 * i });


            DateTime? start = null;
            var stop = start;
            var distanze = new List<DistanzaStimata>();
            var mre = new ManualResetEvent(false);
            var jobs = new Queue<DistanzaStimata>();

            var route = maps.ServiceHelper.GetRouteService();
            //route.CalculateRouteCompleted += (sender, e) =>
            //{
            //    var d = e.UserState as DistanzaStimata;
            //    if (start == null)
            //        start = DateTime.Now;

            //    if (e.Error == null && d != null)
            //    {
            //        d.Km = e.Result.Result.Summary.Distance;
            //        d.TimeInSeconds = e.Result.Result.Summary.TimeInSeconds;
            //    }
            //    lock (d)
            //        Monitor.Pulse(d);

            //    d.Done();
            //    if (distanze.All(a => a.Completed))
            //    {
            //        stop = DateTime.Now;
            //        mre.Set();
            //    }
            //};

  

            var combos = new Utilities.Combinatorics.Combinations<Location>(punti, 2);
            foreach (var combinazione in combos)
            {
                var da = combinazione[0];
                var a = combinazione[1];
                var distanza = new DistanzaStimata { From = da, To = a };
                distanze.Add(distanza);
                jobs.Enqueue(distanza);
            }


            //for (int i = 0; i < 10; i++)
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        while (jobs.Count > 0)
            //        {
            //            var distanza = jobs.Dequeue();

            //            var points = new[] { distanza.From, distanza.To }.Select(l => new maps.RouteService.Waypoint
            //            {
            //                Location = new maps.RouteService.Location { Latitude = l.Latitude, Longitude = l.Longitude }
            //            }).To_ObservableCollection();


            //            var request = new maps.RouteService.RouteRequest
            //            {
            //                Credentials = new maps.RouteService.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
            //                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
            //                Options = new maps.RouteService.RouteOptions { Optimization = maps.RouteService.RouteOptimization.MinimizeDistance },
            //                Waypoints = points
            //            };

            //            route.CalculateRouteAsync(request, distanza);

            //            lock (distanza)
            //                Monitor.Wait(distanza);

            //        }
            //    });
            //}

            var builder = new Route_Request_Builder(distanze);
            var requests = builder.Get_Requests().To_Queue();
            var test = builder.Get_Requests();
            var q1 = test.SelectMany(r => r.Get_Routes()).Where(r => !r.Completed);

            route.CalculateRouteCompleted += (sender, e) =>
            {
                var d = e.UserState as Request;
                if (start == null)
                    start = DateTime.Now;

                if (e.Error == null && d != null)
                {
                    for (int i = 0; i < e.Result.Result.Legs.Count; i++)
                    {
                        var leg = e.Result.Result.Legs[i];
                        var r = d.Get_Routes().ElementAt(i);
                        r.Km = leg.Summary.Distance;
                        r.TimeInSeconds = leg.Summary.TimeInSeconds;
                        r.Done();
                    }
                    d.Done();
                }
                lock (d)
                    Monitor.Pulse(d);

                if (test.All(a => a.Completed))
                {
                    stop = DateTime.Now;
                    mre.Set();
                }
            };

            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() => 
                {
                    while (requests.Count > 0)
                    {
                        var request = requests.Dequeue();

                        var points = request.Get_Locations().Select(l => new maps.RouteService.Waypoint 
                        {
                            Location = new maps.RouteService.Location { Latitude = l.Latitude, Longitude = l.Longitude }
                        }).To_ObservableCollection();


                        var routeRequest = new maps.RouteService.RouteRequest
                        {
                            Credentials = new maps.RouteService.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                            Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                            Options = new maps.RouteService.RouteOptions { Optimization = maps.RouteService.RouteOptimization.MinimizeDistance },
                            Waypoints = points
                        };

                        route.CalculateRouteAsync(routeRequest, request);

                        lock (request)
                            Monitor.Wait(request);

                    }
                });
            }
        }

        public Task<IEnumerable<DistanzaStimata>> Calcola_Percorsi_Intermedi()
        {
            return Task.Factory.StartNew(() =>
            {
                var tasks = new List<Task<DistanzaStimata>>();

                var punti = Points.Select(p => p.Destination.Location).Distinct(new Location_Comparer()).ToList();
                //    SelectedSimulation.Starting_Latitude =   44.1383781433105;
                // SelectedSimulation.Starting_Longitude =   12.2387800216675;
                
                for (int i = 0; i < 100; i++)
                {
                    punti.Add(new Location { Latitude = 44 + 0.0001*i, Longitude= 12 + 0.0001*i });
                }

                foreach (var combinazione in new Utilities.Combinatorics.Combinations<Location>(punti, 2))
                {
                    var da = combinazione[0];
                    var a = combinazione[1];
                    var task = Calcola_Distanza(da, a);
                    tasks.Add(task);
                    //task.Start();
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
                d.Done();

                if (e.Error != null || d == null)
                {
                    tcs.TrySetException(e.Error);
                    return;
                }
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

        public bool Completed { get; protected  set; }
        public void Done()
        {
            Completed = true;
            Debug.WriteLine(string.Format("{0} => {1}, {2} Km, {3}s", From, To, Km, TimeInSeconds));
        }

        public override string ToString()
        {
            return string.Format("From {0}, To {1}", From, To);
        }
    }
   
}
