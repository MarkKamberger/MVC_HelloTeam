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
    public class LFSGuideTypesRepository : LinqRepository<LFSGuideTypes>, ILFSGuideTypesRepository
    {
        public List<LFSGuideTypes> GetLFSGuideTypes()
        {
            var query = Session.Query<LFSGuideTypes>();
            return query.ToList();
        }
    }
    public class AchievementPlanAreaOfConcernRepository : LinqRepository<AchievementPlanAreaOfConcern>, IAchievementPlanAreaOfConcernRepository
    {
        public List<AchievementPlanAreaOfConcern> ListAreaOfConcern(LfsQueryFilter filter)
        {
            var query = Session.Query<AchievementPlanAreaOfConcern>();
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(s => s.Category.Any(r => r.AchievementPlanCategoryId == filter.CategoryId));
            }
            return query.ToList();
        }

        public AchievementPlanAreaOfConcern GetAreaOfConcern(int Id)
        {
           return Session.Get<AchievementPlanAreaOfConcern>(Id);
        }

        public void SaveAreaOfConcern(AchievementPlanAreaOfConcern areaOfConcern)
        {
            Session.SaveOrUpdate(areaOfConcern);
            Session.Flush();
          
        }
    }
    public class AchievementPlanSubAreaOfConcernRepository : LinqRepository<AchievementPlanSubAreaOfConcern>, IAchievementPlanSubAreaOfConcernRepository
    {
        public List<AchievementPlanSubAreaOfConcern> ListSubAreaOfConcern(LfsQueryFilter filter)
        {
            var query = Session.Query<AchievementPlanSubAreaOfConcern>();
            if (filter.AreaOfConcernId.HasValue)
            {
                query = query.Where(s => s.AreaOfConcern.Any(r => r.AchievementPlanAreaOfConcernId == filter.AreaOfConcernId));
            }
            return query.ToList();
        }

        public AchievementPlanSubAreaOfConcern GetSubAreaOfConcern(int Id)
        {
            return Session.Get<AchievementPlanSubAreaOfConcern>(Id);
        }

        public void SaveSubAreaOfConcern(AchievementPlanSubAreaOfConcern achievementPlanSubAreaOfConcern)
        {
            Session.SaveOrUpdate(achievementPlanSubAreaOfConcern);
            Session.Flush();
        }
    }
    public class AchievementPlanTargetListRepository : LinqRepository<AchievementPlanTargetList>, IAchievementPlanTargetListRepository
    {
        public List<AchievementPlanTargetList> ListSmartsTarget(LfsQueryFilter filter)
        {
            var query = Session.Query<AchievementPlanTargetList>();
            if (filter.SubAreaOfConcernId.HasValue)
            {
                query = query.Where(s => s.SubAreaofConcern.Any(r => r.AchievementPlanSubAreaOfConcernId == filter.SubAreaOfConcernId));
            }
            return query.ToList();
        }

        public AchievementPlanTargetList GetTargetList(int Id)
        {
            return Session.Get<AchievementPlanTargetList>(Id);
        }

        public void SaveSmartstarget(AchievementPlanTargetList achievementPlanTargetList)
        {
            Session.SaveOrUpdate(achievementPlanTargetList);
            Session.Flush();
        }
    }
    public class AchievementPlanRootCauseRepository : LinqRepository<AchievementPlanRootCause>, IAchievementPlanRootCauseRepository
    {
        public List<AchievementPlanRootCause> ListRootCause(LfsQueryFilter filter)
        {
            var query = Session.Query<AchievementPlanRootCause>();
            if (filter.TargetId.HasValue)
            {
                query = query.Where(s => s.Target.Any(r => r.AchievementPlanTargetId == filter.TargetId));
            }
            return query.ToList();
        }

        public AchievementPlanRootCause GetRootCause(int Id)
        {
            return Session.Get<AchievementPlanRootCause>(Id);
        }

        public void SaveRootCause(AchievementPlanRootCause rootCause)
        {
            Session.SaveOrUpdate(rootCause);
            Session.Flush();
        }
    }
    public class AchievementPlanLeveragePointRepository : LinqRepository<AchievementPlanLeveragePoint>, IAchievementPlanLeveragePointRepository
    {
        public List<AchievementPlanLeveragePoint> ListLeveragePoint(LfsQueryFilter filter)
        {
            var query = Session.Query<AchievementPlanLeveragePoint>();
            if (filter.RootCauseId.HasValue)
            {
                query = query.Where(s => s.RootCause.Any(r => r.AchievementPlanRootCauseId == filter.RootCauseId));
            }
            return query.ToList();
        }

        public AchievementPlanLeveragePoint GetLeveragePoint(int Id)
        {
            return Session.Get<AchievementPlanLeveragePoint>(Id);
        }

        public void SaveLeveragePoint(AchievementPlanLeveragePoint leveragePoint)
        {
            Session.SaveOrUpdate(leveragePoint);
            Session.Flush();
        }
    }
    public class AchievementPlanActionRepository : LinqRepository<AchievementPlanAction>, IAchievementPlanActionRepository
    {
        public List<AchievementPlanAction> ListPlanAction(LfsQueryFilter filter)
        {
            var query = Session.Query<AchievementPlanAction>();
            if (filter.LeveragePointId.HasValue)
            {
                query = query.Where(s => s.LeveragePoint.Any(r => r.AchievementPlanLeveragePointId == filter.LeveragePointId));
            }
            return query.ToList();
        }

        public AchievementPlanAction GetPlanAction(int Id)
        {
            return Session.Get<AchievementPlanAction>(Id);
        }

        public void SavePlanAction(AchievementPlanAction planAction)
        {
            Session.SaveOrUpdate(planAction);
            Session.Flush();
        }
    }

    
}
