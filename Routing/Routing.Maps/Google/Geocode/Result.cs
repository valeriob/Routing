// JSON C# Class Generator
// http://at-my-window.blogspot.com/?page=json-class-generator

using System;
using Newtonsoft.Json.Linq;
using JsonCSharpClassGenerator;

namespace Routing.Maps.Google.Geocode
{

    public class Result
    {

        private JObject __jobject;
        public Result(JObject obj)
        {
            this.__jobject = obj;
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private AddressComponent[] _addressComponents;
        public AddressComponent[] AddressComponents
        {
            get
            {
                if(_addressComponents == null)
                    _addressComponents = (AddressComponent[])JsonClassHelper.ReadArray<AddressComponent>(JsonClassHelper.GetJToken<JArray>(__jobject, "address_components"), JsonClassHelper.ReadStronglyTypedObject<AddressComponent>, typeof(AddressComponent[]));
                return _addressComponents;
            }
        }

        public string FormattedAddress
        {
            get
            {
                return JsonClassHelper.ReadString(JsonClassHelper.GetJToken<JValue>(__jobject, "formatted_address"));
            }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Geometry _geometry;
        public Geometry Geometry
        {
            get
            {
                if(_geometry == null)
                    _geometry = JsonClassHelper.ReadStronglyTypedObject<Geometry>(JsonClassHelper.GetJToken<JObject>(__jobject, "geometry"));
                return _geometry;
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
