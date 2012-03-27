using System;
using System.Net;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;


namespace Silverlight.Common.Helper
{
    //  http://blogs.msdn.com/b/silverlight_sdk/archive/2011/04/25/binding-to-dynamic-properties-with-icustomtypeprovider-silverlight-5-beta.aspx
    //  http://blogs.msdn.com/b/eternalcoding/archive/2011/04/26/dynamic-properties-in-silverlight-5.aspx
    //  http://msdn.microsoft.com/en-us/magazine/ff798279.aspx

    public class CustomTypeHelper<T> : ICustomTypeProvider, INotifyPropertyChanged
    {
        private static List<CustomPropertyInfoHelper> _customProperties = new List<CustomPropertyInfoHelper>();
        private Dictionary<string, object> _customPropertyValues;
        private CustomType _ctype;
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public CustomTypeHelper()
        {
            _customPropertyValues = new Dictionary<string, object>();
            foreach (var property in this.GetCustomType().GetProperties())
            {
                _customPropertyValues.Add(property.Name, null);
            }
        }

        public static void AddProperty(string name)
        {
            if (!CheckIfNameExists(name))
                _customProperties.Add(new CustomPropertyInfoHelper(name, typeof(String)));
        }

        public static void AddProperty(string name, Type propertyType)
        {
            if (!CheckIfNameExists(name))
                _customProperties.Add(new CustomPropertyInfoHelper(name, propertyType));
        }

        public static void AddProperty(string name, Type propertyType, List<Attribute> attributes)
        {
            if (!CheckIfNameExists(name))
                _customProperties.Add(new CustomPropertyInfoHelper(name, propertyType, attributes));
        }

        private static bool CheckIfNameExists(string name)
        {
            if ((from p in _customProperties select p.Name).Contains(name) || (from p in typeof(T).GetProperties() select p.Name).Contains(name))
                throw new Exception("The property with this name already exists: " + name);
            else return false;
        }

        public void SetPropertyValue(string propertyName, object value)
        {
            CustomPropertyInfoHelper propertyInfo = (from prop in _customProperties where prop.Name == propertyName select prop).FirstOrDefault();
            if (!_customPropertyValues.ContainsKey(propertyName))
                throw new Exception("There is no property " + propertyName);
            if (ValidateValueType(value, propertyInfo._type))
            {
                if (_customPropertyValues[propertyName] != value)
                {
                    _customPropertyValues[propertyName] = value;
                    NotifyPropertyChanged(propertyName);
                }
            }
            else throw new Exception("Value is of the wrong type or null for a non-nullable type.");
        }

        private bool ValidateValueType(object value, Type type)
        {
            if (value == null)
                // Non-value types can be assigned null.
                if (!type.IsValueType)
                    return true;
                else
                    // Check if the type if a Nullable type.
                    return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
            else
                return type.IsAssignableFrom(value.GetType());
        }

        public object GetPropertyValue(string propertyName)
        {
            if (_customPropertyValues.ContainsKey(propertyName))
                return _customPropertyValues[propertyName];
            else
                throw new Exception("There is no property " + propertyName);
        }

        public PropertyInfo[] GetProperties()
        {
            return this.GetCustomType().GetProperties();
        }

        public Type GetCustomType()
        {
            if (_ctype == null)
            {
                _ctype = new CustomType(typeof(T));
            }
            return _ctype;
        }

        private class CustomType : Type
        {
            Type _baseType;
            public CustomType(Type delegatingType)
            {
                _baseType = delegatingType;
            }
            public override Assembly Assembly
            {
                get { return _baseType.Assembly; }
            }

            public override string AssemblyQualifiedName
            {
                get { return _baseType.AssemblyQualifiedName; }
            }

            public override Type BaseType
            {
                get { return _baseType.BaseType; }
            }

            public override string FullName
            {
                get { return _baseType.FullName; }
            }

            public override Guid GUID
            {
                get { return _baseType.GUID; }
            }

            protected override TypeAttributes GetAttributeFlagsImpl()
            {
                throw new NotImplementedException();
            }

            protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
            {

                throw new NotImplementedException();
            }

            public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
            {
                return _baseType.GetConstructors(bindingAttr);
            }

            public override Type GetElementType()
            {
                return _baseType.GetElementType();
            }

            public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
            {
                return _baseType.GetEvent(name, bindingAttr);
            }

            public override EventInfo[] GetEvents(BindingFlags bindingAttr)
            {
                return _baseType.GetEvents(bindingAttr);
            }

            public override FieldInfo GetField(string name, BindingFlags bindingAttr)
            {
                return _baseType.GetField(name, bindingAttr);
            }

            public override FieldInfo[] GetFields(BindingFlags bindingAttr)
            {
                return _baseType.GetFields(bindingAttr);
            }

            public override Type GetInterface(string name, bool ignoreCase)
            {
                return _baseType.GetInterface(name, ignoreCase);
            }

            public override Type[] GetInterfaces()
            {
                return _baseType.GetInterfaces();
            }

            public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
            {
                return _baseType.GetMembers(bindingAttr);
            }

            protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
            {
                throw new NotImplementedException();
            }

            public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
            {
                return _baseType.GetMethods(bindingAttr);
            }

            public override Type GetNestedType(string name, BindingFlags bindingAttr)
            {
                return _baseType.GetNestedType(name, bindingAttr);
            }

            public override Type[] GetNestedTypes(BindingFlags bindingAttr)
            {
                return _baseType.GetNestedTypes(bindingAttr);
            }

            public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
            {
                PropertyInfo[] clrProperties = _baseType.GetProperties(bindingAttr);
                if (clrProperties != null)
                {
                    return clrProperties.Concat(_customProperties).ToArray();
                }
                else
                    return _customProperties.ToArray();
            }

            protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
            {
                // Look for the CLR property with this name first.
                PropertyInfo propertyInfo = (from prop in GetProperties(bindingAttr) where prop.Name == name select prop).FirstOrDefault();
                if (propertyInfo == null)
                {
                    // If the CLR property was not found, return a custom property
                    propertyInfo = (from prop in _customProperties where prop.Name == name select prop).FirstOrDefault();
                }
                return propertyInfo;
            }

            protected override bool HasElementTypeImpl()
            {
                throw new NotImplementedException();
            }

            public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, System.Globalization.CultureInfo culture, string[] namedParameters)
            {
                return _baseType.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
            }

            protected override bool IsArrayImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsByRefImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsCOMObjectImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsPointerImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsPrimitiveImpl()
            {
                return _baseType.IsPrimitive;
            }

            public override Module Module
            {
                get { return _baseType.Module; }
            }

            public override string Namespace
            {
                get { return _baseType.Namespace; }
            }

            public override Type UnderlyingSystemType
            {
                get { return _baseType.UnderlyingSystemType; }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return _baseType.GetCustomAttributes(attributeType, inherit);
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return _baseType.GetCustomAttributes(inherit);
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                return _baseType.IsDefined(attributeType, inherit);
            }

            public override string Name
            {
                get { return _baseType.Name; }
            }
        }

        // Custom implementation of the PropertyInfo
        private class CustomPropertyInfoHelper : PropertyInfo
        {
            public string _name;
            public Type _type;
            public List<Attribute> _attributes = new List<Attribute>();


            public CustomPropertyInfoHelper(string name, Type type)
            {
                _name = name;
                _type = type;
            }

            public CustomPropertyInfoHelper(string name, Type type, List<Attribute> attributes)
            {
                _name = name;
                _type = type;
                _attributes = attributes;
            }

            public override PropertyAttributes Attributes
            {
                get { throw new NotImplementedException(); }
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override MethodInfo[] GetAccessors(bool nonPublic)
            {
                throw new NotImplementedException();
            }

            public override MethodInfo GetGetMethod(bool nonPublic)
            {
                throw new NotImplementedException();
            }

            public override ParameterInfo[] GetIndexParameters()
            {
                throw new NotImplementedException();
            }

            public override MethodInfo GetSetMethod(bool nonPublic)
            {
                throw new NotImplementedException();
            }

            // Returns the value from the dictionary stored in the Customer's instance.
            public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
            {

                return obj.GetType().GetMethod("GetPropertyValue").Invoke(obj, new[] { _name });
            }

            public override Type PropertyType
            {
                get { return _type; }
            }

            // Sets the value in the dictionary stored in the Customer's instance.
            public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
            {
                obj.GetType().GetMethod("SetPropertyValue").Invoke(obj, new[] { _name, value });
            }

            public override Type DeclaringType
            {
                get { throw new NotImplementedException(); }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                var attrs = from a in _attributes where a.GetType() == attributeType select a;
                return attrs.ToArray();
            }

            public override object[] GetCustomAttributes(bool inherit)
            {
                return _attributes.ToArray();
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override string Name
            {
                get { return _name; }
            }

            public override Type ReflectedType
            {
                get { throw new NotImplementedException(); }
            }

            internal List<Attribute> CustomAttributesInternal
            {
                get { return _attributes; }
            }
        }
    }
}
