using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPrototype.Models
{
    public class LoginResponseModel
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

    }
}