using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.NavigationModels;
using DomainLayer.SecurityModels;
using Infrastructure.SecutiryRepository;
using SharpArch.NHibernate;

namespace Infrastructure.ApplicationRepository
{
    public class ListNavigationLinksRepository : LinqRepository<_Mvc_ListNavigationLinks>, IListNavigationLinksRepository
    {
        /// <summary>
        /// Lists the navigation links.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationLinks> ListNavigationLinks(int applicationId)
        {
            const string sql = "EXEC _Mvc_ListNavigationLinks :@ApplicationId";

            return (List<_Mvc_ListNavigationLinks>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationLinks))
                                 .SetParameter("@ApplicationId", applicationId)
                                 .List<_Mvc_ListNavigationLinks>();
        }

        /// <summary>
        /// Lists the navigation children.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navId">The nav id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationChild> ListNavigationChildren(int applicationId, int navId)
        {
            const string sql = "EXEC _Mvc_ListNavigationChild :@ApplicationId, :@NavId";

            return (List<_Mvc_ListNavigationChild>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationChild))
                                 .SetParameter("@ApplicationId", applicationId)
                                 .SetParameter("@NavId", navId)
                                 .List<_Mvc_ListNavigationChild>();
        }

        /// <summary>
        /// Lists the navigation special customers.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navId">The nav id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationSpecialCustomer> ListNavigationSpecialCustomers(int applicationId, int navId)
        {
            const string sql = "EXEC _Mvc_ListNavigationSpecialCustomer :@ApplicationId, :@NavId";

            return (List<_Mvc_ListNavigationSpecialCustomer>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationSpecialCustomer))
                                 .SetParameter("@ApplicationId", applicationId)
                                   .SetParameter("@NavId", navId)
                                 .List<_Mvc_ListNavigationSpecialCustomer>();
        }

        /// <summary>
        /// Lists the navigation special users.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navId">The nav id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationSpecialUser> ListNavigationSpecialUsers(int applicationId, int navId)
        {
            const string sql = "EXEC _Mvc_ListNavigationSpecialUser :@ApplicationId, :@NavId";

            return (List<_Mvc_ListNavigationSpecialUser>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationSpecialUser))
                                 .SetParameter("@ApplicationId", applicationId)
                                   .SetParameter("@NavId", navId)
                                 .List<_Mvc_ListNavigationSpecialUser>();
        }

        /// <summary>
        /// Lists the navigation child special customers.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navChildId">The nav child id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationChildSpecialCustomer> ListNavigationChildSpecialCustomers(int applicationId, int navChildId)
        {
            const string sql = "EXEC _Mvc_ListNavigationChildSpecialCustomer :@ApplicationId, :@NavChildId";

            return (List<_Mvc_ListNavigationChildSpecialCustomer>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationChildSpecialCustomer))
                                 .SetParameter("@ApplicationId", applicationId)
                                   .SetParameter("@NavChildId", navChildId)
                                 .List<_Mvc_ListNavigationChildSpecialCustomer>();
        }

        /// <summary>
        /// Lists the navigation child special users.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navChildId">The nav child id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationChildSpecialUser> ListNavigationChildSpecialUsers(int applicationId, int navChildId)
        {
            const string sql = "EXEC _Mvc_ListNavigationChildSpecialUser :@ApplicationId, :@NavChildId";

            return (List<_Mvc_ListNavigationChildSpecialUser>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationChildSpecialUser))
                                 .SetParameter("@ApplicationId", applicationId)
                                   .SetParameter("@NavChildId", navChildId)
                                 .List<_Mvc_ListNavigationChildSpecialUser>();
        }

        /// <summary>
        /// Lists the navigation child roles.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navChildId">The nav child id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationChildRole> ListNavigationChildRoles(int applicationId, int navChildId)
        {
            const string sql = "EXEC _Mvc_ListNavigationChildRole :@ApplicationId, :@NavChildId";

            return (List<_Mvc_ListNavigationChildRole>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationChildRole))
                                 .SetParameter("@ApplicationId", applicationId)
                                   .SetParameter("@NavChildId", navChildId)
                                 .List<_Mvc_ListNavigationChildRole>();
        }

        /// <summary>
        /// Lists the navigation roles.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navchildId">The navchild id.</param>
        /// <returns></returns>
        public List<_Mvc_ListNavigationRole> ListNavigationRoles(int applicationId, int navId)
        {
            const string sql = "EXEC _Mvc_ListNavigationRole :@ApplicationId, :@NavChildId";

            return (List<_Mvc_ListNavigationRole>)Session.CreateSQLQuery(sql)
                                 .AddEntity(typeof(_Mvc_ListNavigationRole))
                                 .SetParameter("@ApplicationId", applicationId)
                                   .SetParameter("@NavChildId", navId)
                                 .List<_Mvc_ListNavigationRole>();
        }
    }
}
