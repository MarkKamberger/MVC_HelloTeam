using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.LFSTools;
using SharpArch.Domain.PersistenceSupport;

namespace Infrastructure.LFSRepository
{
    public interface ILFSGuideTypesRepository : ILinqRepositoryWithTypedId<LFSGuideTypes, int>
    {
        List<LFSGuideTypes> GetLFSGuideTypes();
    }

    public interface IAchievementPlanAreaOfConcernRepository : ILinqRepositoryWithTypedId<AchievementPlanAreaOfConcern, int>
    {
        List<AchievementPlanAreaOfConcern> ListAreaOfConcern(LfsQueryFilter filter);
        AchievementPlanAreaOfConcern GetAreaOfConcern(int Id);
        void SaveAreaOfConcern(AchievementPlanAreaOfConcern areaOfConcern);

    }
    public interface IAchievementPlanSubAreaOfConcernRepository : ILinqRepositoryWithTypedId<AchievementPlanSubAreaOfConcern, int>
    {
        List<AchievementPlanSubAreaOfConcern> ListSubAreaOfConcern(LfsQueryFilter filter);
        AchievementPlanSubAreaOfConcern GetSubAreaOfConcern(int Id);
        void SaveSubAreaOfConcern(AchievementPlanSubAreaOfConcern achievementPlanSubAreaOfConcern);

    }
    public interface IAchievementPlanTargetListRepository : ILinqRepositoryWithTypedId<AchievementPlanTargetList, int>
    {
        List<AchievementPlanTargetList> ListSmartsTarget(LfsQueryFilter filter);
        AchievementPlanTargetList GetTargetList(int Id);
        void SaveSmartstarget(AchievementPlanTargetList achievementPlanTargetList);

    }
    public interface IAchievementPlanRootCauseRepository : ILinqRepositoryWithTypedId<AchievementPlanRootCause, int>
    {

        List<AchievementPlanRootCause> ListRootCause(LfsQueryFilter filter);
        AchievementPlanRootCause GetRootCause(int Id);
        void SaveRootCause(AchievementPlanRootCause rootCause);

    }
    public interface IAchievementPlanLeveragePointRepository : ILinqRepositoryWithTypedId<AchievementPlanLeveragePoint, int>
    {
        List<AchievementPlanLeveragePoint> ListLeveragePoint(LfsQueryFilter filter);
        AchievementPlanLeveragePoint GetLeveragePoint(int Id);
        void SaveLeveragePoint(AchievementPlanLeveragePoint leveragePoint);

    }
    public interface IAchievementPlanActionRepository : ILinqRepositoryWithTypedId<AchievementPlanAction, int>
    {
        List<AchievementPlanAction> ListPlanAction(LfsQueryFilter filter);
        AchievementPlanAction GetPlanAction(int Id);
        void SavePlanAction(AchievementPlanAction planAction);

    }
 
}
