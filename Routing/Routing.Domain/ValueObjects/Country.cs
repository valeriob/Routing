using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.ValueObjects
{
    public class Country : IValueObject
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            var other = obj as Country;
            return other != null && other.Name == Name;
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode();
        }
    }
}
