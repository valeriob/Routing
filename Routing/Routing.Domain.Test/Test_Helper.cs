using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Autofac;
using Raven.Client.Document;

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
    }
}
