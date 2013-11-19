using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MemberCenter20NS.Models;

namespace MvcHelloTeam.Models
{
    public class LoginInputModel : BaseInputModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}