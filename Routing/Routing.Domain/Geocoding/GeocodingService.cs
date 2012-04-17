using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Geocoding
{
    public abstract class GeocodingService
    {
        public virtual GeocodeResult Geocode(string searchQuery)
        {
            return null;
        }

        public virtual ReverseGeocodeResult ReverseGeocode(Location location)
        {
            return null;
        }

        public virtual RoutingResult Routing(IEnumerable<Location> locations)
        {
            return null;
        }
    }

    public class GeocodeResult
    {
        public IEnumerable<Place> Places { get; set; }
    }

    public class Place
    {
        public Location Location { get; set; }
        public Address Address { get; set; }
    }

    public class ReverseGeocodeResult
    {
        public Address Address { get; set; }
    }

    public class RoutingResult
    {

    }

    
}
