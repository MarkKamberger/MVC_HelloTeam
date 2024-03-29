﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPrototype.BaseModels;

namespace MvcPrototype.Models
{
    public class LoginResponseModel : BaseModel
    {
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

    }
}