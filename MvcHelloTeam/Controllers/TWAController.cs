using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainLayer.TWADataModels;
using DotFramework.Web.Mvc.Attributes;
using MemberCenter20NS.Controllers;
using MvcHelloTeam.Models;
using MvcHelloTeam.Models.TWAModels.View;
using Services;

namespace MvcHelloTeam.Controllers
{
    public class TWAController : BaseController
    {
       
        private readonly ITWAService _twaService;
        public TWAController(ITWAService twaService)
        {
            _twaService = twaService;
        }
       
        [RequiresAuthentication]
        public ActionResult Index()
        {
            var filter = new ActivityMasteryFilter
            {
                StudentId = 0
            };
            var vm = new TWAPageViewModel(_twaService.GetMasteryDetails(filter));
            return View();
        }

        [RequiresAuthentication]
        public ActionResult GetTwaActivityMastery(int studentId)
        {
            var filter = new ActivityMasteryFilter
                {
                    StudentId = studentId
                };
            var vm = new TWAPageViewModel(_twaService.GetMasteryDetails(filter));
            return View("PartialTwaActivity", vm);
        }

       

    }
}
