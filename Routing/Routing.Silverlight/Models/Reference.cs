using System;
using System.Net;
using System.Linq;
using ReactiveUI;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;

namespace Routing.Silverlight.Models
{
    public class Reference : ReactiveObject
    {
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { _Id = value; this.RaisePropertyChanged(v => v.Id); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; this.RaisePropertyChanged(v => v.Name); }
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

        private string _ExternalId;
        public string ExternalId
        {
            get { return _ExternalId; }
            set { _ExternalId = value; this.RaisePropertyChanged(v => v.ExternalId);}
        }
        

        public override void Invalidate()
        {
            base.Invalidate();
            Location = new Location();
            Resolved_Address = new Address();
        }

        public Location_Reference()
        {
            this.WhenAny(p => p.Id, q => q.Name, r=> r.Resolved_Address, (a, b,c) => { return a; }).Subscribe(s =>
            {
                Display = ToString();
            });
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}{2}{3}", Id, Name, Environment.NewLine, Resolved_Address);
        }
    }

}
