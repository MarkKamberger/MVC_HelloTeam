using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer;
using DomainLayer.NavigationModels;
using DomainLayer.TWADataModels;

namespace Services
{
    public interface ITWAService
    {
        /// <summary>
        /// Saves the mastery.
        /// </summary>
        /// <param name="twaActivity2Student">The twa activity2 student.</param>
        void SaveMastery(TWAActivity2Student twaActivity2Student);

        /// <summary>
        /// Gets the mastery details.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IList<TWAActivity2Student> GetMasteryDetails(ActivityMasteryFilter filter);

        /// <summary>
        /// Lises the navigation links.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="sso">The sso.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns></returns>
        IList<_Mvc_ListNavigationLinks> LisNavigationLinks(int applicationId, StrongSecurityObject sso, int  userId, int  customerId);


    }
}
