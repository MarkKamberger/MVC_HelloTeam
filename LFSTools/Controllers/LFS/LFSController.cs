using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainLayer;
using DomainLayer.LFSTools;
using LFSTools.Models.GenericModels;
using LFSTools.Models.LFSModels;
using LFSTools.Providers;
using Services.LFSService;

namespace LFSTools.Controllers.LFS
{
    public class LFSController : BaseController
    {
        //
        // GET: /LFS/
        private ILFSService _lfsService;
        public LFSController(ILFSService lfsService) : base(ServiceFactory.CreateTWAService())
        {
            _lfsService = lfsService;
        }

        [RequiresAuthentication, RequiresRole(Role = RoleSSO.MemberCenterUser)]
        public ActionResult Index()
        {
            var vm = new LfsViewModel {GuideTypes = {SelectList =  _lfsService.ListLFSGuideTypes().AsEnumerable()}};
            return View("LFSEdit",vm);
        }

        [AcceptVerbs(HttpVerbs.Get),RequiresAuthentication, RequiresRole(Role = RoleSSO.MemberCenterUser)]
        public ActionResult GetDrillDownData(string type,int Id)
        {
            var vm = new LfsViewModel();
            var filter = new LfsQueryFilter();
            switch (type)
            {
                case "guideType":
                    filter.CategoryId = Id;
                    vm.SelectList.SelectList =
                        _lfsService.ListAreaOfConcern(filter).Select(x => new EnumerableSelectList
                            {
                                Active = x.Active,
                                Description = x.AreaOfConcernDesc,
                                Id = x.Id

                            }).AsEnumerable();
                    break;
                case "areaOfConcern":
                   filter.AreaOfConcernId = Id;
                    vm.SelectList.SelectList =
                        _lfsService.ListSubAreaOfConcern(filter).Select(x => new EnumerableSelectList
                            {
                                Active = x.Active,
                                Description = x.SubAreaOfConcernDesc,
                                Id = x.Id

                            }).AsEnumerable();
                    break;
                case "subAreaOfConcern":
                    filter.SubAreaOfConcernId = Id;
                    vm.SelectList.SelectList =
                        _lfsService.ListSmartsTarget(filter).Select(x => new EnumerableSelectList
                            {
                                Active = x.Active,
                                Description = x.TargetDesc,
                                Id = x.Id

                            }).AsEnumerable();
                    break;
                case "target":
                    filter.TargetId = Id;
                    vm.SelectList.SelectList =
                        _lfsService.ListRootCause(filter).Select(x => new EnumerableSelectList
                            {
                                Active = x.Active,
                                Description = x.RootCauseDesc,
                                Id = x.Id

                            }).AsEnumerable();
                    break;
                case "rootCause":
                    filter.RootCauseId = Id;
                    vm.SelectList.SelectList =
                        _lfsService.ListLeveragePoint(filter).Select(x => new EnumerableSelectList
                            {
                                Active = x.Active,
                                Description = x.LeveragePointDesc,
                                Id = x.Id

                            }).AsEnumerable();
                    break;
                case "impFocus":
                   filter.LeveragePointId = Id;
                    vm.SelectList.SelectList =
                        _lfsService.ListPlanAction(filter).Select(x => new EnumerableSelectList
                            {
                                Active = x.Active,
                                Description = x.ActionDesc,
                                Id = x.Id

                            }).AsEnumerable();
                    break;
            }
            return Json(vm, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get), RequiresAuthentication, RequiresRole(Role = RoleSSO.MemberCenterUser)]
        public ActionResult saveEdit(string type, int id, string text, bool active)
        {
            var vm = new GenericViewModel(); 
            var filter = new LfsQueryFilter();
            try
            {
                switch (type)
                {
                    case "GuideType":
                        filter.CategoryId = id;
                        break;
                    case "areaOfConcern":
                        var areaObj = _lfsService.GetAreaOfConcern(id);
                        areaObj.AreaOfConcernDesc = text;
                        areaObj.Active = active;
                        _lfsService.SaveAreaOfConcern(areaObj);
                        break;
                    case "subAreaOfConcern":
                        var subAreaobj = _lfsService.GetSubAreaOfConcern(id);
                        subAreaobj.SubAreaOfConcernDesc = text;
                        subAreaobj.Active = active;
                        _lfsService.SaveSubAreaOfConcern(subAreaobj);
                        break;
                    case "target":
                        var targetObj = _lfsService.GetTargetList(id);
                        targetObj.TargetDesc = text;
                        _lfsService.SaveSmartstarget(targetObj);
                        break;
                    case "rotCause":
                        var rootObj = _lfsService.GetRootCause(id);
                        rootObj.RootCauseDesc = text;
                        rootObj.Active = active;
                        _lfsService.SaveRootCause(rootObj);
                        break;
                    case "impFocus":
                        var leverageObj = _lfsService.GetLeveragePoint(id);
                        leverageObj.LeveragePointDesc = text;
                        leverageObj.Active = active;
                        _lfsService.SaveLeveragePoint(leverageObj);
                        break;
                    case "action":
                        var actionObj = _lfsService.GetPlanAction(id);
                        actionObj.ActionDesc = text;
                        actionObj.Active = active;
                        _lfsService.SavePlanAction(actionObj);
                        break;
                }
                vm.Success = true;
            }
            catch (Exception ex)
            {
                vm.Success = false;
                vm.Message = ex.Message;
            }
           
            return Json(vm, JsonRequestBehavior.AllowGet);
        }
    }
}
