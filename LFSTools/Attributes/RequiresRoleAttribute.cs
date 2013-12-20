using System;
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
            
                var authenticated = false;
                if (controller.UserSecurityObject.Roles != null)
                {
                    if (controller.UserSecurityObject.obj == null && controller.UserSecurityObject.Roles == null) return;
                    foreach (var securityObjectSso in controller.UserSecurityObject.Roles.Select(thisObj => (RoleSSO)thisObj).Where(securityObjectSso => securityObjectSso == Role))
                    {
                        authenticated = true;
                    }
                    if (authenticated) return;
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult { Data = new { LogonRequired = false, Message = "Insufficient Permissions", Success = false, Url = FormsAuthentication.LoginUrl }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        var loginUrl = FormsAuthentication.LoginUrl;// +redirectUrl;
                        filterContext.Result = new HttpUnauthorizedResult();
                        filterContext.HttpContext.Response.Redirect(loginUrl, true);
                    }
                }
                else
                {
                    controller.Logout();
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult { Data = new { LogonRequired = true, Message = "Session timed out due to inactivity", Success = false, Url = FormsAuthentication.LoginUrl }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        var loginUrl = FormsAuthentication.LoginUrl;// +redirectUrl;
                        filterContext.Result = new HttpUnauthorizedResult();
                        filterContext.HttpContext.Response.Redirect(loginUrl, true);
                    }
                }  
          
           
        }
    }
}