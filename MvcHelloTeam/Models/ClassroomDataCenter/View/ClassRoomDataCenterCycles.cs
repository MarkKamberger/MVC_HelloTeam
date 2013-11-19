using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberCenter20NS.Models
{
    public class ClassRoomDataCenterCycles : BaseInputModel
    {
        private readonly DataTable _dtClassroom;
        private readonly DataTable _dtGradingPeriod;
        private readonly DataTable _dtSchoolYear;
        private readonly DataTable _classLessonList;
        public ClassRoomDataCenterCycles(DataTable dtClassrooms, DataTable dtGradingPeriod, DataTable dtSchoolYear, DataTable dtClassLessonList)
        {
            _dtClassroom = dtClassrooms;
            _dtGradingPeriod = dtGradingPeriod;
            _dtSchoolYear = dtSchoolYear;
            _classLessonList = dtClassLessonList;
        }
        public DropDownListViewModel.ClassRoomsDropList Classrooms { get { return new DropDownListViewModel.ClassRoomsDropList(_dtClassroom); } }
        public DropDownListViewModel.GradingPeriodsDropList GradingPeriod {get{return new DropDownListViewModel.GradingPeriodsDropList(_dtGradingPeriod);}}
        public DropDownListViewModel.SchoolYearDropList SchoolYears { get { return new DropDownListViewModel.SchoolYearDropList(_dtSchoolYear); } }
        public ClassLessonListModel ClassLessonList { get{return new ClassLessonListModel(_classLessonList); } }
       
    }
    
}