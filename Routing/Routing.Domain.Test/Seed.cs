using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Autofac;
using Routing.Domain.Aggregates;
using Raven.Client.Document;

namespace Routing.Domain.Test
{
    [TestClass]
    public class Seed
    {
        IContainer Container;
        DocumentStore DocumentStore;
        public bool initialized;

        public void Init()
        {
            if (initialized)
                return;
            Container = Routing.Domain.Infrastructure.Container.Instance;
            //var builder = new ContainerBuilder();
            //var document = Test_Helper.Init_Document_Store();
            //builder.Register(c => c.Resolve<DocumentStore>().OpenSession()).InstancePerLifetimeScope();

            //Container = builder.Build();
            initialized = true;
        }


        [TestMethod]
        public void Seed_Accounts()
        {
            Init();
            var service = Container.Resolve<IDocumentSession>();

            service.Store(new User { Registered = DateTime.Now  });

            service.SaveChanges();
        }

        [TestMethod]
        public void Seed_Clients()
        {
            Init();
            var service = Container.Resolve<IDocumentSession>();

            var account = service.Query<User>().First();

            service.Store(new Destination { UserId = account.Id, Name = "client 1", Address = new ValueObjects.Address { City = "Mugiana" } });
            service.Store(new Destination { UserId = account.Id, Name = "client 2", Address = new ValueObjects.Address { City = "Faenza" } });
            service.Store(new Destination { UserId = account.Id, Name = "client 3", Address = new ValueObjects.Address { City = "Russi" } });

            service.SaveChanges();
        }
    }
}
