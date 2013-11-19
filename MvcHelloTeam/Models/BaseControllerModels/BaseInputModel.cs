using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcHelloTeam.Models;
using SALIBusinessLogic;
using SFAFGlobalObjects;

namespace MemberCenter20NS.Models
{
    public class BaseInputModel
    {
        public string SFAFAppName { get;  set; }
        public bool ENABLE_GLOBAL_CACHE { get; set; }
        public ISFAFPresentation ISfafPresentation { get; set; }
        public SALIMainBusinessLogic BusinessLogicObject { get; set; }
        public ClientModel ClientModel { get; set; }
        
  
    }

}