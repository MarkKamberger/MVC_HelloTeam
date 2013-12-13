using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SALISecurityObjects;
using SFAFGlobalObjects;

namespace LFSTools.Models.Security
{
    public class UserPermissionModel
    {
        public int CustomerId { get; set; }
        public SALIUserTypes UserTypes { get; set; }
        public string SecurityObjectName { get; set; }
        public CustomerRoles CustomerRoles { get; set; }
        public CustomerObject CustomerObject { get; set; }
        public UserPermission UserPermission { get; set; }
        public UserRole UserRole { get; set; }
        
    }
}