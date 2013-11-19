using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcPrototype.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [RequiresAuthentication]
        public ActionResult Index()
        {
            return View();
        }

    }
}
