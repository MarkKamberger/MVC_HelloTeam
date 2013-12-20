using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainLayer;
using Infrastructure.ApplicationRepository;
using Infrastructure.TWARepository;
using LFSTools.Attributes;
using LFSTools.Providers;
using Services;

namespace LFSTools.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/


         private readonly ITWAService _twaService;
         public HomeController(ITWAService twaService)
             : base(ServiceFactory.CreateSecurityService(), ServiceFactory.CreateTWAService())
         {
            _twaService = twaService;

        }

        [RequiresAuthentication]
        public ActionResult Index()
        {
            if (Request.Browser.IsMobileDevice)
            {
                return View("MobileHome");
            }
            else
            {
                return View();
            }
           
        }

    }
}
