using System;
using System.Net;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;
using System.Collections.ObjectModel;
using maps = Silverlight.Common.Maps;
using System.Collections.Generic;
using System.Diagnostics;


namespace Routing.Silverlight.Address_Validation
{
    public class Search_LocationViewModel : ReactiveObject
    {
        private Location _Location;
        public Location Location
        {
            get { return _Location; }
            set { _Location = value; this.RaisePropertyChanged(v => v.Location); }
        }

        private string _Search_Address;
        public string Search_Address
        {
            get { return _Search_Address; }
            set { _Search_Address = value; this.RaisePropertyChanged(v => v.Search_Address); }
        }

        private Address _Resolved_Address;
        public Address Resolved_Address
        {
            get { return _Resolved_Address; }
            set { _Resolved_Address = value; this.RaisePropertyChanged(v => v.Resolved_Address); }
        }

        private ObservableCollection<GeocodeResult> _GeocodeResult;
        public ObservableCollection<GeocodeResult> GeocodeResult
        {
            get { return _GeocodeResult ?? (_GeocodeResult = new ObservableCollection<GeocodeResult>()); }
            set { _GeocodeResult = value; this.RaisePropertyChanged(v => v.GeocodeResult); }
        }

        private GeocodeResult _SelectedResult;
        public GeocodeResult SelectedResult
        {
            get { return _SelectedResult; }
            set { _SelectedResult = value; this.RaisePropertyChanged(v => v.SelectedResult); }
        }

        private bool _HasResults;
        public bool HasResults
        {
            get { return _HasResults; }
            set { _HasResults = value;  this.RaisePropertyChanged(v => v.HasResults);}
        }

        private bool _PositionConfirmed;
        public bool PositionConfirmed
        {
            get { return _PositionConfirmed; }
            set { _PositionConfirmed = value; this.RaisePropertyChanged(v => v.PositionConfirmed); }
        }
        


        public Search_LocationViewModel()
        {
            this.ObservableForProperty(p => p.SelectedResult).Value()
                .Subscribe(Select_Result);
            this.ObservableForProperty(p => p.Search_Address)
               .Subscribe(s => Search());

            //this.ObservableForProperty(p => p.Location).Window(TimeSpan.FromSeconds(1),1)
            //    .Buffer(TimeSpan.FromMilliseconds(1000))
            //    .Subscribe(s=> {
            //        Debug.WriteLine(s.Count);
            //    });

            MessageBus.Current.Listen<Manually_Validate_Address>().Subscribe(Manually_Validate_Address);
        }
        protected void Find_Address(Location location)
        {
            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.ReverseGeocodeCompleted += (sender, e) =>
            {
                var correlated = e.UserState as Location;
                GeocodeResult.Clear();
                GeocodeResult.AddRange(e.Result.Results);
                SelectedResult = GeocodeResult.FirstOrDefault();
            };
            geocoding.ReverseGeocodeAsync(new maps.GeocodeService.ReverseGeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Location = location
            }, location);
        }

        Manually_Validate_Address Serving;
        protected void Manually_Validate_Address(Manually_Validate_Address toBeValidated)
        {
            if (!toBeValidated.IsValid())
                return;

            Serving = toBeValidated;
            Initialize(Serving.Search_Address);
        }

        public void Initialize(string searchAddress)
        {
            Search_Address = searchAddress;
        }



        public void Confirm_Position()
        {
            PositionConfirmed = true;
            if (Serving != null && SelectedResult != null)
                MessageBus.Current.SendMessage(new Address_Validated(SelectedResult, Serving));
        }

        public void Cancel()
        {
            PositionConfirmed = false;
            if (Serving != null)
                MessageBus.Current.SendMessage(new Validation_Canceled(Serving));
        }

        protected void Sanitize_Parameters()
        {
            if (Location == null && Resolved_Address == null && !string.IsNullOrEmpty(Search_Address))
            {
                Search();
            }
            if (Location != null)
            { 
            }
        }

        protected void Verify_Address_For_Location()
        {
            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.ReverseGeocodeCompleted += (sender, e) =>
            {
                SelectedResult = e.Result.Results.SingleOrDefault(); ;
            };

            geocoding.ReverseGeocodeAsync(new maps.GeocodeService.ReverseGeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials },
                Location = Location
            });
        }

        protected void Select_Result(GeocodeResult result)
        {
            if (result == null)
            {
                Resolved_Address = null;
                Location = null;
                return;
            }
            Resolved_Address = result.Address;
            //! piu di una?
            Location = result.Locations.FirstOrDefault();
        }

        protected void Search()
        {
            if(string.IsNullOrEmpty(Search_Address))
                return;

            var geocoding = maps.ServiceHelper.GetGeocodeService();
            geocoding.GeocodeCompleted += (sender, e) =>
            {
                GeocodeResult.Clear();
                foreach (var item in e.Result.Results)
                    GeocodeResult.Add(item);
                if (e.Result.Results.Count == 1)
                    SelectedResult = e.Result.Results.First();

                HasResults = GeocodeResult.Any();
            };

            geocoding.GeocodeAsync(new maps.GeocodeService.GeocodeRequest
            {
                Credentials = new Microsoft.Maps.MapControl.Credentials() { Token = maps.ServiceHelper.GeocodeServiceCredentials }, 
                Culture = System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(),
                Query = Search_Address
            });
        }

        protected void Clear()
        {
            Resolved_Address = null;
            Location = null;
            GeocodeResult.Clear();
        }

    


    }

 

  
    
}
