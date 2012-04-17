// JSON C# Class Generator
// http://at-my-window.blogspot.com/?page=json-class-generator

using System;
using Newtonsoft.Json.Linq;
using JsonCSharpClassGenerator;
using Routing.Maps.Google.Geocode;

namespace Routing.Maps.Google
{

    public class GeocodeResult
    {

        public GeocodeResult(string json)
         : this(JObject.Parse(json))
        {
        }

        private JObject __jobject;
        public GeocodeResult(JObject obj)
        {
            this.__jobject = obj;
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Result[] _results;
        public Result[] Results
        {
            get
            {
                if(_results == null)
                    _results = (Result[])JsonClassHelper.ReadArray<Result>(JsonClassHelper.GetJToken<JArray>(__jobject, "results"), JsonClassHelper.ReadStronglyTypedObject<Result>, typeof(Result[]));
                return _results;
            }
        }

        public string Status
        {
            get
            {
                return JsonClassHelper.ReadString(JsonClassHelper.GetJToken<JValue>(__jobject, "status"));
            }
        }

    }
}
