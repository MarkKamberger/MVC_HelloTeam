﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Web.Security;
using DomainLayer;
using MvcPrototype.BaseModels;
using MvcPrototype.Injection;
using MvcPrototype.Models;
using SALI;
using SALISecurityObjects;
using SFABase;
using SFAFGlobalObjects;

namespace MvcPrototype.Controllers.Security
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
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
                    }
                }
                else
                {
                    vm.Success = false;
                    vm.Message = "Sorry you do not have permission to log into Member Center";
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
                    IsDemo = _baseModel.BusinessLogicObject.IsDemo;
                    //_baseModel.UserPermissions = new UserPermissions(_baseModel.UserName, _baseModel.ClientModel.CurrentLoginId, DataAccessTypes.NetworkDatabase, _baseModel.ClientModel.CurrentSessionId).AsDataTable();
                    SessionAuthenticated = true;
                    Session["UseSession"] = 1;
                    UserSecurityObject = SSOFactory.MakeObject(new UserPermissions(_baseModel.ClientModel.UserName, _baseModel.ClientModel.CurrentLoginId, DataAccessTypes.NetworkDatabase, _baseModel.ClientModel.CurrentSessionId).AsDataTable());
                    HttpRuntime.Cache.GetOrStore<StrongSecurityObject>("UserSecurityObject",UserSecurityObject);
                    HttpRuntime.Cache.GetOrStore<ClientModel>("ClientModel", _baseModel.ClientModel);
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