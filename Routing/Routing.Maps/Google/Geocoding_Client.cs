using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Hammock;

namespace Routing.Maps.Google
{
    public class Geocoding_Client
    {
        public static string Base = @"http://maps.googleapis.com/maps/api/geocode/json?";

        public GeocodeResult Geocode(string searchAddress)
        {
            var client = new RestClient();
            var query = client.GetQueryFor(new RestRequest(), Base);
           
            return null;
        }
    }

    //public class GeocodeResult
    //{
    //    public Status status { get; set; }

    //}

    //public class Result
    //{
    //    public IEnumerable<Address_Component> address_components { get; set; }

    //    public string formatted_address { get; set; }

    //}

    //public class Address_Component
    //{
    //    public string long_name { get; set; }
    //    public string short_name { get; set; }
    //    public IEnumerable<Types> types { get; set; }
    //}

    //public enum Types{}
    //public enum Status { OK}

    //public class Geometry 
    //{ 
    
    //}
}
