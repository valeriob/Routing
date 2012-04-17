// JSON C# Class Generator
// http://at-my-window.blogspot.com/?page=json-class-generator

using System;
using Newtonsoft.Json.Linq;
using JsonCSharpClassGenerator;

namespace Routing.Maps.Google.Geocode
{

    public class AddressComponent
    {

        private JObject __jobject;
        public AddressComponent(JObject obj)
        {
            this.__jobject = obj;
        }

        public string LongName
        {
            get
            {
                return JsonClassHelper.ReadString(JsonClassHelper.GetJToken<JValue>(__jobject, "long_name"));
            }
        }

        public string ShortName
        {
            get
            {
                return JsonClassHelper.ReadString(JsonClassHelper.GetJToken<JValue>(__jobject, "short_name"));
            }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string[] _types;
        public string[] Types
        {
            get
            {
                if(_types == null)
                    _types = (string[])JsonClassHelper.ReadArray<string>(JsonClassHelper.GetJToken<JArray>(__jobject, "types"), JsonClassHelper.ReadString, typeof(string[]));
                return _types;
            }
        }

    }
}
