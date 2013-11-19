using MvcPrototype.Models.Security;
using SALIBusinessLogic;
using SALISecurityObjects;
using SFAFGlobalObjects;
namespace MvcPrototype.BaseModels
{
    public class BaseModel
    {
        public string SFAFAppName { get;  set; }
        public bool ENABLE_GLOBAL_CACHE { get; set; }
        public ISFAFPresentation ISfafPresentation { get; set; }
        public SALIMainBusinessLogic BusinessLogicObject { get; set; }
        public ClientModel ClientModel { get; set; }
    }

}