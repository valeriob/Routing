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


        public static bool operator ==(Location c1, Location c2)
        {
            return !object.ReferenceEquals(c1, null) && !object.ReferenceEquals(c2, null) && c1.Equals(c2);
        }

        public static bool operator !=(Location c1, Location c2)
        {
            return object.ReferenceEquals(c1, null) || object.ReferenceEquals(c2, null) || !c1.Equals(c2);
        }


        public override bool Equals(object obj)
        {
            var other = obj as Location;
            return !object.ReferenceEquals(other, null) && other.Latitude == Latitude && other.Longitude == Longitude && other.Approximation == Approximation;
        }

        public override int GetHashCode()
        {
            return 37 * Latitude.GetHashCode() + 17 * Longitude.GetHashCode() + 31 * Approximation.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Lat : {0}, Lon : {1}", Latitude, Longitude);
        }

    }

    
}
