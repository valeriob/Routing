using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Routing.Domain.Infrastructure;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using Routing.Web.Services;
using Autofac.Integration.Wcf;
using System.Web.Optimization;

namespace Routing.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            var container = Container.Instance;
    

            var builder = new ContainerBuilder();

            Container.Configure_Raven(builder);

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            
            // http://code.google.com/p/autofac/wiki/Mvc3Integration
            //builder.RegisterModelBinders(Assembly.GetExecutingAssembly()); 
            //builder.RegisterModelBinderProvider();

            builder.RegisterType<References>();
            builder.RegisterType<Scenario>();

            builder.Update(container);

            AutofacHostFactory.Container = container;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); 
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleTable.Bundles.RegisterTemplateBundles();
        }
    }
}