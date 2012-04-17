// JSON C# Class Generator
// http://at-my-window.blogspot.com/?page=json-class-generator

using System;
using Newtonsoft.Json.Linq;
using JsonCSharpClassGenerator;

namespace Routing.Maps.Google.Geocode
{

    public class Southwest
    {

        private JObject __jobject;
        public Southwest(JObject obj)
        {
            this.__jobject = obj;
        }

        public double Lat
        {
            get
            {
                return JsonClassHelper.ReadFloat(JsonClassHelper.GetJToken<JValue>(__jobject, "lat"));
            }
        }

        public double Lng
        {
            get
            {
                return JsonClassHelper.ReadFloat(JsonClassHelper.GetJToken<JValue>(__jobject, "lng"));
            }
        }

    }
}
