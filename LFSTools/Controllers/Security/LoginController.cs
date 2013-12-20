using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using DomainLayer;
using LFSTools.BaseModels;
using LFSTools.Models;
using LFSTools.Providers;
using SALI;
using SALISecurityObjects;
using SFABase;
using SFAFGlobalObjects;
using Services;
using Services.SecurityService;

namespace LFSTools.Controllers.Security
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/
         private readonly StrongSecurityObject _securityObject;
         private readonly ITWAService _twaService;
        private readonly ISecurityService _securityService;
         public LoginController(ITWAService twaService, ISecurityService securityService) : base(ServiceFactory.CreateSecurityService(),ServiceFactory.CreateTWAService())
         {
             
            _twaService = twaService;
            _securityObject = new StrongSecurityObject();
             _securityService = securityService;
         }
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Request.Browser.IsMobileDevice )
            {
                return View("MobileLogin");
            }
            else
            {
                return View();
            }
          
        }

        /// <summary>
        /// Users the log out.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous, AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserLogOut()
        {
            var vm = new LoginResponseModel {Success = true};
            Logout();
            return Json(vm, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="inputViewModel">The input view model.</param>
        /// <returns></returns>
        public ActionResult UserLogin(LoginInputModel inputViewModel)
        {
            var vm = new LoginResponseModel();
            var isValid = true;
            if (String.IsNullOrEmpty(inputViewModel.UserName) || String.IsNullOrEmpty(inputViewModel.Password))
            {
                vm.Success = false;
                vm.Message = "You must enter a user name & password";
                isValid = false;
            }
            if (isValid)
            {
                if (_baseModel.BusinessLogicObject.CheckUserObjectPermissionDB(inputViewModel.UserName,
                                                                               "Login",
                                                                               SFAFSecurityLevel.SELECT,
                                                                               SFAFSecurityScope.SELF))
                {
                    _baseModel.BusinessLogicObject.WriteToLog("Logging in user \"" + inputViewModel.UserName +
                                                              "\" from client " + Request.UserHostAddress);
                    var delegates = new List<Delegate>();
                    Session.Timeout = 61;
                    LoginCustomerContact(inputViewModel.UserName, inputViewModel.Password, delegates,
                                         false);

                    vm.Success = _baseModel.ClientModel.LoginSuccess;
                    if (!_baseModel.ClientModel.LoginSuccess)
                    {
                        vm.Message = "Login Unsuccessful";
                    }
                    else
                    {
                       
                        vm.UserName = _baseModel.BusinessLogicObject.UserFirstName + " " +
                                      _baseModel.BusinessLogicObject.UserLastName;
                        vm.Message = "Success";
                        
                        ViewData["BaseModel"] = _baseModel;
                    }
                }
                else
                {
                    vm.Success = false;
                    vm.Message = "Invalid username/password";
                    _baseModel.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.Login,
                                                                     "User " + inputViewModel.UserName +
                                                                     " attempted to login but does not have permission");
                }

            }
            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserLoginMethodOrm(LoginInputModel inputViewModel)
        {
            var vm = new LoginResponseModel();
            var isValid = true;
            if (String.IsNullOrEmpty(inputViewModel.UserName) || String.IsNullOrEmpty(inputViewModel.Password))
            {
                vm.Success = false;
                vm.Message = "You must enter a user name & password";
                isValid = false;
            }
            if (isValid)
            {
                if (LoginOrm(inputViewModel))
                {
                    vm.Message = "Welcome";
                    vm.Success = true;
                }
                else
                {
                    vm.Success = false;
                    vm.Message = "Invalid username/password";
                    _baseModel.BusinessLogicObject.WriteAuditHistory(AuditTypesEnum.Trace, AuditSubjectsEnum.Login,
                                                                     "User " + inputViewModel.UserName +
                                                                     " attempted to login but does not have permission");
                }

            }
            return Json(vm, JsonRequestBehavior.AllowGet);
        }






        #region  Private Methods

      


        /// <summary>
        /// Logins the customer contact.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private ClientModel LoginCustomerContact(string userName, string password)
        {
            return LoginCustomerContact(userName, password, new List<Delegate>());
        }

        /// <summary>
        /// Logins the customer contact.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="postLoginDelegates">The post login delegates.</param>
        /// <returns></returns>
        private ClientModel LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates)
        {
            return LoginUser(userName, password, LoginType.CustomerContact, postLoginDelegates, false);
        }

        /// <summary>
        /// Logins the customer contact.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="postLoginDelegates">The post login delegates.</param>
        /// <param name="isDemo">if set to <c>true</c> [is demo].</param>
        /// <returns></returns>
        private ClientModel LoginCustomerContact(string userName, string password, List<Delegate> postLoginDelegates,
                                                   bool isDemo)
        {
            return LoginUser(userName, password, LoginType.CustomerContact, postLoginDelegates, isDemo);
        }

        /// <summary>
        /// Logins the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="type">The type.</param>
        /// <param name="postLoginDelegates">The post login delegates.</param>
        /// <param name="isDemo">if set to <c>true</c> [is demo].</param>
        /// <returns></returns>
        private ClientModel LoginUser(string userName, string password, LoginType type,
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
                        _baseModel.ClientModel.Message =
                            "A System Error has occurred which prevented you from logging in." +
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
                    _baseModel.ClientModel.UserName = userName;
                    _baseModel.ClientModel.LoginSuccess = true;
                    _baseModel.ClientModel.CurrentSessionId = _baseModel.BusinessLogicObject.SetInvalidSession();
                    _baseModel.ClientModel.Message = "Success";
                    _baseModel.ClientModel.IsLoggedIn = true;
                   
                    
                    FormsAuthentication.SetAuthCookie(_baseModel.BusinessLogicObject.UserName, false);
                    Session["UseSession"] = 1;
                    UserSecurityObject = SSOFactory.MakeObject(new UserPermissions(_baseModel.ClientModel.UserName
                        , _baseModel.ClientModel.CurrentLoginId
                        ,DataAccessTypes.NetworkDatabase
                        , _baseModel.ClientModel.CurrentSessionId).AsDataTable());
                    _baseModel.UserSecurityObject = UserSecurityObject;
                    _baseModel.NavigationLinks = _twaService.LisNavigationLinks(NumberHelpers.ReturnAsInt(ConfigurationManager.AppSettings["ApplicationId"])
                        ,_baseModel.UserSecurityObject
                        ,_baseModel.BusinessLogicObject.UserId
                        ,_baseModel.BusinessLogicObject.UserCustomerId);

                    Session["UserSecurityObject"] = UserSecurityObject;
                    Session["ClientModel"] = _baseModel.ClientModel;
                    Session["NavigationLinks"] = _baseModel.NavigationLinks;
                    Session["UserName"] = userName;
                    Session["Password"] = password;
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

       
        #endregion
    }

}
