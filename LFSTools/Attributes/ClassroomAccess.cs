using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SALI;
using SFABase;


namespace LFSTools
{
    public class ClassroomAccess : ActionFilterAttribute
    {

        public int ClassroomAssignmentId { get; set; }
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //redirect if not authenticated
            var controller = filterContext.Controller as BaseController;
            
                
                Security s = new Security(controller.CurrentUser.UserName, DataAccessTypes.NetworkDatabase, controller.Session.SessionID);

                if (!s.UserHasAccessToClassroom(ClassroomAssignmentId))
                {
                    //string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;
                    filterContext.Result = new HttpUnauthorizedResult();
                    //Do Nothing
                    /*string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess);
                    string loginUrl = FormsAuthentication.LoginUrl;// +redirectUrl;
                    filterContext.Result = new HttpUnauthorizedResult();
                    filterContext.HttpContext.Response.Redirect(loginUrl, true); */
                }
            

            base.OnActionExecuting(filterContext);
        }
    }
}