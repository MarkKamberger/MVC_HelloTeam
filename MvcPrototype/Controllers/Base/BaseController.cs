using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Security;
using System.Web.SessionState;
using DomainLayer;
using MvcPrototype.BaseModels;
using MvcPrototype.Injection;
using SALIBusinessLogic;
using SFAFGlobalObjects;

namespace MvcPrototype
{
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        public string _startDate;
        public string _endDate;
        public bool IsDemo = false;
        public string setSessionVariableLocation = string.Empty;
        public string setSessionCenterImageLocation = string.Empty;
        public User CurrentUser { get; set; }
        public bool SessionAuthenticated = false;
        private HttpSessionState _sessionState;
        protected enum LoginType
        {
            Staff = 1,
            CustomerContact = 2
        }
        private SALIMainBusinessLogic _businessLogic;
        private List<Delegate> _postLoginMethods = new List<Delegate>();
        protected ISFAFPresentation _presenter = null;
        protected BaseModel _baseModel;
        public StrongSecurityObject UserSecurityObject;



        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {
            //UserSecurityObject = new StrongSecurityObject();
            

            _baseModel = new BaseModel();
            _baseModel.ENABLE_GLOBAL_CACHE = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SFAFAppName"]))
                _baseModel.SFAFAppName = ConfigurationManager.AppSettings["SFAFAppName"];

            _baseModel.ENABLE_GLOBAL_CACHE = NumberHelpers.ReturnAsBool(ConfigurationManager.AppSettings["EnableGlobalCache"]);

            _presenter = new BondedContentPage.BondedPresenter(System.Web.HttpContext.Current.Session, _baseModel.SFAFAppName,
                                                         _baseModel.ENABLE_GLOBAL_CACHE,
                                                         System.Web.HttpContext.Current.Cache);

            
            bool sessionOK = false;

            _baseModel.BusinessLogicObject = new SALIMainBusinessLogic(_presenter, true);
            UserSecurityObject = HttpRuntime.Cache.Get<StrongSecurityObject>("UserSecurityObject") ?? new StrongSecurityObject();
            _baseModel.ClientModel = HttpRuntime.Cache.Get<ClientModel>("ClientModel") ?? new ClientModel();
            ViewData["BaseModel"] = _baseModel; 

        }


        /// <summary>
        /// Initializes data that might not be available when the constructor is called.
        /// </summary>
        /// <param name="requestContext">The HTTP context and route data.</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string username = _baseModel.BusinessLogicObject.UserName;
                _baseModel.ClientModel.IsLoggedIn = true;
                ViewData["CurrentUser"] = "";
                ViewData["BaseModel"] = _baseModel;

            }
            else
                ViewData["CurrentUser"] = null;
        }

        /// <summary>
        /// Renders a specified view and returns it as a string
        /// </summary>
        /// <param name="viewName">View to render</param>
        /// <param name="model">View model data</param>
        /// <returns></returns>
        public string RenderView(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }


        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            _baseModel.ClientModel = new ClientModel();
            UserSecurityObject = new StrongSecurityObject();
            Session["UseSession"] = 0;
            FormsAuthentication.SignOut();
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            ViewData["BaseInputModel"] = _baseModel;
            _presenter.ClearPresenterCache();
            HttpRuntime.Cache.Remove("ClientModel");
            HttpRuntime.Cache.Remove("UserSecurityObject");
        }
    }
}
