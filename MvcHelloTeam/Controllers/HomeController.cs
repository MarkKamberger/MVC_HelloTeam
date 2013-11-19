using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotFramework.Web.Mvc.Attributes;
using MemberCenter20NS.Controllers;

namespace MvcHelloTeam.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        [RequiresAuthentication]
        public ActionResult Index()
        {
            return View("Index");
        }

    }
}
