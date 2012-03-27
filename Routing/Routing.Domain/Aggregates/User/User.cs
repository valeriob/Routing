using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Aggregates
{
    public class User : INamedDocument
    {
        public static readonly User Empty = new User { Name = "Unknown" };

        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime Registered { get; set; }

        private List<Identity> _Identities;
        public IEnumerable<Identity> Identities { get { return _Identities; } }

        private List<Location> _Deposits;
        public IEnumerable<Location> Deposits { get { return _Deposits ??(_Deposits = new List<Location>());} }


        public User()
        {
            _Identities = new List<Identity>();
        }

        public void Identify(string provider, string id, string display)
        {
            _Identities.Add(new Identity { Id = id, Provider = provider, Display_Name = display });
        }
    }

}
