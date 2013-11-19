using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MemberCenter20NS.Models
{
    public class ClassLessonListModel
    {
        public ClassLessonListModel(DataTable classLessons)
        {
            ClassLessons = new List<ClassLessonModel>();
            foreach (DataRow row in classLessons.Rows)
            {
                ClassLessons.Add(new ClassLessonModel(row));
            }
        }
        public IList<ClassLessonModel> ClassLessons { get; set; } 
    }
    public class ClassLessonModel
    {
        private DataRow _dataRow;
        public ClassLessonModel(DataRow dataRow)
        {
            _dataRow = dataRow;
        }
        public string  StartDate { get { return _dataRow[27].ToString(); } }
        public string Source { get { return _dataRow[3].ToString(); } }
        public string LessonCycles { get { return _dataRow[2].ToString(); } }
        public string ReadingObjectives { get { return _dataRow[15].ToString(); } }
        public string StrategyTarget { get { return _dataRow[18].ToString(); } }
        public string SubStrategy { get { return _dataRow[4].ToString(); } }
    }
}