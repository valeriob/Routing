using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Routing.Domain.Dto;

namespace Routing.Domain.Services
{
    public class SimulationService
    {
        IDocumentSession DocumentSession { get; set; }

        public SimulationService(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;

        }

        //public void Update(SimulationDto simulation)
        //{

        //}
       
    }
}
