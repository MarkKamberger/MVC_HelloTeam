using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.TWADataModels;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;

namespace Infrastructure.TWARepository
{
   public  interface IStudent2ActivityRepository : ILinqRepositoryWithTypedId<TWAActivity2Student,int>
    {
        /// <summary>
        /// Searches the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns></returns>
        IEnumerable<TWAActivity2Student> Search(ActivityMasteryFilter filter, ref int totalRecords);

        /// <summary>
        /// Searches the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IList<TWAActivity2Student> Search(ActivityMasteryFilter filter);



    }
}
