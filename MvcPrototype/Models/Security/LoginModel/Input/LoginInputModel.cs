using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPrototype.BaseModels;


namespace MvcPrototype.Models
{
    public class LoginInputModel : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}