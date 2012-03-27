using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.ValueObjects
{
    public class Location : IValueObject
    {
        public readonly static Location Empty = new Location(0,0);

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Approximation { get; set; }

        public string Address { get; set; }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Location;
            return other != null && other.Latitude == Latitude && other.Longitude == Longitude && other.Approximation == Approximation;
        }

        public override int GetHashCode()
        {
            return 37 * Latitude.GetHashCode() + 17 * Longitude.GetHashCode() + 31 * Approximation.GetHashCode();
        }

    }

    
}
