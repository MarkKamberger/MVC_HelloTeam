using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLayer.TWADataModels;
using MvcHelloTeam.Models.BaseControllerModels;

namespace MvcHelloTeam.Models.TWAModels.View
{
    public class TWAStudentActivityListViewModel: BaseViewModel
    {
        public TWAStudentActivityListViewModel(IList<TWAActivity2Student> studentActivity)
        {
            StudentActivity = new List<TWAStudentActivityViewModel>();
            foreach (var activity2Student in studentActivity)
            {
                StudentActivity.Add(new TWAStudentActivityViewModel(activity2Student));
            }
        }
        public IList<TWAStudentActivityViewModel> StudentActivity { get; set; } 
    }
}