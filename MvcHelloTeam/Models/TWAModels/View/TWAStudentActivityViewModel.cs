using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLayer.TWADataModels;
using MvcHelloTeam.Models.TWAModels.View;

namespace MvcHelloTeam.Models.TWAModels
{
    public class TWAStudentActivityViewModel
    {
        private readonly TWAActivity2Student _twaActivity2Student;
        public TWAStudentActivityViewModel(TWAActivity2Student twaActivity2Student)
        {
            _twaActivity2Student = twaActivity2Student;
        }
        public int ActivityId { get { return _twaActivity2Student.TWAActivityId; }}
        public DateTime DateLastAssessed { get { return _twaActivity2Student.LastDateAssessed; } }
        public IList<TWAActivity2StudentLevelMastery> TwaActivity2StudentLevelMasterys { get { return _twaActivity2Student.TWAActivity2StudentLevelMastery; } }
        public TWATutoringActivitiesTemplateActivity TwaTutoringActivitiesTemplateActivity { get { return _twaActivity2Student.TwaTutoringActivitiesTemplateActivity; } }
       
       

    }
}