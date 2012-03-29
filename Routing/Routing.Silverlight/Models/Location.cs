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
using Routing.Silverlight.Address_Validation;


namespace Routing.Silverlight.Models
{
    public class LocationViewModel : ReactiveObject
    {
        private string _ExternalReference;
        public string ExternalReference
        {
            get { return _ExternalReference; }
            set { _ExternalReference = value; this.RaisePropertyChanged(v => v.ExternalReference); }
        }
        
        private DateTime _Shipping;
        public DateTime Shipping
        {
            get { return _Shipping; }
            set { _Shipping = value; this.RaisePropertyChanged(v => v.Shipping);}
        }
        

        private Location_Reference _Destination;
        public Location_Reference Destination
        {
            get { return _Destination; }
            protected set { _Destination = value; this.RaisePropertyChanged(v => v.Destination); }
        }

        private Amount _Amount;
        public Amount Amount
        {
            get { return _Amount; }
            set { _Amount = value; this.RaisePropertyChanged(v => v.Amount); }
        }
        


        static Random random = new Random(DateTime.Now.Millisecond);
        public LocationViewModel()
        {
            ExternalReference = Guid.NewGuid().ToString();
            Shipping = DateTime.Today.AddDays(1);

            if (random.Next(10) % 10 > 5)
                Destination = new Location_Reference("CLIENT1", "Cliente 1", new Location(44, 23));
            //Destination = new Location_Reference
            //{
            //    Id = "client1",
            //    Name = "Client 1",
            //    Location = new Location(44, 23),
            //    Search_Address = "boh",
            //};
            else
                Destination = Location_Reference.Empty;
            Amount = new Amount { Unit="KG", Value = 10 };
        }


        public void Remove_Destination()
        {
            Destination = null;
        }
        public void Change_Destination(Address_Validated result)
        {
            if (result == null)
                return;
            Destination = new Location_Reference(result.Location, result.Address);
            //Destination = new Location_Reference() 
            //{ 
            //    Location = result.Location,
            //    Resolved_Address = result.Address,
            //    Search_Address = result.Address.FormattedAddress,
            //};

            //Destination.Validate();

            //System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
            //{
            //    Location = result.Location;
            //    Resolved_Address = result.Address;
            //    Search_Address = result.Address.FormattedAddress;

            //    Validate();
            //});
        }

        public void Change_Destination(string externalId, string name, Location location, Address address)
        {
            Destination = new Location_Reference(externalId, name, location, address);
        }
        public void Change_Destination(string externalId, string name, Location location)
        {
            Destination = new Location_Reference(externalId, name, location);
        }
   


    }
}
