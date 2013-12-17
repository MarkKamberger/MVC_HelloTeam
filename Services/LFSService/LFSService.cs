using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.LFSTools;
using Infrastructure.LFSRepository;
using NHibernate;
using SharpArch.NHibernate.Web.Mvc;

namespace Services.LFSService
{
    public class LFSService :ILFSService
    {
        private ILFSGuideTypesRepository _lfsGuide;
        private IAchievementPlanAreaOfConcernRepository _areaOfConcern;
        private IAchievementPlanSubAreaOfConcernRepository _subAreaOfConcern;
        private IAchievementPlanTargetListRepository _targetList;
        private IAchievementPlanRootCauseRepository _rootCause;
        private IAchievementPlanLeveragePointRepository _leveragePoint;
        private IAchievementPlanActionRepository _action;
        public LFSService(ILFSGuideTypesRepository lfsGuide
            ,IAchievementPlanAreaOfConcernRepository areaOfConcern
            ,IAchievementPlanSubAreaOfConcernRepository subAreaOfConcern
            ,IAchievementPlanTargetListRepository targetList
            ,IAchievementPlanRootCauseRepository rootCause
            ,IAchievementPlanLeveragePointRepository leveragePoint
            ,IAchievementPlanActionRepository action)
        {
            _lfsGuide = lfsGuide;
            _areaOfConcern = areaOfConcern;
            _subAreaOfConcern = subAreaOfConcern;
            _targetList = targetList;
            _rootCause = rootCause;
            _leveragePoint = leveragePoint;
            _action = action;

        }

        public IList<LFSGuideTypes> ListLFSGuideTypes()
        {
            return _lfsGuide.GetLFSGuideTypes();
        }

        public IList<AchievementPlanAreaOfConcern> ListAreaOfConcern(LfsQueryFilter filter)
        {
            return _areaOfConcern.ListAreaOfConcern(filter);
        }

        public AchievementPlanAreaOfConcern GetAreaOfConcern(int id)
        {
            return _areaOfConcern.GetAreaOfConcern(id);
        }

        public void SaveAreaOfConcern(AchievementPlanAreaOfConcern areaOfConcern)
        {
            _areaOfConcern.SaveAreaOfConcern(areaOfConcern);
        }

        public IList<AchievementPlanSubAreaOfConcern> ListSubAreaOfConcern(LfsQueryFilter filter)
        {
            return _subAreaOfConcern.ListSubAreaOfConcern(filter);
        }

        public AchievementPlanSubAreaOfConcern GetSubAreaOfConcern(int id)
        {
            return _subAreaOfConcern.GetSubAreaOfConcern(id);
        }

        public void SaveSubAreaOfConcern(AchievementPlanSubAreaOfConcern areaOfConcern)
        {
            _subAreaOfConcern.SaveSubAreaOfConcern(areaOfConcern);
        }

        public IList<AchievementPlanTargetList> ListSmartsTarget(LfsQueryFilter filter)
        {
            return _targetList.ListSmartsTarget(filter);
        }

        public AchievementPlanTargetList GetTargetList(int id)
        {
            return _targetList.GetTargetList(id);
        }

        public void SaveSmartstarget(AchievementPlanTargetList targetList)
        {
           _targetList.SaveSmartstarget(targetList);
        }

        public IList<AchievementPlanRootCause> ListRootCause(LfsQueryFilter filter)
        {
            return _rootCause.ListRootCause(filter);
        }

        public AchievementPlanRootCause GetRootCause(int id)
        {
            return _rootCause.GetRootCause(id);
        }

        public void SaveRootCause(AchievementPlanRootCause rootCause)
        {
          _rootCause.SaveRootCause(rootCause);
        }

        public IList<AchievementPlanLeveragePoint> ListLeveragePoint(LfsQueryFilter filter)
        {
            return _leveragePoint.ListLeveragePoint(filter);
        }

        public AchievementPlanLeveragePoint GetLeveragePoint(int id)
        {
            return _leveragePoint.GetLeveragePoint(id);
        }

        public void SaveLeveragePoint(AchievementPlanLeveragePoint leveragePoint)
        {
            _leveragePoint.SaveLeveragePoint(leveragePoint);
        }

        public IList<AchievementPlanAction> ListPlanAction(LfsQueryFilter filter)
        {
            return _action.ListPlanAction(filter);
        }

        public AchievementPlanAction GetPlanAction(int id)
        {
            return _action.GetPlanAction(id);
        }

        public void SavePlanAction(AchievementPlanAction planAction)
        {
            _action.SavePlanAction(planAction);
        }
    }
}
