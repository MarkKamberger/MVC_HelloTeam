using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.SecurityModels;
using DomainLayer.TWADataModels;
using SharpArch.Domain.PersistenceSupport;

namespace Infrastructure.SecutiryRepository
{
    public interface I_SALI_LoginWebUserRepository : ILinqRepositoryWithTypedId<_SALI_LoginWebUser, int>
    {
        List<_SALI_LoginWebUser> Login(string userName, string password, bool isDemo);
    }
}
