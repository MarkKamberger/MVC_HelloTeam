using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberCenter20NS.Models
{
    public class DropDownListViewModel
    {
       
        public class GradingPeriodsDropList
        {
            private readonly DataTable _dataTable;
            private int selectedID;
            private int gradingPeriodID;
            public GradingPeriodsDropList(DataTable gradingPeriodDT)
            {
                _dataTable = gradingPeriodDT;
            }
            public int SelectedID { get { return selectedID; } set {selectedID = value; } }
            public int GradingPerioID { get { return gradingPeriodID; } set {gradingPeriodID = value; } }
            public IEnumerable<SelectListItem> GradingPeriods
            {
                get
                {
                    IEnumerable<SelectListItem> items = _dataTable.AsEnumerable().Select(g => new SelectListItem
                        {
                            Value = g[0].ToString(),
                            Text = g[1].ToString()
                        });
                    return items;
                }
            }
        }

        public class SchoolYearDropList
        {
            private readonly DataTable _dataTable;
            private int selectedID;
            private int schoolYearID;
            public SchoolYearDropList(DataTable schoolYearDT)
            {
                _dataTable = schoolYearDT;
            }
            public int SelectedID { get { return selectedID; } set { schoolYearID = value; } }
            public int SchoolYearID { get { return schoolYearID; } set { schoolYearID = value; } }
            public IEnumerable<SelectListItem> SchoolYears
            {
                get
                {

                    IEnumerable<SelectListItem> items = _dataTable.AsEnumerable().Select(g => new SelectListItem
                        {
                            Value = g[0].ToString(),
                            Text = g[1].ToString()
                        });
                    return items;
                }
            }
        }

        public class ClassRoomsDropList
        {
            private readonly DataTable _dataTable;
            private int selectedID;
            private int classRoomID;
            public ClassRoomsDropList(DataTable classroomDT)
            {
                _dataTable = classroomDT;
            }
            public int SelectedID { get { return selectedID; } set { selectedID = value; } }
            public int ClassRoomID { get { return classRoomID; } set { classRoomID = value; } }
            public IEnumerable<SelectListItem> Classrooms
            {
                get
                {


                    IEnumerable<SelectListItem> items = _dataTable.AsEnumerable().Select(g => new SelectListItem
                        {
                            Value = g[0].ToString(),
                            Text = g[1].ToString()
                        });


                    return items;
                }
            }
        }
    }
}