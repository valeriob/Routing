using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace Routing.Web.Controllers
{
    public class MapApiController : ApiController
    {

        public IEnumerable<string> Search_Places(string query)
        {
            return null;
        }


        public string Reverse_Geocode(double latitude, double longitute)
        {
            return null;
        }

        public string Find_Route(object from, object to, object optimizeFor)
        {
            return null;
        }


    }
}
