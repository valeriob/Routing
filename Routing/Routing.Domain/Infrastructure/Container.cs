using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

using Raven.Client.Indexes;
using Routing.Domain.Services;
using Raven.Client;
using Raven.Client.Document;
using Routing.Domain.ReadModel;

namespace Routing.Domain.Infrastructure
{
    public class Container
    {
        protected static IContainer _Instance;
        public static IContainer Instance
        {
            get 
            { 
                return _Instance ??(_Instance = Init_Container());
            }
        }

        public static IContainer Init_Container()
        {
            var builder = new ContainerBuilder();

            var documentStore = new DocumentStore
            {
                ConnectionStringName = "Routing_Test",
                //   Url = @"http://server:8090",
                //Conventions =
                //{
                //    FindTypeTagName = type => typeof(Resource).IsAssignableFrom(type) ? "Resources" : null,
                //}
                //Conventions =
                //{
                //    FindTypeTagName = type =>
                //    {
                //        if (typeof(Resource).IsAssignableFrom(type))
                //            return "Resources";
                //        if (typeof(Payment).IsAssignableFrom(type))
                //            return "Payments";

                //        return null;
                //    }
                //}
            };

            documentStore.Initialize();

            var generator = new MultiTypeHiLoKeyGenerator(documentStore, 10);
            documentStore.Conventions.DocumentKeyGenerator = entity => generator.GenerateDocumentKey(documentStore.Conventions, entity);

            IndexCreation.CreateIndexes(typeof(Destination_ByLocation).Assembly, documentStore);

            builder.RegisterInstance(documentStore);

            builder.Register(c => c.Resolve<DocumentStore>().OpenSession()).InstancePerLifetimeScope();

            builder.RegisterType<SimulationService>();
            builder.RegisterType<RegistrationService>();
            builder.RegisterType<ScenarioService>();

            builder.RegisterType<References_ReadModel>();
            builder.RegisterType<Scenario_ReadModel>();
            //builder.RegisterType<Simulation_ReadModel>();

            var ioc = builder.Build();

           //var configure = Configure.With()
           //var configure = Configure.WithWeb()
           //     .DefineEndpointName("NServiceBus3Test_MessageQueue")
           //        .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
           //        .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
           //     .DefiningMessagesAs(t => t.Namespace != null && t.Namespace == "Messages")
           // .AutofacBuilder(ioc)
           // .InMemorySubscriptionStorage()
           // .XmlSerializer();

           // var bus = configure.MsmqTransport().IsTransactional(true)
           //     .UnicastBus()
           //     .LoadMessageHandlers()
           //     .CreateBus()
           //     .Start(() =>
           //     {
           //         Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install();
           //     });

            return ioc;
        }
    }
}
