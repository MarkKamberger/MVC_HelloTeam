using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.SecurityModels;
using Infrastructure.SecutiryRepository;

namespace Services.SecurityService
{
    public class SecurityService :ISecurityService
    {
        #region Constructor

        private I_SALI_LoginWebUserRepository _loginWebUserRepository;
        public SecurityService(I_SALI_LoginWebUserRepository loginWebUserRepository)
        {
            _loginWebUserRepository = loginWebUserRepository;
        }
        #endregion
        public IList<_SALI_LoginWebUser> LoginWebUser(string userName, string password, bool isDemo)
        {
            var result = _loginWebUserRepository.Login(userName, password, isDemo);
            return result;
        }
    }
}
