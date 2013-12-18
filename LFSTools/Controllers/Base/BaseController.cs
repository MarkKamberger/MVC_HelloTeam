using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Providers.Entities;
using System.Web.Security;
using DomainLayer;
using DomainLayer.NavigationModels;
using DomainLayer.SecurityModels;
using LFSTools.BaseModels;
using LFSTools.Helpers;
using LFSTools.Models;
using SALIBusinessLogic;
using SALISecurityObjects;
using SFABase;
using SFAFGlobalObjects;
using Services;
using Services.SecurityService;

namespace LFSTools
{
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        private readonly ISecurityService _securityService;
        private ITWAService _twaService;
        public StrongSecurityObject UserSecurityObject;
        protected BaseModel _baseModel;
        protected ISFAFPresentation _presenter = null;
        protected enum LoginType
        {
            Staff = 1,
            CustomerContact = 2
        }
        public User CurrentUser { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController(ISecurityService securityService, ITWAService twaService)
        {
            _securityService = securityService;
            _twaService = twaService;
            _baseModel = new BaseModel
                {
                    ClientModel = new ClientModel(),
                    NavigationLinks = new List<_Mvc_ListNavigationLinks>(),
                    ENABLE_GLOBAL_CACHE = false
                };
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SFAFAppName"]))
                _baseModel.SFAFAppName = ConfigurationManager.AppSettings["SFAFAppName"];

            _baseModel.ENABLE_GLOBAL_CACHE = NumberHelpers.ReturnAsBool(ConfigurationManager.AppSettings["EnableGlobalCache"]);

            _presenter = new BondedContentPage.BondedPresenter(System.Web.HttpContext.Current.Session, _baseModel.SFAFAppName,
                                                         _baseModel.ENABLE_GLOBAL_CACHE,
                                                         System.Web.HttpContext.Current.Cache);
            _baseModel.BusinessLogicObject = new SALIMainBusinessLogic(_presenter, true);
            ViewData["BaseModel"] = _baseModel; 

        }


        /// <summary>
        /// Initializes data that might not be available when the constructor is called.
        /// </summary>
        /// <param name="requestContext">The HTTP context and route data.</param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (!Session.IsNewSession)
            {
                if (requestContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    string username = requestContext.HttpContext.User.Identity.Name;
                    UserSecurityObject = Session["UserSecurityObject"] as StrongSecurityObject;
                    _baseModel.ClientModel = Session["ClientModel"] as ClientModel;
                    _baseModel.NavigationLinks = Session["NavigationLinks"] as List<_Mvc_ListNavigationLinks>;
                    //string username = _baseModel.BusinessLogicObject.UserName;
                    _baseModel.ClientModel.IsLoggedIn = true;
                    ViewData["CurrentUser"] = Session["UserName"] as string;
                    ViewData["BaseModel"] = _baseModel;

                }
                else
                    ViewData["CurrentUser"] = null; 
            }
            else
            {
               
            }
            
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
            FormsAuthentication.SignOut();
            _presenter.ClearPresenterCache();
        }

        public bool LoginOrm(LoginInputModel inputViewModel)
        {
            var user = new _SALI_LoginWebUser();
            var result = _securityService.LoginWebUser(inputViewModel.UserName, inputViewModel.Password, false);
            if (result.Count > 0)
            {
                user = result.First();
                if (user != null)
                {
                    UserSecurityObject = SSOFactory.MakeObject(new UserPermissions(
                                                                   userName: inputViewModel.UserName
                                                                   , customerId: user.Id
                                                                   , accessType: DataAccessTypes.NetworkDatabase
                                                                   , sessionId: _baseModel.ClientModel.CurrentSessionId)
                                                                   .AsDataTable());
                    if (SecurityPermissionHelper.CheckObjectPermission(UserSecurityObject, ObjectsSSO.Login,
                                                                       ScopeSSO.SELF,
                                                                       PrivilegeSSO.SELECT))
                    {
                        _baseModel.ClientModel.UserName = inputViewModel.UserName;
                        _baseModel.ClientModel.LoginSuccess = true;
                        _baseModel.ClientModel.CurrentSessionId = Session.SessionID;
                        _baseModel.ClientModel.Message = "Success";
                        _baseModel.ClientModel.IsLoggedIn = true;
                        FormsAuthentication.SetAuthCookie(inputViewModel.UserName, false);
                        UserSecurityObject = SSOFactory.MakeObject(new UserPermissions(_baseModel.ClientModel.UserName
                                                                                       ,
                                                                                       _baseModel.ClientModel
                                                                                                 .CurrentLoginId
                                                                                       , DataAccessTypes.NetworkDatabase
                                                                                       ,
                                                                                       _baseModel.ClientModel
                                                                                                 .CurrentSessionId)
                                                                       .AsDataTable());
                        _baseModel.UserSecurityObject = UserSecurityObject;
                        _baseModel.NavigationLinks = _twaService.LisNavigationLinks(NumberHelpers.ReturnAsInt(ConfigurationManager.AppSettings["ApplicationId"])
                                                                                    , _baseModel.UserSecurityObject
                                                                                    ,
                                                                                    _baseModel.BusinessLogicObject
                                                                                              .UserId
                                                                                    ,
                                                                                    _baseModel.BusinessLogicObject
                                                                                              .UserCustomerId);
                        Session["UserSecurityObject"] = UserSecurityObject;
                        Session["ClientModel"] = _baseModel.ClientModel;
                        Session["NavigationLinks"] = _baseModel.NavigationLinks;
                        Session["UserName"] = inputViewModel.UserName;
                        Session["Password"] = inputViewModel.Password;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
