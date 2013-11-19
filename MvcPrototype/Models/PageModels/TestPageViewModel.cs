using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLayer;

namespace MvcPrototype.Models
{
    public class TestPageViewModel
    {
        public IList<RoleSSO> Roles { get; set; }
        public IList<SecurityObjectSSO> Object { get; set; }

    }
}