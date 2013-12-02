using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainLayer;

namespace MvcPrototype.Helpers
{
    public static class SecurityPermissionHelper
    {
        public static bool CheckObjectPermission(StrongSecurityObject sso, ObjectsSSO obj,ScopeSSO scope,PrivilegeSSO privilege)
        {
            var retVal = false;
            var thisObject = sso.obj.First(x => x.Object == obj);
            if (thisObject != null)
            {
                if (thisObject.Privilege >= privilege && thisObject.Scope >= scope)
                {
                    retVal = true;
                }
            }
            return retVal;
        }
    }
}