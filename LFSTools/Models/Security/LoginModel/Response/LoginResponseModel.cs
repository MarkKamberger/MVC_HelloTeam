using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LFSTools.BaseModels;

namespace LFSTools.Models
{
    public class LoginResponseModel : BaseModel
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public new bool Success { get; set; }

    }
}