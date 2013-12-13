using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LFSTools.BaseModels;


namespace LFSTools.Models
{
    public class LoginInputModel : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}