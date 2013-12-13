using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainLayer;
using Infrastructure.ApplicationRepository;
using Infrastructure.TWARepository;
using LFSTools.Models;
using LFSTools.Providers;
using SALISecurityObjects;
using Services;

namespace LFSTools.Controllers
{
    public class HiberhateController : BaseController
    {
        //
        // GET: /Hiberhate/
         private readonly StrongSecurityObject securityObject;
         private readonly ITWAService _twaService;
         public HiberhateController(ITWAService twaService) : base(ServiceFactory.CreateTWAService())
         {
            _twaService = twaService;
             securityObject = new StrongSecurityObject();
        }

        [RequiresRoleAttribute(Role = RoleSSO.MemberCenterUser)]
        public ActionResult Index()
        {
            return View();
        }

        [RequiresRoleAttribute(Role = RoleSSO.MemberCenterUser)]
        public ActionResult TestRole()
        {
            var vm = new TestPageViewModel
                {
                    Roles = UserSecurityObject.Roles,
                    Object = UserSecurityObject.obj

                };
            return View(vm);
        }

    }
}
