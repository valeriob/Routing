using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Maps
{
    public class Address 
    {
        public readonly static Address Empty = new Address();

        public string Street { get; set; }
        public string Street_Specific { get; set; }
        public string City { get; set; }
        public string State_Province_Region { get; set; }
        public string ZIP { get; set; }
        
        public string Country { get; set; }

        public static Address Parse(string formatted)
        {
            return new Address { Street = formatted };
        }

        public override bool Equals(object obj)
        {
            var other = obj as Address;

            return other != null && other.Street == Street && other.Street_Specific == Street_Specific &&
                other.City == City && other.State_Province_Region == State_Province_Region &&
                other.ZIP == ZIP && other.Country == Country;
        }

        public override int GetHashCode()
        {
            int hash = 37;

            if (Street != null)
                hash = hash + 17 * Street.GetHashCode();

            if (Street_Specific != null)
                hash = hash + 19 * Street_Specific.GetHashCode();
            if (City != null)
                hash = hash + 23 * City.GetHashCode();
            if (State_Province_Region != null)
                hash = hash + 27 * State_Province_Region.GetHashCode();

            if (ZIP != null)
                hash = hash + 31 * ZIP.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}"
                ,Street, Street_Specific, City, State_Province_Region, ZIP, Country);
        }
    }
}
