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
using Microsoft.Practices.Unity;
using MvcPrototype.Controllers;
using SharpArch.Domain.Events;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Web.Mvc;
using SharpArch.Web.Mvc.Castle;

namespace MvcPrototype
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private WebSessionStorage _webSessionStorage;
        /// <summary>
        /// Executes custom initialization code after all event handler modules have been added.
        /// </summary>
        public override void Init()
        {
            base.Init();
            this._webSessionStorage = new WebSessionStorage(this);
        }

        /// <summary>
        /// Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            NHibernateInitializer.Instance().InitializeNHibernateOnce(this.InitialiseNHibernateSessions);
            // Facebook API
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"CAO PSA OUR\"");

            // Cross domain requests
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
        }

        /// <summary>
        /// Application_s the start.
        /// </summary>
        protected void Application_Start()
        {
            this.InitializeServiceLocator();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Bootstrapper.Initialise();
        }

        #region Protected / Private Methods
        /// <summary>
        /// Initializes the service locator.
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

        /// <summary>
        /// Initialises the N hibernate sessions.
        /// </summary>
        private void InitialiseNHibernateSessions()
        {
            NHibernateSession.ConfigurationCache = null; // new NHibernateConfigurationFileCache();

            NHibernateSession.Init(
                this._webSessionStorage,
                new[] { Server.MapPath("~/bin/Infrastructure.dll") },
                new AutoPersistenceModelGenerator().Generate(),
                Server.MapPath("~/NHibernate.config"));
        }
        #endregion
    }
}