// JSON C# Class Generator
// http://at-my-window.blogspot.com/?page=json-class-generator

using System;
using Newtonsoft.Json.Linq;
using JsonCSharpClassGenerator;

namespace Routing.Maps.Google.Geocode
{

    public class Geometry
    {

        private JObject __jobject;
        public Geometry(JObject obj)
        {
            this.__jobject = obj;
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Bounds _bounds;
        public Bounds Bounds
        {
            get
            {
                if(_bounds == null)
                    _bounds = JsonClassHelper.ReadStronglyTypedObject<Bounds>(JsonClassHelper.GetJToken<JObject>(__jobject, "bounds"));
                return _bounds;
            }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Location _location;
        public Location Location
        {
            get
            {
                if(_location == null)
                    _location = JsonClassHelper.ReadStronglyTypedObject<Location>(JsonClassHelper.GetJToken<JObject>(__jobject, "location"));
                return _location;
            }
        }

        public string LocationType
        {
            get
            {
                return JsonClassHelper.ReadString(JsonClassHelper.GetJToken<JValue>(__jobject, "location_type"));
            }
        }

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Viewport _viewport;
        public Viewport Viewport
        {
            get
            {
                if(_viewport == null)
                    _viewport = JsonClassHelper.ReadStronglyTypedObject<Viewport>(JsonClassHelper.GetJToken<JObject>(__jobject, "viewport"));
                return _viewport;
            }
        }

    }
}
