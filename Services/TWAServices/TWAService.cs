using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer;
using DomainLayer.NavigationModels;
using DomainLayer.TWADataModels;
using Infrastructure.ApplicationRepository;
using Infrastructure.DataMapping;
using Infrastructure.TWARepository;
using Services.Helper;
using SharpArch.NHibernate.Web.Mvc;


namespace Services
{
    public class TWAService : ITWAService
    {
        #region Fields
        private readonly IStudent2ActivityRepository _repository;
        private readonly IListNavigationLinksRepository _navigationLinksRepository;
        private readonly INavigationLinkOrmRepository _navigationLinkOrm;
        
        #endregion

        #region Constructor
        public TWAService(IStudent2ActivityRepository repository
            , IListNavigationLinksRepository navigationLinksRepository, INavigationLinkOrmRepository navigationLinkOrm)
        {
            _repository = repository;
            _navigationLinksRepository = navigationLinksRepository;
            _navigationLinkOrm = navigationLinkOrm;

        }
        #endregion


        /// <summary>
        /// Saves the mastery.
        /// </summary>
        /// <param name="twaActivity2Student">The twa activity2 student.</param>
        public void SaveMastery(TWAActivity2Student twaActivity2Student)
        {
           _repository.Save(twaActivity2Student);
        }

        /// <summary>
        /// Gets the mastery details.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public IList<TWAActivity2Student> GetMasteryDetails(ActivityMasteryFilter filter)
        {
            var student2Activities = _repository.Search(filter).ToList();
            return student2Activities;
        }

    

        /// <summary>
        /// Lises the navigation links.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="sso">The sso.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>List of links and dropdown lists that are allowed by user Role/Object & Special Permissions</returns>
        public IList<_Mvc_ListNavigationLinks> LisNavigationLinks(int applicationId, StrongSecurityObject sso, int  userId, int  customerId)
        {
            var navList = _navigationLinksRepository.ListNavigationLinks(applicationId);
            var returnList = new List<_Mvc_ListNavigationLinks>();
            foreach (_Mvc_ListNavigationLinks navItem in navList)
            {
                var itemObject = Helper.HelperClass.ParseEnum<ObjectsSSO>(navItem.Object);
                var obj = sso.obj.Any(x => x.Object == itemObject);
                var specialUsers = _navigationLinksRepository.ListNavigationSpecialUsers(applicationId, navItem.Id);
                var specialCustomers = _navigationLinksRepository.ListNavigationSpecialCustomers(applicationId, navItem.Id);
                var navChildren = _navigationLinksRepository.ListNavigationChildren(applicationId, navItem.Id);
                var role = _navigationLinksRepository.ListNavigationRoles(applicationId, navItem.Id);
                var inRole = sso.Roles.Any(sr => role.Any());
                var addItem = itemObject == ObjectsSSO.All;
                if (inRole)
                {
                    addItem = true;
                }
                if (specialCustomers.Count > 0)
                {
                    var addForCustomer = specialCustomers.Any(x => x.CustomerId == customerId);
                    if (!addItem && addForCustomer)
                    {
                        addItem = true;
                    }
                }
                if (specialUsers.Count > 0)
                {
                    var addForUser = specialUsers.Any(x => x.UserId == userId);
                    if (!addItem && addForUser)
                    {
                        addItem = true;
                    }
                }
                if (obj && !addItem)
                {
                    addItem = true;
                }
                if (addItem)
                {
                     var allowedNavChildren = new List<_Mvc_ListNavigationChild>();
                    if (navChildren.Count > 0)
                    {
                        foreach (_Mvc_ListNavigationChild child in navChildren)
                        {
                            var childRole = _navigationLinksRepository.ListNavigationChildRoles(1, child.Id);
                            var inChildRole = sso.Roles.Any(sr => childRole.Any());
                            var childItemObject = HelperClass.ParseEnum<ObjectsSSO>(child.Object);
                            var childObject = sso.obj.Any(x => x.Object == childItemObject);
                            var specialChildUsers = _navigationLinksRepository.ListNavigationChildSpecialUsers(1,
                                                                                                               child.Id);
                            var specialChildCustomers = _navigationLinksRepository.ListNavigationChildSpecialCustomers(1,
                                                                                                                  child
                                                                                                                      .Id);
                            child.SpecialCustomers = specialChildCustomers;
                            child.SpecialUsers = specialChildUsers;
                            var addChildItem = childItemObject == ObjectsSSO.All;
                            if (inChildRole)
                            {
                                addChildItem = true;
                            }
                            if (specialChildCustomers.Count > 0)
                            {
                                var addForCustomer = specialChildCustomers.Any(x => x.CustomerId == customerId);
                                if (!addChildItem && addForCustomer)
                                {
                                    addChildItem = true;
                                }
                            }
                            if (specialChildUsers.Count > 0)
                            {
                                var addForUser = specialChildUsers.Any(x => x.UserId == userId);
                                if (!addChildItem && addForUser)
                                {
                                    addChildItem = true;
                                }
                            }
                            if (childObject && !addChildItem)
                            {
                                addChildItem = true;
                            }
                            if (addChildItem)
                            {
                                allowedNavChildren.Add(child);
                            }
                        }
                    } 
                    var links = new _Mvc_ListNavigationLinks
                    {
                        Name = navItem.Name,
                        Object = navItem.Object,
                        Url = navItem.Url,
                        TypeId = navItem.TypeId,
                        NavigationChildren = allowedNavChildren,
                        SpecialCustomers = specialCustomers,
                        SpecialUsers = specialUsers
                    };
                    returnList.Add(links);
                }
            }
            return returnList;
        }

        
        public IList<NavigationLink> ListNavigationLinkORM(int applicationId)
        {
            return _navigationLinkOrm.ListNavigationLinks(applicationId);
        }

     
    }
}
