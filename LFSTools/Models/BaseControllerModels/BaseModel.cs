using System.Collections.Generic;
using DomainLayer;
using DomainLayer.NavigationModels;
using LFSTools.Models;
using LFSTools.Models.Security;
using SALIBusinessLogic;
using SALISecurityObjects;
using SFAFGlobalObjects;
namespace LFSTools.BaseModels
{
    public class BaseModel
    {
        public BaseModel()
        {
            NavigationLinks = new List<_Mvc_ListNavigationLinks>();
        }
        public string SFAFAppName { get;  set; }
        public bool ENABLE_GLOBAL_CACHE { get; set; }
        public ISFAFPresentation ISfafPresentation { get; set; }
        public SALIMainBusinessLogic BusinessLogicObject { get; set; }
        public ClientModel ClientModel { get; set; }
        public IList<_Mvc_ListNavigationLinks> NavigationLinks { get; set; }
        public StrongSecurityObject UserSecurityObject { get; set; }

    }

}