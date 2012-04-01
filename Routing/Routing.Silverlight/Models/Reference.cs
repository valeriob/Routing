using System;
using System.Net;
using System.Linq;
using ReactiveUI;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;
using Routing.Silverlight.Address_Validation;
using System.Collections.Generic;

namespace Routing.Silverlight.Models
{
    public class Reference : ReactiveObject
    {
        private string _Id;
        public string Id
        {
            get { return _Id; }
            protected set { _Id = value; this.RaisePropertyChanged(v => v.Id); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            protected set { _Name = value; this.RaisePropertyChanged(v => v.Name); }
        }

        private bool _IsValid;
        public bool IsValid
        {
            get { return _IsValid; }
            protected set { _IsValid = value; this.RaisePropertyChanged(v => v.IsValid);}
        }
        

        public virtual void Validate()
        {
            IsValid = true;
        }

        public virtual void Invalidate()
        {
            IsValid = false;
        }

        private string _Display;
        public string Display
        {
            get { return _Display; }
            set { _Display = value; this.RaisePropertyChanged(a => a.Display);}
        }

        public Reference()
        {
            this.WhenAny(p => p.Id, q => q.Name, (a, b) => { return a; }).Subscribe(s =>
            {
                Display = ToString();
            });
        }

 

        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Name);
        }
    }


    public class Location_Reference : Reference
    {
        public readonly static Location_Reference Empty = new Location_Reference(new Location(), new Address());

        private Location _Location;
        public Location Location
        {
            get { return _Location; }
            protected set { _Location = value; this.RaisePropertyChanged(v => v.Location); }
        }

        //private string _Search_Address;
        //public string Search_Address
        //{
        //    get { return _Search_Address; }
        //    protected set { _Search_Address = value; this.RaisePropertyChanged(v => v.Search_Address); }
        //}

        //private string _ExternalId;
        //public string ExternalId
        //{
        //    get { return _ExternalId; }
        //    set { _ExternalId = value; this.RaisePropertyChanged(v => v.ExternalId);}
        //}
        

        //private string _Display_Address;
        //public string Display_Address
        //{
        //    get { return _Display_Address; }
        //    protected set { _Display_Address = value; this.RaisePropertyChanged(v => v.Display_Address); }
        //}

        private Address _Resolved_Address;
        public Address Resolved_Address
        {
            get { return _Resolved_Address; }
            protected set { _Resolved_Address = value; this.RaisePropertyChanged(v => v.Resolved_Address); }
        }

        //private string _ExternalId;
        //public string ExternalId
        //{
        //    get { return _ExternalId; }
        //    protected set { _ExternalId = value; this.RaisePropertyChanged(v => v.ExternalId);}
        //}
        

        //public override void Invalidate()
        //{
          
        //    Location = new Location();
        //    Resolved_Address = new Address();
        //}

        public Location_Reference(Location location)
        {
            Location = location;
        }

        public Location_Reference(Location location, Address address)
        {
            Location = location;
            Display = address.FormattedAddress;
            Resolved_Address = address;
        }

        public Location_Reference(string externalId, string name, Location location)
        {
            Id = externalId;
            Name = name;
            Location = location;
        }
        public Location_Reference(string externalId, string name, Location location, Address address)
        {
            Id = externalId;
            Name = name;
            Location = location;
            if(address!=null)
                Display = address.FormattedAddress;
            Resolved_Address = address;
        }

        //public Location_Reference()
        //{
        //    this.WhenAny(p => p.Id, q => q.Name, r=> r.Resolved_Address, (a, b,c) => { return a; }).Subscribe(s =>
        //    {
        //        Display = ToString();
        //    });
        //}

        public override string ToString()
        {
            //return string.Format("{0} - {1}{2}{3}", Id, Name, Environment.NewLine, Resolved_Address);
            return Display;
        }

        public void Resolve_Address(Address address)
        {
            Resolved_Address = address;
            Display = Resolved_Address.FormattedAddress;
        }

   
    }

    public class Location_Comparer : IEqualityComparer<Location>
    {
        //public override bool Equals(object obj)
        //{
        //    var other = obj as Location;
        //    return other != null && Equals(this, other);
        //}

        //public override int GetHashCode()
        //{
        //    return 3* 7* 
        //}

        public bool Equals(Location x, Location y)
        {
            return x.Altitude == y.Altitude && x.Longitude == y.Longitude && x.Latitude == y.Latitude;
        }

        public int GetHashCode(Location obj)
        {
            return 3 + 7 * obj.Altitude.GetHashCode() + 11 * obj.Latitude.GetHashCode() + 13 * obj.Longitude.GetHashCode();
        }
    }

}
