using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LFSTools.Controllers.Error
{
    public class OopsController : Controller
    {
        //
        // GET: /Oops/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsufficientPermission()
        {
            return View();
        }

    }
}
