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
            set { _Destination = value; this.RaisePropertyChanged(v => v.Destination); }
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
                Destination = new Location_Reference
                {
                    Id = "client1",
                    Name = "Client 1",
                    Location = new Location(44, 23),
                    Search_Address = "boh",
                };
            else
                Destination = new Location_Reference();
            Amount = new Amount { Unit="KG", Value = 10 };
        }


   


    }
}
