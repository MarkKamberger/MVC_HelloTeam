﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DomainLayer;
using DomainLayer.Helper;
using SALISecurityObjects;

namespace LFSTools
{
    /// <summary>
    /// Verifies that user has a specified role or is in a list of roles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property,
                   AllowMultiple = true, Inherited = true)]
    public class RequiresRoleAttribute : ActionFilterAttribute
    {
        public RoleSSO Role { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <remarks>Fist check if authenticated, then check role</remarks>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;
            var failAuthentication = !filterContext.HttpContext.User.Identity.IsAuthenticated;
            if (failAuthentication)
            {
                controller.Logout();
                var loginUrl = FormsAuthentication.LoginUrl;// +redirectUrl;
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
                base.OnActionExecuting(filterContext);
            }
            else
            {
                var authenticated = false;
                if (controller.UserSecurityObject.Roles != null)
                {
                    if (controller.UserSecurityObject.obj != null || controller.UserSecurityObject.Roles != null)
                    {
                        foreach (var thisObj in controller.UserSecurityObject.Roles)
                        {
                            var securityObjectSso = (RoleSSO) thisObj;
                            if (securityObjectSso == Role)
                            {
                                authenticated = true;
                            }

                        }
                        if (!authenticated)
                        {
                            filterContext.Controller.TempData["Message"] = "Access Denied";
                            filterContext.Result = new HttpUnauthorizedResult();
                        }
                    }
                }
                else
                {
                    controller.Logout();
                    var loginUrl = FormsAuthentication.LoginUrl;// +redirectUrl;
                    filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.HttpContext.Response.Redirect(loginUrl, true);
                    base.OnActionExecuting(filterContext);
                }
            }

           
        }
    }
}