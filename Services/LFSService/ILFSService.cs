using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.LFSTools;

namespace Services.LFSService
{
    public interface ILFSService
    {
        IList<LFSGuideTypes> ListLFSGuideTypes();

        IList<AchievementPlanAreaOfConcern> ListAreaOfConcern(LfsQueryFilter filter);
        AchievementPlanAreaOfConcern GetAreaOfConcern(int id);
        void SaveAreaOfConcern(AchievementPlanAreaOfConcern areaOfConcern);


        IList<AchievementPlanSubAreaOfConcern> ListSubAreaOfConcern(LfsQueryFilter filter);
        AchievementPlanSubAreaOfConcern GetSubAreaOfConcern(int id);
        void SaveSubAreaOfConcern(AchievementPlanSubAreaOfConcern areaOfConcern);

        IList<AchievementPlanTargetList> ListSmartsTarget(LfsQueryFilter filter);
        AchievementPlanTargetList GetTargetList(int id);
        void SaveSmartstarget(AchievementPlanTargetList targetList);

        IList<AchievementPlanRootCause> ListRootCause(LfsQueryFilter filter);
        AchievementPlanRootCause GetRootCause(int id);
        void SaveRootCause(AchievementPlanRootCause rootCause);

        IList<AchievementPlanLeveragePoint> ListLeveragePoint(LfsQueryFilter filter);
        AchievementPlanLeveragePoint GetLeveragePoint(int id);
        void SaveLeveragePoint(AchievementPlanLeveragePoint leveragePoint);

        IList<AchievementPlanAction> ListPlanAction(LfsQueryFilter filter);
        AchievementPlanAction GetPlanAction(int id);
        void SavePlanAction(AchievementPlanAction planAction);
    
    }

}
