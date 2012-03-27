using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Resources;

namespace Silverlight.Common.Helpers
{
    public class EnumWrapper<TEnum> where TEnum : struct
    {
        public TEnum? EnumValue { get; set; }

        public string Display { get; set; }

        public static IEnumerable<EnumWrapper<TEnum>> GetCollection(Type resourceType = null)
        {
            ResourceManager resourceManager = null;
            if (resourceType != null)
                resourceManager = new ResourceManager(resourceType);

            var enumValues = EnumHelper.GetValues(typeof(TEnum)).Cast<TEnum>();

            List<EnumWrapper<TEnum>> values = new List<EnumWrapper<TEnum>>();
            values.Add(new EnumWrapper<TEnum>() { Display = " " });

            foreach (var value in enumValues)
            {
                string display = value + "";
                if (resourceManager != null)
                    display = resourceManager.GetString(display);
                values.Add(new EnumWrapper<TEnum>() { EnumValue = value, Display = display });
            }
            return values;
        }

        public override string ToString()
        {
            return Display;
        }
    }
}
