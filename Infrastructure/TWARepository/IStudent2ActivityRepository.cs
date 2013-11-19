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
        IEnumerable<TWAActivity2Student> Search(ActivityMasteryFilter filter, ref int totalRecords);
        IList<TWAActivity2Student> Search(ActivityMasteryFilter filter);
       
    }
}
