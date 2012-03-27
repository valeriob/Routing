using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.ValueObjects
{
    public class Quantity
    {
        public double Amount { get; protected set; }
        public Unit Unit { get; protected set; }

        public Quantity(double amount, Unit unit)
        {
            Amount = amount;
            Unit = unit;
        }
    }

   
}
