using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Aggregates;

namespace Routing.Domain.ValueObjects
{
    public class Unit 
    {
        public string Name { get; protected set; }
        public string Description { get; set; }

        public Unit(string name)
        {
            Name = name;
        }

       
    }

    public class Volumes
    {
        public static Unit m3 { get; protected set;}
        static Volumes()
        {
            m3 = new Unit("m3");
        }
    }
}
