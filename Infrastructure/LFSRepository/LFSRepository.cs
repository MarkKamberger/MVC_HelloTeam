using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.LFSTools;
using DomainLayer.NavigationModels;
using NHibernate.Linq;
using SharpArch.NHibernate;

namespace Infrastructure.LFSRepository
{
   public class LFSRepository : LinqRepository<LFSGuideTypes>, ILFSRepository
    {
       public List<LFSGuideTypes> GetLFSGuideTypes()
        {
            var query = Session.Query<LFSGuideTypes>();
            return query.ToList();
        }
    }
}
