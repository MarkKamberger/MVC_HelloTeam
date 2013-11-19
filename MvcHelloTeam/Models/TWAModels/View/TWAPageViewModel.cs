using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLayer.TWADataModels;
using MemberCenter20NS.Models;

namespace MvcHelloTeam.Models.TWAModels.View
{
    public class TWAPageViewModel :BaseInputModel
    {
        private readonly IList<TWAActivity2Student> _twaActivity2Student;
        public TWAPageViewModel(IList<TWAActivity2Student> studentActivity)
        {
            _twaActivity2Student = studentActivity;
           TwaStudentActivityListViewModel = new TWAStudentActivityListViewModel(_twaActivity2Student);
        }
        public TWAStudentActivityListViewModel TwaStudentActivityListViewModel { get; set; }
        
    }
}