using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace LFSTools.Attributes
{
    public class RequiresAuthenticationAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        /// <summary>
        /// Called when [RequiresAuthentication].
        /// </summary>
        /// <param name="authorizationContext">The authorization context.</param>
        /// <remarks>Using System.Web.Mvc.AuthorizeAttribute for non API controllers fires before ActionFilterAttribute
        /// This is used only for session authentication.  Inheriting ActionFilterAttribute fires later on in execution pipe 
        /// and should be used for role/scope/priveledge checks
        /// If the request is ajax then retun a json model with the status/error else redirect to login page </remarks>
        public override void OnAuthorization(AuthorizationContext authorizationContext)
        {
            //redirect if not authenticated
            var controller = authorizationContext.Controller as BaseController;
   
            var failAuthentication = !authorizationContext.HttpContext.User.Identity.IsAuthenticated;
            if (!failAuthentication) return;
            if (controller != null)
            {
                controller.Logout();
            }
            if (authorizationContext.HttpContext.Request.IsAjaxRequest())
            {
                authorizationContext.Result = new JsonResult { Data = new { LogonRequired = true, Message = "Session timed out due to inactivity ", Success = false, Url = FormsAuthentication.LoginUrl }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var loginUrl = FormsAuthentication.LoginUrl;// +redirectUrl;
                authorizationContext.Result = new HttpUnauthorizedResult();
                authorizationContext.HttpContext.Response.Redirect(loginUrl, true);
            }
            
        }
        
    }
}