using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class SystemExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
