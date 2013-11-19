using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MemberCenter20NS.Controllers;
using MvcHelloTeam.Models;
using SALI;
using SFAFGlobalObjects;


namespace MvcHelloTeam.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/
        private delegate void _loadTopNavMenu();
        private delegate void _setClientInfo();

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous,AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserLogOut()
        {
            var vm = new LoginResponseModel {Success = true};
            Logout();
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
       
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
                     var clientModel = LoginCustomerContact(inputViewModel.UserName, inputViewModel.Password, delegates,
                                                            false);
                     vm.Success = clientModel.LoginSuccess;
                    if (!clientModel.LoginSuccess)
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

    }
}
