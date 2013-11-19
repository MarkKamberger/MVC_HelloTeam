using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using DomainLayer.SecurityModels;
using SharpArch.NHibernate;

namespace Infrastructure.SecutiryRepository
{
    public class _SALI_LoginWebUserRepository : LinqRepository<_SALI_LoginWebUser>, I_SALI_LoginWebUserRepository
    {
        public List<_SALI_LoginWebUser> Login(string userName, string password, bool isDemo)
        {
            string sql = "EXEC _SALI_LoginWebUser :@UserName, :@Password,:@IsDemo";

            return (List<_SALI_LoginWebUser>) Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof (_SALI_LoginWebUser))
                                 .SetParameter("@UserName", userName)
                                 .SetParameter("@Password", password)
                                 .SetParameter("@IsDemo", isDemo)
                                 .List<_SALI_LoginWebUser>();
            /*.Select(x => new _SALI_LoginWebUser
                        {
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName,
                            UserType =  x.UserType

                        }).ToList();*/

        }
    }
}
