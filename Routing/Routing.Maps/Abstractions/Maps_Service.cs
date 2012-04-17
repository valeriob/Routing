using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Routing.Maps
{
    public class Maps_Service
    {
        


        public Maps_Service()
        {
            
        }


        public IEnumerable<Located_Address> Search_Places(string query)
        {
            return null;

        }


        public Located_Address Reverse_Geocode(Location location)
        {
            return null;
        }

        public Route Find_Route(Location from, Location to, object optimizeFor)
        {
            return null;
        }
    }



    public interface Map_Account_Provider
    {
        IEnumerable<Located_Address> Search_Places(string query);
        Located_Address Reverse_Geocode(Location location);
        Route Find_Route(Location from, Location to, object optimizeFor);

        int Searches { get; set; }
        int Reverse { get; set; }
    }

    public class Bing_Provider
    {

    }

    public class Google_Provider
    {

    }


    public class Located_Address : Address
    {
        public Location Location { get; set; }

    }

    public class Route
    {
        public IEnumerable<Leg> Legs { get; set; }
    }

    public class Leg
    {
        public Location From { get; set; }
        public Location To { get; set; }

        public double Km { get; set; }
        public TimeSpan Time { get; set; }
    }
}
