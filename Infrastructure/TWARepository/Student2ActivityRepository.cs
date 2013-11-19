using System.Collections.Generic;
using System.Linq;
using DomainLayer.TWADataModels;
using NHibernate.Linq;
using SharpArch.NHibernate;

namespace Infrastructure.TWARepository
{
    public class Student2ActivityRepository : LinqRepository<TWAActivity2Student>, IStudent2ActivityRepository
    {
        public IEnumerable<TWAActivity2Student> Search(ActivityMasteryFilter filter, ref int totalRecords)
        {
            TWAActivity2Student studentAlias = null;
            var query = Session.Query<TWAActivity2Student>();
           
            if (filter.ActivityId.HasValue)
            {
                query = query.Where(s => s.TWAActivityId == filter.ActivityId.Value);
            }
            if (filter.Activity2StudentId.HasValue)
            {
                query = query.Where(s => s.TWAActivity2StudentId == filter.Activity2StudentId.Value);
            }
            if (filter.StudentId.HasValue)
            {
                query = query.Where(s => s.StudentId == filter.StudentId);
            }
            //allRecords = query.Count();
            return query.Take(totalRecords).ToList();
        }

        public IList<TWAActivity2Student> Search(ActivityMasteryFilter filter)
        {
            TWAActivity2Student studentAlias = null;
            var query = Session.Query<TWAActivity2Student>();

            if (filter.ActivityId.HasValue)
            {
                query = query.Where(s => s.TWAActivityId == filter.ActivityId.Value);
            }
            if (filter.Activity2StudentId.HasValue)
            {
                query = query.Where(s => s.TWAActivity2StudentId == filter.Activity2StudentId.Value);
            }
            if (filter.StudentId.HasValue)
            {
                query = query.Where(s => s.StudentId == filter.StudentId);
            }
            return query.ToList();
        }
       

     
    }
}
