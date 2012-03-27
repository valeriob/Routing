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

            Validate_All();

            MessageBus.Current.Listen<Validation_Canceled>().Subscribe(s =>
            {
                validator.Dispose();
            });
            MessageBus.Current.Listen<Address_Validated>().Subscribe(s =>
            {
                validator.Dispose();
                Validating_Location.Location = s.Location;
                Validating_Location.Resolved_Address = s.Address;
                Validating_Location.Search_Address = s.Address.FormattedAddress;
                Validating_Location.Validate();
            });
        }

        public void Validate_All()
        {
            foreach (var point in Points)
            {
                if (string.IsNullOrEmpty(point.Destination.Search_Address))
                    continue;

                Find_Location(point.Destination);
            }
        }


        protected void Find_Location(Location_Reference point)
        {
            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.GeocodeCompleted += (sender, e) =>
            {
                var correlated = e.UserState as Location_Reference;

                if (e.Result.Results.Count == 1)
                    Correct_Position(correlated, e.Result.Results.Single());
            };

            geocoding.GeocodeAsync(new maps.GeocodeService.GeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Query = point.Search_Address
            }, point);
        }

        protected void Find_Address(Location_Reference location)
        {
            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.ReverseGeocodeCompleted += (sender, e) => 
            {
                var correlated = e.UserState as Location_Reference;
                if (e.Result.Results.Count == 1)
                    Correct_Position(correlated, e.Result.Results.Single());
                else
                    correlated.Invalidate();
            };
            geocoding.ReverseGeocodeAsync(new maps.GeocodeService.ReverseGeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Location= location.Location
            }, location);
        }

        protected void Correct_Position(Location_Reference point, GeocodeResult result)
        {
            point.Location = result.Locations.First();
            point.Search_Address = result.Address.FormattedAddress;
            point.Resolved_Address = result.Address;
            point.Validate();
        }


        public void F2_Destination(LocationViewModel sender, bool silent, string text)
        {
            Action onNoResults = () =>
            {
                sender.Destination = new Location_Reference();
            };
            Action onCancel = () =>
            {
            };
            Action<DestinationDto> onOneResult = (destination) =>
            {
                sender.Destination.Name = destination.Name;
                sender.Destination.Id = destination.Id;
                sender.Destination.ExternalId = destination.ExternalId;
                //sender.Destination.Validate();
                sender.Destination.Location = new Location(destination.Latitude, destination.Longitude);
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


        public Location_Reference Validating_Location { get; set; }
        Address_Validation_Helper validator;
        public void F2_Location(LocationViewModel sender, Panel panel, bool silent, string text)
        {
           
            Validating_Location = sender.Destination;
            validator = new Address_Validation_Helper(panel);
            validator.Validate_Address(silent, text).ContinueWith(c => 
            {
                if (c.IsCanceled)
                    validator.Manually_Validate_Address(text).ContinueWith(v => { Validate(v.Result); });
                else
                    Validate(c.Result);
            });
        }

        protected void Validate(Address_Validated result)
        {
            System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Validating_Location.Location = result.Location;
                Validating_Location.Resolved_Address = result.Address;
                Validating_Location.Search_Address = result.Address.FormattedAddress;
                Validating_Location.Validate();
            });
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
            var service = new ScenarioClient();
            service.Create_ScenarioCompleted += (sender, e) => 
            { 
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
                        DestinationExternalId = p.Destination.ExternalId,

                        Latitude = p.Destination.Location.Latitude,
                        Longitude = p.Destination.Location.Longitude, 
                        Volume = p.Amount.Value, 
                        Volume_Unit = p.Amount.Unit,
                        Description="nessuna ", 
                    
                        Address = p.Destination.Resolved_Address.FormattedAddress
                    })
                    .To_ObservableCollection()
                }
            };
            service.Create_ScenarioAsync(command);
        }
    }




   
}
