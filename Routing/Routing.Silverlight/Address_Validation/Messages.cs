using System;
using System.Net;
using System.Linq;
using Microsoft.Maps.MapControl;
using Silverlight.Common.Maps.GeocodeService;

namespace Routing.Silverlight.Address_Validation
{
    public abstract class Validate_Address
    {
        public Guid RequestId { get; protected set; }
        public string Search_Address { get; set; }
        public object UserState { get; protected set; }

        public Validate_Address()
        {
            RequestId = Guid.NewGuid();
        }
        public Validate_Address(object userState) :this()
        {
            UserState = userState;
        }

        protected Validate_Address(Guid id) { }


        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Search_Address);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Validate_Address;
            return other != null && RequestId == other.RequestId;
        }

        public override int GetHashCode()
        {
            return RequestId.GetHashCode();
        }

        public override string ToString()
        {
            return RequestId + " - Searching : " +Search_Address;
        }
    }

    public class Automatically_Validate_Address : Validate_Address
    {
        public Automatically_Validate_Address() { }
        public Automatically_Validate_Address(object userState) : base(userState) { }
        public Automatically_Validate_Address(Guid id) : base(id) { }

        public Manually_Validate_Address To_Manual()
        {
            var manual = new Manually_Validate_Address(RequestId);
            manual.Search_Address = Search_Address;
            return manual;
        }


    }

    public class Manually_Validate_Address : Validate_Address
    {
        public Manually_Validate_Address() { }
        public Manually_Validate_Address(object userState) : base(userState) { }
        public Manually_Validate_Address(Guid id) : base(id) { }
    }


    public class Validation_Canceled
    {
        public Validate_Address Related_To_Validate_Address { get; protected set; }

        public Validation_Canceled(Validate_Address from)
        {
            Related_To_Validate_Address = from;
        }
    }


    public class Address_Validated
    {
        public Validate_Address Related_To_Validate_Address { get; protected set; }

        public Location Location { get; protected set; }
        public Address Address { get; protected set; }

        public GeocodeResult GeocodeResult { get; protected set; }

        public Address_Validated(GeocodeResult result, Validate_Address from)
        {
            Related_To_Validate_Address = from;

            GeocodeResult = result;
            Location = result.Locations.FirstOrDefault();
            Address = result.Address;
        }
    }
}
