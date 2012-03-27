using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto
{
    public class SimulationDto
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public string Starting_Address { get; set; }
        public double Starting_Latitude { get; set; }
        public double Starting_Longitude { get; set; }

        public string Returning_Address { get; set; }
        public double Returning_Latitude { get; set; }
        public double Returning_Longitude { get; set; }

        public DateTime Created { get; set; }

        public List<VoyageDto> Voyages { get; set; }
    }

    public class VoyageDto
    {
        public DateTime Departing { get; set; }
        public TimeSpan Exitmated_Time { get; set; }
        public double Extimated_Lenght_Km { get; set; }

        public List<int> Orders { get; set; }
        //public IEnumerable<OrderDto> Orders { get; set; }
    }
}
