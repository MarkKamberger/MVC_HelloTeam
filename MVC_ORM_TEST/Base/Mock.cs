using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer;

namespace MVC_ORM_TEST.Base
{
    public class Mock
    {
        public static StrongSecurityObject MockStrongSecurityObject()
        {
           
            var securityObject = new StrongSecurityObject
                {
                    obj = new List<SecurityObjectSSO>(),
                    Roles = new List<RoleSSO>()
                };
            securityObject.Roles.Add(RoleSSO.FullAccess);
            securityObject.obj.Add(new SecurityObjectSSO
                {
                    Object = ObjectsSSO.LFS,
                    Privilege = PrivilegeSSO.SELECT,
                    Scope = ScopeSSO.SELF
                });

            return securityObject;

        }
    }
}
