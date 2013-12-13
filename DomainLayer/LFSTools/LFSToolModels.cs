using SharpArch.Domain.DomainModel;

namespace DomainLayer.LFSTools
{
    public class LFSGuideTypes : Entity 
    {
        public virtual string Title { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual int ReportTypeId { get; set; }
        public virtual string ShortTitle { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual bool Active { get; set; }

    }
    public class AchievementPlanAreaOfConcern : Entity
    {
        public virtual string AreaOfConcernDesc { get; set; }
        public virtual bool Active { get; set; }

    }
    public class AchievementPlanSubAreaOfConcern :Entity
    {
        public virtual string SubAreaOfConcernDesc { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanTargetList :Entity
    {
        public virtual string TargetDesc { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanRootCause : Entity
    {
        public virtual string RootCauseDesc { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanLeveragePoint : Entity
    {
        public virtual string LeveragePointDesc { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanAction :Entity
    {
        public virtual string ActionDesc { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanCategory : Entity
    {
        public virtual string CategoryDesc { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanSubgroups : Entity
    {
        public virtual string Subgroup { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanTargetMeasure : Entity
    {
        public virtual string MeasureDesc { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanTargetVariables : Entity
    {
        public virtual int AchievementPlanTargetId { get; set; }
        public virtual int TargetVariableId { get; set; }
        public virtual int ValueId { get; set; }
        public virtual string ValueText { get; set; }
        public virtual bool Active { get; set; }
    }
    public class AchievementPlanTargetVariablesList : Entity
    {
        public virtual string TargetVar { get; set; }
        public virtual string TargetVarDesc { get; set; }
        public virtual string TargetVarInstr { get; set; }
        public virtual int DataTypeId { get; set; }
        public virtual bool Active { get; set; }
    }


#region LinkTables
    public class AchievementPlanAreaOfConcern2SubAreaOfConcern : Entity
    {
        public virtual int AchievementPlanAreaOfConcernId { get; set; }
        public virtual int AchievementPlanSubAreaOfConcernId { get; set; }
    }
    public class AchievementPlanCategory2AreaOfConcern : Entity
    {
        public virtual int AchievementPlanCategoryId { get; set; }
        public virtual int AchievementPlanAreaOfConcernId { get; set; }
    }
    public class AchievementPlanLeveragePoint2Action :Entity
    {
        public virtual int LeveragePointId { get; set; }
        public virtual int ActionId { get; set; }
        public virtual string ResponsibleParties { get; set; }
        public virtual string Notes { get; set; }
        public virtual int ActionCompleted { get; set; }
        public virtual string OtherActionDesc { get; set; }
    }
    public class AchievementPlanLeveragePoint2ActionList : Entity
    {
        public virtual int AchievementPlanLeveragePointId { get; set; }
        public virtual int AchievementPlanActionId { get; set; }
    }
    public class AchievementPlanRootCause2LeveragePoint : Entity
    {
        public virtual int LeveragePointId { get; set; }
        public virtual int RootCauseId { get; set; }
        public virtual string OtherLeveragePointDesc { get; set; }
        public virtual string Notes { get; set; }
        public virtual int LeveragePointAddressed { get; set; }
        public virtual int ActionsContinued { get; set; }
    }
    public class AchievementPlanRootCause2LeveragePointList : Entity
    {
        public virtual int AchievementPlanRootCauseId { get; set; }
        public virtual int AchievementPlanLeveragePointId { get; set; }
    }
    public class AchievementPlanSubAreaofConcern2Target : Entity
    {
        public virtual int AchievementPlanSubAreaOfConcernId { get; set; }
        public virtual int AchievementPlanTargetId { get; set; }

    }
    public class AchievementPlanTarget2RootCauseList : Entity
    {
        public virtual int AchievementPlanTargetId { get; set; }
        public virtual int AchievementPlanRootCauseId { get; set; }

    }
#endregion LinkTables
}
