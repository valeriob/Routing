using System;
using System.Net;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace Silverlight.Common.Helpers
{
    public class EnumHelper
    {
        public static int GetIntFromEnum<TEnum>(TEnum enumValue)
            where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException(typeof(TEnum).Name + " is not an Enum", "enumValue");

            var enumField = typeof(TEnum).GetFields().SingleOrDefault(f => f.GetValue(enumValue).Equals(enumValue));

            EnumMemberAttribute attribute = EnumMemberAttribute.GetCustomAttribute(enumField, typeof(EnumMemberAttribute), true) as EnumMemberAttribute;
            //var attribute = enumValue.GetType().GetCustomAttributes(typeof(EnumMemberAttribute), false).Cast<EnumMemberAttribute>().FirstOrDefault();
            int intValue;
            if (int.TryParse(attribute.Value, out intValue))
                return intValue;
            else
                throw new Exception("Could not convert enum to int");
        }

        // TODO: Fare cache della traduzione.
        public static TEnum GetEnumFromInt<TEnum>(int intValue)
            where TEnum : struct
        {
            var values = new List<Tuple<int, TEnum>>();


            foreach (TEnum value in EnumHelper.GetValues(typeof(TEnum)))
            {
                values.Add(Tuple.Create(GetIntFromEnum(value), value));
            }

            return (TEnum)values.FirstOrDefault(t => t.Item1 == intValue).Item2;
        }

        public static T[] GetValues<T>()
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<T> values = new List<T>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add((T)value);
            }

            return values.ToArray();
        }


        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();
        }
    }
}
