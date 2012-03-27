using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Resources;
using System.Reflection;
using System.Collections.Generic;

namespace Silverlight.Common.Helpers
{
    public static class ReflectionHelper
    {
        public static List<Type> Types { get; set; }
        
        public static Type GetAssemblyType(string assemblyName, string className)
        {
            StreamResourceInfo info = Application.GetResourceStream(new Uri(assemblyName, UriKind.Relative));
            Assembly assembly = new AssemblyPart().Load(info.Stream);
            Type type = assembly.GetType(className);

            return type;
        }

        public static Type GetAssemblyType(string className)
        {
            Type type = null;
            foreach (AssemblyPart part in Deployment.Current.Parts)
            {
                type = GetAssemblyType(part.Source, className);
                if (type != null)
                    break;
            }
            return type;
        }

        public static IEnumerable<Type> GetTypes()
        {
            if (Types == null)
            {
                Types = new List<Type>();
                foreach (AssemblyPart part in Deployment.Current.Parts)
                {
                    StreamResourceInfo info = Application.GetResourceStream(new Uri(part.Source, UriKind.Relative));
                    Assembly assembly = new AssemblyPart().Load(info.Stream);
                    Types.AddRange(assembly.GetTypes());
                }
            }
            return Types;
        }

    }
}
