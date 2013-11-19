using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DomainLayer;
using DomainLayer.Helper;
using SALISecurityObjects;

namespace MvcPrototype
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
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool authenticated = false;
            var controller = filterContext.Controller as BaseController;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string loginUrl = FormsAuthentication.LoginUrl;
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
            else
            {
                foreach (var thisObj in controller.UserSecurityObject.Roles)
                {
                    var securityObjectSso = (RoleSSO)thisObj;
                    if (securityObjectSso == Role)
                    {
                        authenticated = true;
                    }
                   
                }
                if (!authenticated)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }
    }
}