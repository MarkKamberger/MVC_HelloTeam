﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainLayer;
using Infrastructure.ApplicationRepository;
using Infrastructure.TWARepository;
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
             : base(ServiceFactory.CreateTWAService())
         {
            _twaService = twaService;

        }

        [RequiresAuthentication]
        public ActionResult Index()
        {
            return View();
        }

    }
}
