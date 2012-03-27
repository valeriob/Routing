using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;

namespace Routing.Domain.ReadModel
{
    public class Simulation_ReadModel
    {
        IDocumentSession Session;
        public Simulation_ReadModel(IDocumentSession documentSession)
        {
            Session = documentSession;
        }
    }
}
