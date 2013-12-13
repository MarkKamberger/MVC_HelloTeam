using DomainLayer.LFSTools;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping.LFSTools
{
    public class LFSToolMap :IAutoMappingOverride<LFSGuideTypes>
    {
        public void Override(AutoMapping<LFSGuideTypes> mapping)
        {
            mapping.Table("LFSGuideTypes");
            mapping.Id(x => x.Id).Column("GuideTypeId").GeneratedBy.Assigned();
            mapping.Map(x => x.Title).Length(100).Nullable();
            mapping.Map(x => x.Enabled).Nullable();
            mapping.Map(x => x.ReportTypeId).Nullable();
            mapping.Map(x => x.ShortTitle).Length(50).Nullable();
            mapping.Map(x => x.CategoryId).Nullable();
            mapping.Map(x => x.SortOrder).Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanAreaOfConcernMap : IAutoMappingOverride<AchievementPlanAreaOfConcern>
    {
        public void Override(AutoMapping<AchievementPlanAreaOfConcern> mapping)
        {
            mapping.Table("AchievementPlanAreaOfConcern");
            mapping.Id(x => x.Id).Column("AreaOfConcernId").GeneratedBy.Assigned();
            mapping.Map(x => x.AreaOfConcernDesc).Not.Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanSubAreaOfConcernMap : IAutoMappingOverride<AchievementPlanSubAreaOfConcern>
    {
        public void Override(AutoMapping<AchievementPlanSubAreaOfConcern> mapping)
        {
            mapping.Table("AchievementPlanSubAreaOfConcern");
            mapping.Id(x => x.Id).Column("SubAreaOfConcernId").GeneratedBy.Assigned();
            mapping.Map(x => x.SubAreaOfConcernDesc).Not.Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanTargetListMap : IAutoMappingOverride<AchievementPlanTargetList>
    {
        public void Override(AutoMapping<AchievementPlanTargetList> mapping)
        {
            mapping.Table("AchievementPlanTargetList");
            mapping.Id(x => x.Id).Column("TargetId").GeneratedBy.Assigned();
            mapping.Map(x => x.TargetDesc).Not.Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanRootCauseMap : IAutoMappingOverride<AchievementPlanRootCause>
    {
        public void Override(AutoMapping<AchievementPlanRootCause> mapping)
        {
            mapping.Table("AchievementPlanRootCause");
            mapping.Id(x => x.Id).Column("RootCauseId").GeneratedBy.Assigned();
            mapping.Map(x => x.RootCauseDesc).Not.Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanLeveragePointMap : IAutoMappingOverride<AchievementPlanLeveragePoint>
    {
        public void Override(AutoMapping<AchievementPlanLeveragePoint> mapping)
        {
            mapping.Table("AchievementPlanLeveragePoint");
            mapping.Id(x => x.Id).Column("LeveragePointId").GeneratedBy.Assigned();
            mapping.Map(x => x.LeveragePointDesc).Not.Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanActionMap : IAutoMappingOverride<AchievementPlanAction>
    {
        public void Override(AutoMapping<AchievementPlanAction> mapping)
        {
            mapping.Table("AchievementPlanAction");
            mapping.Id(x => x.Id).Column("ActionId").GeneratedBy.Assigned();
            mapping.Map(x => x.ActionDesc).Not.Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanCategoryMap : IAutoMappingOverride<AchievementPlanCategory>
    {
        public void Override(AutoMapping<AchievementPlanCategory> mapping)
        {
            mapping.Table("AchievementPlanCategory");
            mapping.Id(x => x.Id).Column("CategoryId").GeneratedBy.Assigned();
            mapping.Map(x => x.CategoryDesc).Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanSubgroupsMap : IAutoMappingOverride<AchievementPlanSubgroups>
    {
        public void Override(AutoMapping<AchievementPlanSubgroups> mapping)
        {
            mapping.Table("AchievementPlanSubgroups");
            mapping.Id(x => x.Id).Column("SubgroupId").GeneratedBy.Assigned();
            mapping.Map(x => x.Subgroup).Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanTargetMeasureMap : IAutoMappingOverride<AchievementPlanTargetMeasure>
    {
        public void Override(AutoMapping<AchievementPlanTargetMeasure> mapping)
        {
            mapping.Table("AchievementPlanTargetMeasure");
            mapping.Id(x => x.Id).Column("MeasureId").GeneratedBy.Assigned();
            mapping.Map(x => x.Active).Nullable();
            mapping.Map(x => x.MeasureDesc).Nullable();
        }
    }
    public class AchievementPlanTargetVariablesMap : IAutoMappingOverride<AchievementPlanTargetVariables>
    {
        public void Override(AutoMapping<AchievementPlanTargetVariables> mapping)
        {
            mapping.Table("AchievementPlanTargetVariables");
            mapping.Id(x => x.Id).Column("TargetVariableMapId").GeneratedBy.Assigned();
            mapping.Map(x => x.Active).Nullable();
            mapping.Map(x => x.AchievementPlanTargetId).Not.Nullable();
            mapping.Map(x => x.TargetVariableId).Not.Nullable();
            mapping.Map(x => x.ValueId).Nullable();
            mapping.Map(x => x.ValueText).Length(250).Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }
    public class AchievementPlanTargetVariablesListMap : IAutoMappingOverride<AchievementPlanTargetVariablesList>
    {
        public void Override(AutoMapping<AchievementPlanTargetVariablesList> mapping)
        {
            mapping.Table("AchievementPlanTargetVariablesList");
            mapping.Id(x => x.Id).Column("TargetVarId").GeneratedBy.Assigned();
            mapping.Map(x => x.Active).Nullable();
            mapping.Map(x => x.TargetVar).Length(10).Nullable();
            mapping.Map(x => x.TargetVarDesc).Length(50).Nullable();
            mapping.Map(x => x.TargetVarInstr).Length(250).Nullable();
            mapping.Map(x => x.DataTypeId).Nullable();
            mapping.Map(x => x.Active).Nullable();
        }
    }

    #region LinkTables
    public class AchievementPlanAreaOfConcern2SubAreaOfConcernMap : IAutoMappingOverride<AchievementPlanAreaOfConcern2SubAreaOfConcern>
    {
        public void Override(AutoMapping<AchievementPlanAreaOfConcern2SubAreaOfConcern> mapping)
        {
            mapping.Table("AchievementPlanAreaOfConcern2SubAreaOfConcern");
            mapping.Id(x => x.Id).Column("Id").GeneratedBy.Assigned();
            mapping.Map(x => x.AchievementPlanAreaOfConcernId).Not.Nullable();
            mapping.Map(x => x.AchievementPlanSubAreaOfConcernId).Not.Nullable();
        }
    }
    public class AchievementPlanCategory2AreaOfConcernMap : IAutoMappingOverride<AchievementPlanCategory2AreaOfConcern>
    {
        public void Override(AutoMapping<AchievementPlanCategory2AreaOfConcern> mapping)
        {
            mapping.Table("AchievementPlanCategory2AreaOfConcern");
            mapping.Id(x => x.Id).Column("Id").GeneratedBy.Assigned();
            mapping.Map(x => x.AchievementPlanAreaOfConcernId).Not.Nullable();
            mapping.Map(x => x.AchievementPlanCategoryId).Not.Nullable();
        }
    }
    public class AchievementPlanLeveragePoint2ActionMap : IAutoMappingOverride<AchievementPlanLeveragePoint2Action>
    {
        public void Override(AutoMapping<AchievementPlanLeveragePoint2Action> mapping)
        {
            mapping.Table("AchievementPlanLeveragePoint2Action");
            mapping.Id(x => x.Id).Column("AchievementPlanLeveragePoint2ActionId").GeneratedBy.Assigned();
            mapping.Map(x => x.LeveragePointId).Not.Nullable();
            mapping.Map(x => x.ActionId).Not.Nullable();
            mapping.Map(x => x.ResponsibleParties).Not.Nullable();
            mapping.Map(x => x.Notes).Nullable();
            mapping.Map(x => x.ActionCompleted).Nullable();
            mapping.Map(x => x.OtherActionDesc).Nullable();
        }
    }
    public class AchievementPlanLeveragePoint2ActionListMap : IAutoMappingOverride<AchievementPlanLeveragePoint2ActionList>
    {
        public void Override(AutoMapping<AchievementPlanLeveragePoint2ActionList> mapping)
        {
            mapping.Table("AchievementPlanLeveragePoint2ActionList");
            mapping.Id(x => x.Id).Column("Id").GeneratedBy.Assigned();
            mapping.Map(x => x.AchievementPlanActionId).Not.Nullable();
            mapping.Map(x => x.AchievementPlanLeveragePointId).Not.Nullable();
        }
    }
    public class AchievementPlanRootCause2LeveragePointMap : IAutoMappingOverride<AchievementPlanRootCause2LeveragePoint>
    {
        public void Override(AutoMapping<AchievementPlanRootCause2LeveragePoint> mapping)
        {
            mapping.Table("AchievementPlanRootCause2LeveragePoint");
            mapping.Id(x => x.Id).Column("RootCause2LeveragePointId").GeneratedBy.Assigned();
            mapping.Map(x => x.RootCauseId).Not.Nullable();
            mapping.Map(x => x.LeveragePointId).Not.Nullable();
            mapping.Map(x => x.OtherLeveragePointDesc).Nullable();
            mapping.Map(x => x.Notes).Nullable();
            mapping.Map(x => x.LeveragePointAddressed).Nullable();
            mapping.Map(x => x.ActionsContinued).Nullable();
        }
    }
    public class AchievementPlanRootCause2LeveragePointListMap : IAutoMappingOverride<AchievementPlanRootCause2LeveragePointList>
    {
        public void Override(AutoMapping<AchievementPlanRootCause2LeveragePointList> mapping)
        {
            mapping.Table("AchievementPlanRootCause2LeveragePointList");
            mapping.Id(x => x.Id).Column("Id").GeneratedBy.Assigned();
            mapping.Map(x => x.AchievementPlanLeveragePointId).Not.Nullable();
            mapping.Map(x => x.AchievementPlanRootCauseId).Not.Nullable();
        }
    }
    public class AchievementPlanSubAreaofConcern2TargetMap : IAutoMappingOverride<AchievementPlanSubAreaofConcern2Target>
    {
        public void Override(AutoMapping<AchievementPlanSubAreaofConcern2Target> mapping)
        {
            mapping.Table("AchievementPlanSubAreaofConcern2Target");
            mapping.Id(x => x.Id).Column("Id").GeneratedBy.Assigned();
            mapping.Map(x => x.AchievementPlanSubAreaOfConcernId).Not.Nullable();
            mapping.Map(x => x.AchievementPlanTargetId).Not.Nullable();
        }
    }
    public class AchievementPlanTarget2RootCauseListMap : IAutoMappingOverride<AchievementPlanTarget2RootCauseList>
    {
        public void Override(AutoMapping<AchievementPlanTarget2RootCauseList> mapping)
        {
            mapping.Table("AchievementPlanTarget2RootCauseList");
            mapping.Id(x => x.Id).Column("Id").GeneratedBy.Assigned();
            mapping.Map(x => x.AchievementPlanRootCauseId).Not.Nullable();
            mapping.Map(x => x.AchievementPlanTargetId).Not.Nullable();
        }
    }
    #endregion LinkTables
}
