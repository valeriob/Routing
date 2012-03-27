using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Abstracts
{
    public class AbstractScenarioDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }

        //public int OrdersCount { get; set; }
    }

}
