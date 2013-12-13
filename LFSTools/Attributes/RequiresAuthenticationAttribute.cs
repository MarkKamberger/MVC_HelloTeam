using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;



namespace LFSTools
{
    /// <summary>
    /// Verifies that user is logged into system.
    /// </summary>
    public class RequiresAuthenticationAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //redirect if not authenticated
            var controller = filterContext.Controller as BaseController;
            bool failAuthentication = !filterContext.HttpContext.User.Identity.IsAuthenticated && (controller == null || controller.CurrentUser == null) || controller.UserSecurityObject.obj == null;


            if (failAuthentication)
            {
                controller.Logout();
                //use the current url for the redirect
                string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;

                //send them off to the login page
                string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                string loginUrl = FormsAuthentication.LoginUrl;// +redirectUrl;
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}