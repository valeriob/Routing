using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Maps.Google
{
    public class GoogleMaps : Map_Account_Provider
    {
        public GoogleMaps()
        {

        }

        public IEnumerable<Located_Address> Search_Places(string query)
        {
            throw new NotImplementedException();
        }

        public Located_Address Reverse_Geocode(Location location)
        {
            throw new NotImplementedException();
        }

        public Route Find_Route(Location from, Location to, object optimizeFor)
        {
            throw new NotImplementedException();
        }

        public int Searches
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Reverse
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
