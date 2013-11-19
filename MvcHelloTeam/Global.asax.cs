using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Infrastructure.DataMapping;
using Microsoft.Practices.ServiceLocation;
using MvcHelloTeam.Controllers;
using MvcHelloTeam.Web.Mvc.CastleWindsor;
using SharpArch.Domain.Events;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Web.Mvc;
using SharpArch.Web.Mvc.Castle;
using SharpArch.Web.Mvc.ModelBinder;

namespace MvcHelloTeam
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        private WebSessionStorage _webSessionStorage;



        /// <summary>
        /// Due to issues on IIS7, the NHibernate initialization must occur in Init().
        /// But Init() may be invoked more than once; accordingly, we introduce a thread-safe
        /// mechanism to ensure it's only initialized once.
        /// See http://msdn.microsoft.com/en-us/magazine/cc188793.aspx for explanation details.
        /// </summary>
        public override void Init()
        {
            base.Init();
            this._webSessionStorage = new WebSessionStorage(this);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
      
            NHibernateInitializer.Instance().InitializeNHibernateOnce(this.InitialiseNHibernateSessions);
            // Facebook API
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"CAO PSA OUR\"");

            // Cross domain requests
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
        }

        protected void Application_Start()
        {
            //XmlConfigurator.Configure();
            //AreaRegistration.RegisterAllAreas();
            //ViewEngines.Engines.Clear();

            //ViewEngines.Engines.Add(new RazorViewEngine());

            //ModelBinders.Binders.DefaultBinder = new SharpModelBinder();

            //ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());

            this.InitializeServiceLocator();
            //AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
            //AutoMapperBootstrap.Configure();
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize(); 
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        /// <summary>
        /// Instantiate the container and add all Controllers that derive from
        /// WindsorController to the container.  Also associate the Controller
        /// with the WindsorContainer ControllerFactory.
        /// </summary>
        protected virtual void InitializeServiceLocator()
        {
            var container = new WindsorContainer();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            container.RegisterControllers(typeof(HomeController).Assembly);
            ComponentRegistrar.AddComponentsTo(container);

            var windsorServiceLocator = new WindsorServiceLocator(container);
            DomainEvents.ServiceLocator = windsorServiceLocator;
            ServiceLocator.SetLocatorProvider(() => windsorServiceLocator);
        }

        private void InitialiseNHibernateSessions()
        {
            NHibernateSession.ConfigurationCache = null; // new NHibernateConfigurationFileCache();

            NHibernateSession.Init(
                this._webSessionStorage,
                new[] { Server.MapPath("~/bin/Infrastructure.dll") },
                new AutoPersistenceModelGenerator().Generate(),
                Server.MapPath("~/NHibernate.config"));
        }
    }
}