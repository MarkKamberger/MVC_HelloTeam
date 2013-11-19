using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using BootstrapMvcSample.Controllers;
using MemberCenter20NS.Models;
using MvcHelloTeam.Models;
using SALI;
using SALIBusinessLogic;
using SFAFGlobalObjects;

namespace MemberCenter20NS.Controllers
{
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        public string _startDate;
        public string _endDate;
        public string setSessionVariableLocation = string.Empty;
        public string setSessionCenterImageLocation = string.Empty;
        public User CurrentUser { get; set; }
        private HttpSessionState _sessionState;


        protected enum LoginType
        {
            Staff = 1,
            CustomerContact = 2
        }

        private SALIMainBusinessLogic _businessLogic;



        private List<Delegate> _postLoginMethods = new List<Delegate>();


        protected ISFAFPresentation _presenter = null;
        protected BaseInputModel _baseModel;



        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        public BaseController()
        {
            //We can build plugins to the site to add functionality for specific users 
            //mefControllerFactory = new MefControllerFactory("Plugins\\");
            //Plugins = mefControllerFactory.MyPlugins;
            _baseModel = new BaseInputModel();
            _baseModel.ENABLE_GLOBAL_CACHE = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SFAFAppName"]))
                _baseModel.SFAFAppName = ConfigurationManager.AppSettings["SFAFAppName"];

            _baseModel.ENABLE_GLOBAL_CACHE = ReturnAs.Bool(ConfigurationManager.AppSettings["EnableGlobalCache"]);

            _presenter = new SFAFMemberCenter20Presenter(System.Web.HttpContext.Current.Session, _baseModel.SFAFAppName,
                                                         _baseModel.ENABLE_GLOBAL_CACHE,
                                                         System.Web.HttpContext.Current.Cache);


            bool sessionOK = false;

            _baseModel.BusinessLogicObject = new SALIMainBusinessLogic(_presenter, true);
            _baseModel.ClientModel = new ClientModel();
            ViewData["BaseInputModel"] = _baseModel; 

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
                ViewData["BaseInputModel"] = _baseModel;

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

        #region  Methods

        protected ClientModel LoginCustomerContact(string userName, string password)
        {
            return LoginCustomerContact(userName, password, new List<Delegate>());
        }

        protected ClientModel LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates)
        {
            return LoginUser(userName, password, LoginType.CustomerContact, postLoginDelegates, false);
        }

        protected ClientModel LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates,
                                                   bool isDemo)
        {
            return LoginUser(userName, password, LoginType.CustomerContact, postLoginDelegates, isDemo);
        }

        protected ClientModel LoginUser(string userName, string password, LoginType type,
                                        List<Delegate> postLoginDelegates, bool isDemo)
        {
            
            bool result = false;
            bool auditLogged = false;

            string message = "Unknown Result";

            if (userName.Trim() != string.Empty && password.Trim() != string.Empty)
            {
                try
                {
                    switch (type)
                    {
                        case LoginType.Staff:
                            result = _baseModel.BusinessLogicObject.LoginSFAFStaff(userName.Trim(), password.Trim());
                            break;
                        case LoginType.CustomerContact:
                            result = _baseModel.BusinessLogicObject.LoginCustomerContact(userName.Trim(),
                                                                                         password.Trim(), isDemo);
                            break;
                    }
                }
                catch (SFAFLoginError le)
                {
                    _baseModel.ClientModel.LoginSuccess = false;
                    _baseModel.ClientModel.Message = le.LoginErrorMessage;
                    _baseModel.ClientModel.CurrentSessionId = _baseModel.BusinessLogicObject.SetInvalidSession();
                    if (!auditLogged)
                    {
                        _baseModel.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.Login,
                                                                         "User " + userName + " login error: " +
                                                                         le.LoginErrorMessage);
                        auditLogged = true;
                    }
                }
                catch (SFAFGenericError ge)
                {
                    _baseModel.ClientModel.LoginSuccess = false;
                    _baseModel.ClientModel.Message = ge.Message;
                    _baseModel.ClientModel.CurrentSessionId = _baseModel.BusinessLogicObject.SetInvalidSession();

                    if (!auditLogged)
                    {
                        _baseModel.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.Login,
                                                                         "User " + userName + " login error: " +
                                                                         ge.Message);
                        auditLogged = true;
                    }
                }
                catch (Exception e)
                {
                    _baseModel.ClientModel.LoginSuccess = false;
                }

                if (result)
                {
                    _baseModel.ClientModel.Message = string.Empty;
                }
            }
            else
                _baseModel.ClientModel.Message = "Please enter username and password!";

            if (result)
            {
                bool keepGoing = true;

                for (int x = 0; x < postLoginDelegates.Count; x++)
                {
                    try
                    {
                        postLoginDelegates[x].DynamicInvoke(null);
                    }
                    catch (Exception exc)
                    {
                        _baseModel.ClientModel.Message = "A System Error has occurred which prevented you from logging in." +
                                              Environment.NewLine + exc.InnerException.Message;
                        _baseModel.ClientModel.CurrentSessionId = _baseModel.BusinessLogicObject.SetInvalidSession();
                        if (!auditLogged)
                        {
                            _baseModel.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace,
                                                                             AuditSubjectsEnum.Login,
                                                                             "User " + userName + " login error: " +
                                                                             exc.InnerException.Message);
                            auditLogged = true;
                        }
                        keepGoing = false;
                    }
                }

                if (keepGoing)
                {
                    _baseModel.ClientModel.LoginSuccess = true;
                    _baseModel.ClientModel.CurrentSessionId = _baseModel.BusinessLogicObject.SetInvalidSession();
                    _baseModel.ClientModel.Message = "Success";
                    _baseModel.ClientModel.IsLoggedIn = true;
                    FormsAuthentication.SetAuthCookie(_baseModel.BusinessLogicObject.UserName, false);

                }
              
            }
            else
            {
                _baseModel.ClientModel.Message = message;
                _baseModel.ClientModel.LoginSuccess = false;
                _baseModel.ClientModel.CurrentSessionId = _baseModel.BusinessLogicObject.SetInvalidSession();
                if (!auditLogged)
                {
                    _baseModel.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.Login,
                                                                     "User " + userName + " login error: " + message);
                }
            }
            return _baseModel.ClientModel;
        }
        protected bool Logout()
        {
            try
            {
                _presenter.ClearPresenterCache();
            }
            catch { }
            _baseModel.ClientModel.IsLoggedIn = false;
            _baseModel.ClientModel.LoginSuccess = false;
            SessionId = string.Empty;
            UserId = -1;
            UserType = SALIUserTypes.Unknown;
            UserName = string.Empty;
            Password = string.Empty;
            UserFirstName = string.Empty;
            UserMiddleName = string.Empty;
            UserLastName = string.Empty;
            UserCustomerId = -1;
            CurrentSchoolYearId = -1;
            IsDistrictCustomer = false;
            IsCustomerTrackSchool = false;
            CurrentCustomerName = string.Empty;
            CurrentSchoolYear = string.Empty;
            FormsAuthentication.SignOut();
            AssignLoginValuesToPresenter();
            ViewData["BaseInputModel"] = _baseModel;
            return true;
        }
        #endregion

        #region Protected Properties
        public string SessionId { get; set; }

        public string LoginId { get; set; }

        public int CurrentApplicationId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int UserId { get; set; }

        public int UserCustomerId { get; set; }

        public SALIUserTypes UserType { get; set; }

        public bool IsDemo { get; set; }

        public string LastError { get; set; }

        public string UserFirstName { get; private set; }

        public string UserMiddleName { get; private set; }

        public string UserLastName { get; private set; }

        public int CurrentSchoolYearId { get; set; }

        public bool IsDistrictCustomer { get; set; }

        public bool IsCustomerTrackSchool { get; set; }

        public string CurrentSchoolYear { get; set; }

        public string CurrentCustomerName { get; set; }

        private void AssignLoginValuesToPresenter()
        {
            _presenter.CurrentSessionId = SessionId;
            _presenter.CurrentLoginId = LoginId;
            _presenter.CurrentUserName = UserName;
            _presenter.CurrentUserId = UserId;
            _presenter.CurrentUserPassword = Password;
            _presenter.CurrentUserType = UserType;
            _presenter.CurrentUserFirstName = UserFirstName;
            _presenter.CurrentUserLastName = UserLastName;
            _presenter.CurrentUserMiddleName = UserMiddleName;
            _presenter.CurrentCustomerId = UserCustomerId;
            _presenter.CurrentSchoolYearId = CurrentSchoolYearId;
            _presenter.IsDistrict = IsDistrictCustomer;
            _presenter.IsTrackSchool = IsCustomerTrackSchool;
            _presenter.CurrentSchoolYear = CurrentSchoolYear;
            _presenter.CurrentCustomerName = CurrentCustomerName;
            _presenter.IsDemo = IsDemo;
        }

        #endregion
    }
}
