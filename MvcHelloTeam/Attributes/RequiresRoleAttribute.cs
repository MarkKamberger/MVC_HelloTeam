﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using SALISecurityObjects;

namespace DotFramework.Web.Mvc.Attributes
{
    /// <summary>
    /// Verifies that user has a specified role or is in a list of roles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property,
                   AllowMultiple = true, Inherited = true)]
    public class RequiresRoleAttribute : ActionFilterAttribute
    {
        public Role[] AllowedRoles { get; set; }

        public RequiresRoleAttribute(Role role)
        {
            AllowedRoles = new Role[] { role };
        }

        public RequiresRoleAttribute(params Role[] roles)
        {
            AllowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool redirect = true;

            //redirect if the user is not authenticated
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                foreach (Role role in AllowedRoles)
                {
                    if (filterContext.HttpContext.User.IsInRole(role.ToString()))
                    {
                        redirect = false;
                    }
                }
            }

            if (redirect)
            {
                //use the current url for the redirect
                string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;

                //send them off to the login page
                string loginUrl = FormsAuthentication.LoginUrl;
                filterContext.Result = new HttpUnauthorizedResult();
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }
}