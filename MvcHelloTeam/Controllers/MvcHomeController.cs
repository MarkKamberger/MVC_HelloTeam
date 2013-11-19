using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotFramework.Web.Mvc.Attributes;
using MemberCenter20NS.Models;
using SALI;


namespace MemberCenter20NS.Controllers
{
    public class MvcHomeController : BaseController
    {
        //
        // GET: /MvcHome/
        
        public MvcHomeController()
        {
            
        }
         [RequiresAuthentication]
        public ActionResult Index()
        {
 
            return View();

        }
        [RequiresAuthentication]
        public ActionResult Cycles()
        {
            var classrooms = _baseModel.BusinessLogicObject.GetClassrooms(_presenter.CurrentCustomerId, _presenter.CurrentSchoolYearId,
                                                       2, -1, -1);
            var schoolYears = _baseModel.BusinessLogicObject.GetSchoolYears();
            var gradingPeriods = _baseModel.BusinessLogicObject.GetGradingPeriods(_presenter.CurrentCustomerId, 1);

            var c = classrooms.ObjectData;
            var s = (SALI.SALISchoolYearEnum)schoolYears.ActualObject;
            var vm = new ClassRoomDataCenterCycles(classrooms.ObjectData,
                                                  gradingPeriods.ObjectData,
                                                  schoolYears.ObjectData,new DataTable());
            vm.SchoolYears.SelectedID = (int)_presenter.CurrentSchoolYearId;
            vm.GradingPeriod.SelectedID =  (int)_baseModel.BusinessLogicObject.GetCurrentGradingPeriod(_presenter.CurrentCustomerId);
            
            return View(vm);
        }
         [RequiresAuthentication]
        public ActionResult GetClassRooms(int schoolYearID, int gradingPeriodID)
        {
            var classrooms = _baseModel.BusinessLogicObject.GetClassrooms(_presenter.CurrentCustomerId, schoolYearID,
                                                       gradingPeriodID, -1, -1);
            var vm = new DropDownListViewModel.ClassRoomsDropList(classrooms.ObjectData);
                

            return Json(vm,JsonRequestBehavior.AllowGet);
        }
         [RequiresAuthentication]
        public ActionResult GetClassLessons(int? customerId, int schoolYearId, int? schoolTrackId, int gradingPeriodId, int classroomAssignId)
        {
            var _customerId = _presenter.CurrentCustomerId;
            var _schoolTraclId = 0;
            if (customerId.HasValue)
            {
                _customerId = customerId.Value;
            }
            if (schoolTrackId.HasValue)
            {
                _schoolTraclId = schoolTrackId.Value;
            }

            var lessons = _baseModel.BusinessLogicObject.GetClassroomLongLessons(_customerId, schoolYearId, _schoolTraclId,
                                                                                 gradingPeriodId, classroomAssignId);
            var vm = new ClassRoomDataCenterCycles(new DataTable(),new DataTable(),new DataTable(),lessons.ObjectData);
      
            return View("ClassLessons",vm);
        }
    }
}
