using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Query
{
    public class SearchSimulations : Paging
    {
        public string UserId { get; set; }
        public string ScenarioId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
