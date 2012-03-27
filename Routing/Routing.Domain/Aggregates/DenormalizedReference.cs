using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Aggregates
{
    public class DenormalizedReference<T> where T : INamedDocument
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static implicit operator DenormalizedReference<T>(T document)
        {
            return new DenormalizedReference<T>
            {
                Id = document.Id,
                Name = document.Name
            };
        }
    }

    public interface INamedDocument
    {
        string Id { get; set; }
        string Name { get; set; }
    }

}
