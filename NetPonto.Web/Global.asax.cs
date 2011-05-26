using System;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;
using Autofac.Integration.Mvc;
using AutofacContrib.CommonServiceLocator;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using NetPonto.Infrastructure.Authentication;
using NetPonto.Infrastructure.Log;
using NetPonto.Infrastructure.Storage;


namespace NetPonto.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static IContainer Container;
        public static ISession Session
        {
            get
            {
                return Container.Resolve<ISession>();
            }
        }
        public ILogger Logger
        {
            get
            {
                return Container.Resolve<ILogger>();
            }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.([iI][cC][oO]|[gG][iI][fF])(/.*)?" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                "Login", // Route name
                "login", // URL with parameters
                new { controller = "Session", action = "Create" } // Parameter defaults
            );

            routes.MapRoute(
                "Logout", // Route name
                "logout", // URL with parameters
                new { controller = "Session", action = "Delete" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            ConfigureAutofac();

            // Finish initialization of MVC-related items.
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Logger.Info("Configuration Over! Application booting...");
        }
        protected void Application_End()
        {
            Logger.Info("Going down for a break...");
        }


        private void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);


            builder.RegisterModelBinders(typeof(NLogLogger).Assembly);
            builder.RegisterModelBinders(typeof(AspMembershipAuthentication).Assembly);
            builder.RegisterModelBinderProvider();


            // Register my interfaces
            builder.RegisterType<NLogLogger>().As<ILogger>().SingleInstance();
            builder.RegisterType<AspMembershipAuthentication>().As<IUserAuthentication>();

            // Set the MVC dependency resolver to use Autofac.
            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));

            // Set the EntLib service locator to use Autofac.
            var autofacLocator = new AutofacServiceLocator(Container);
            EnterpriseLibraryContainer.Current = autofacLocator;

        }
    }
}