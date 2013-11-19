using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SALI;
using SFABase;


namespace MvcPrototype
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
            if (controller.IsDemo)
            {
                
            }
            else
            {
                
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
            }

            base.OnActionExecuting(filterContext);
        }



        

      /*  public bool UserHasAccessToLesson(int lessonId)
        {
            bool result = false;

            if (BondedMasterPage.BusinessLogicObject.IsDemo)
                result = true;
            else
            {
                Security s = new Security(BondedMasterPage.CurrentUserName, AccessType, BondedMasterPage.CurrentSessionId);

                result = s.UserHasAccessToLesson(lessonId);
            }

            return result;
        }

        public bool UserHasAccessToCustomer(int customerId)
        {
            bool result = false;

            if (BondedMasterPage.BusinessLogicObject.IsDemo)
                result = true;
            else
            {
                Security s = new Security(BondedMasterPage.CurrentUserName, AccessType, BondedMasterPage.CurrentSessionId);

                result = s.UserHasAccessToCustomer(customerId);
            }


            return result;
        }

        public bool IsTestId4Sight(int testId)
        {
            bool result = false;

            string[] ids = ConfigurationManager.AppSettings["Current4SightTestIDs"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            for (int x = 0; x < ids.Length; x++)
            {
                if (NumberHelpers.ReturnAsInt(ids[x]) == testId)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }*/
    }
}