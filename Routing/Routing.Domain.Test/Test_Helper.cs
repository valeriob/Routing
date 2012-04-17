using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Autofac;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Embedded;
using Routing.Domain.Dto;
using Routing.Domain.Dto.Command;
using Routing.Domain.Aggregates;
using Routing.Domain.ValueObjects;

namespace Routing.Domain.Test
{
    public class Test_Helper
    {
        public static IDocumentStore Init_Document_Store()
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

            // IndexCreation.CreateIndexes(typeof(LocalizedResource_ByLocation).Assembly, documentStore);

            builder.RegisterInstance(documentStore);

            builder.Register(c => c.Resolve<DocumentStore>().OpenSession()).InstancePerLifetimeScope();

            return documentStore;
        }

        public static ScenarioDto Build_Random_Scenario()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var number = random.Next();
            var locations = new List<Location>();

            var scenario = new ScenarioDto 
            { 
                Date = DateTime.Now, 
                Name = "Scenario " + number,
                UserId = "user bla"
            };
            scenario.Deliveries = new List<DeliveryDto>();
            scenario.Distances = new List<DistanceDto>();


            number = random.Next(10) + 2;
            for (int i = 0; i < number; i++)
            {
                var location = new Location(i, i);
                locations.Add(location);
                var delivery = new DeliveryDto 
                { 
                    Address = "address", 
                    Description = "Description",
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Number = i,  
                    Delivering = DateTime.Today.AddDays(1)
                };
                
                scenario.Deliveries.Add(delivery);
            }


            foreach (var dist in new Utilities.Combinatorics.Combinations<Location>(locations, 2))
            {
                var from = dist[0];
                var to = dist[1];
                scenario.Distances.Add(new DistanceDto 
                {  
                    From_Latitide = from.Latitude, 
                    From_Longitude = from.Longitude, 

                    To_Latitide = to.Latitude, 
                    To_Longitude = to.Longitude, 
                    
                    Km = random.Next(), 
                    TimeInSeconds= random.Next()
                });
            }

            return scenario;
        }

        public static IEnumerable<Destination> Known_Destinations()
        {
            var list = new List<Destination>();
            list.Add(new Destination 
            { 
                Location = new Location(1,1) { Address ="via dei frati 1, Modigliana, Italy" }, 
                ExternalId = "External_Location_1", 
                Id ="Location/1",   
                Name= "casa"
            });
            list.Add(new Destination
            {
                Location = new Location(1, 1) { Address = "via garibaldi, Modigliana, Italy" },
                ExternalId = "External_Location_2",
                Id = "Location/2",
                Name = "centro"
            });
            list.Add(new Destination
            {
                Location = new Location(1, 1) { Address = "Russi, Italy" },
                ExternalId = "External_Location_3",
                Id = "Location/3",
                Name = "Russi"
            });
            list.Add(new Destination
            {
                Location = new Location(1, 1) { Address = "Faenza, Italy" },
                ExternalId = "External_Location_4",
                Id = "Location/4",
                Name = "Faenza"
            });
            return list;
        }
    }

    public static class Test_Extensions
    {
        public static IContainer Configure_Raven_For_Testing(this IContainer container)
        {
            var documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            };

            documentStore.Initialize();

            var generator = new MultiTypeHiLoKeyGenerator(documentStore, 10);
            documentStore.Conventions.DocumentKeyGenerator = entity => generator.GenerateDocumentKey(documentStore.Conventions, entity);

            IndexCreation.CreateIndexes(typeof(Destination_ByLocation).Assembly, documentStore);


            var builder = new ContainerBuilder();
            builder.RegisterInstance(documentStore).As<IDocumentStore>();
            builder.Update(container);

            return container;
        }

        public static IContainer Configure_NServiceBus_For_Testing(this IContainer container)
        {
            //  var configure = Configure.With()

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

            return container;
        }

    }
}
