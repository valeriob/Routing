using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Routing.Domain.Dto.Abstracts;

namespace Routing.Web.Models.Scenario
{
    public class Search_Scenario_ViewModel
    {
        public IEnumerable<AbstractScenarioDto> Scenari { get; set; }

    }
}