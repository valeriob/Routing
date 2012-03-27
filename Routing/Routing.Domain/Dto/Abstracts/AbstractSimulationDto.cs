using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Abstracts
{
    public class AbstractSimulationDto
    {
        public int Number { get; set; }
        public string Name { get; set; }

        //public string Scenario_Id { get; set; }
        //public string Scenario_Name { get; set; }

        //public string User_Id { get; set; }
        //public string User_Name { get; set; }

        public DateTime Created { get; set; }

       
    }

}
