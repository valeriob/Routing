using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;


namespace Routing.Domain.Aggregates
{
    public class Scenario : INamedDocument
    {
        public Scenario()
        {
            _Simulations = new List<Simulation>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }

        protected DenormalizedReference<User> _Owner;
        public DenormalizedReference<User> Owner { get { return _Owner ?? (_Owner = User.Empty); } }

        public DateTime Created { get; set; }
        public DateTime? Deleted { get; protected set; }
        public DateTime Date { get; set; }

        protected List<Delivery> _Deliveries;
        public IEnumerable<Delivery> Deliveries { get { return _Deliveries ?? (_Deliveries = new List<Delivery>()); } set { _Deliveries = value as List<Delivery>; } }

        public IEnumerable<Distance> Distances { get; set; }


        protected List<Simulation> _Simulations;
        public IEnumerable<Simulation> Simulations { get { return _Simulations ??(_Simulations = new List<Simulation>()); } }

        public void Delete() 
        {
            Deleted = DateTime.Now;
        }

        public void Append_Simulation(Simulation simulation)
        {
            simulation.Number = _Simulations.Select(s=> s.Number).DefaultIfEmpty(0).Max() + 1;
            _Simulations.Add(simulation);
        }

        

        
    }

   
}
