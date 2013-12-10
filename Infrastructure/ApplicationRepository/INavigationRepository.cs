using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.NavigationModels;
using SharpArch.Domain.PersistenceSupport;

namespace Infrastructure.ApplicationRepository
{

    public interface INavigationLinkOrmRepository : ILinqRepositoryWithTypedId<NavigationLink, int>
    {
        List<NavigationLink> ListNavigationLinks(int applicationId);
      
    }

    public interface IListNavigationLinksRepository : ILinqRepositoryWithTypedId<_Mvc_ListNavigationLinks, int>
    {
        /// <summary>
        /// Lists the navigation links.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationLinks> ListNavigationLinks(int applicationId);

        /// <summary>
        /// Lists the navigation children.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navId">The nav id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationChild> ListNavigationChildren(int applicationId, int navId);

        /// <summary>
        /// Lists the navigation special customers.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navId">The nav id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationSpecialCustomer> ListNavigationSpecialCustomers(int applicationId, int navId);

        /// <summary>
        /// Lists the navigation special users.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navId">The nav id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationSpecialUser> ListNavigationSpecialUsers(int applicationId, int navId);

        /// <summary>
        /// Lists the navigation child special customers.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navChildId">The nav child id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationChildSpecialCustomer> ListNavigationChildSpecialCustomers(int applicationId, int navChildId);

        /// <summary>
        /// Lists the navigation child special users.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navChildId">The nav child id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationChildSpecialUser> ListNavigationChildSpecialUsers(int applicationId, int navChildId);

        /// <summary>
        /// Lists the navigation roles.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navId">The nav id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationRole> ListNavigationRoles(int applicationId, int navId);

        /// <summary>
        /// Lists the navigation child roles.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="navchildId">The navchild id.</param>
        /// <returns></returns>
        List<_Mvc_ListNavigationChildRole> ListNavigationChildRoles(int applicationId, int navchildId);



    }
}
