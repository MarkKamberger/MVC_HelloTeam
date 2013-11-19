using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.SecurityModels;

namespace Services.SecurityService
{
    public interface ISecurityService
    {
        IList<_SALI_LoginWebUser> LoginWebUser(string userName, string password, bool isDemo);
    }
}
