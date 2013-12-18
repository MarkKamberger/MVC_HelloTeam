using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.LFSTools;
using DotFramework.Tests;
using MVC_ORM_TEST.Base;
using NUnit.Framework;

namespace MVC_ORM_TEST.Test.TWA.Integrations
{
    [TestFixture]
    public class LFSIntegrationTest : BaseIntegrationTest
    {
        [Test]
        public void Get_Lfs_GetDataDrillDown()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();
            var results =  lfsService.ListLFSGuideTypes();
            var filter =new LfsQueryFilter
                {
                    CategoryId = 1
                };
            var areaOfConcern = lfsService.ListAreaOfConcern(filter);

            filter.AreaOfConcernId = areaOfConcern.First().Id;
            var subAreaOfConcern = lfsService.ListSubAreaOfConcern(filter);

            filter.SubAreaOfConcernId = subAreaOfConcern.First().Id;
            var smartsTarget = lfsService.ListSmartsTarget(filter);

            filter.TargetId = smartsTarget.First().Id;
            var rootCause = lfsService.ListRootCause(filter);

            filter.RootCauseId = rootCause.First().Id;
            var leveragePoint = lfsService.ListLeveragePoint(filter);

            filter.LeveragePointId = leveragePoint.First().Id;
            var planAction = lfsService.ListPlanAction(filter);
            Assert.True(areaOfConcern.Count >0
                && subAreaOfConcern.Count >0
                && rootCause.Count > 0
                && leveragePoint.Count >0
                && planAction.Count >0);
        }

        [Test]
        public void Save_Lfs_AreaOfConcern()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();
            
            var filter = new LfsQueryFilter
            {
                CategoryId = 1
            };      
            var areaOfConcern = new AchievementPlanAreaOfConcern
                {
                    Active = true,
                    AreaOfConcernDesc = "Test"
                };
            var child = new AchievementPlanCategory2AreaOfConcern
                {
                    AchievementPlanAreaOfConcernId = areaOfConcern.Id,
                    AchievementPlanCategoryId = filter.CategoryId.Value
                }; 
            areaOfConcern.Category.Add(child);
            child.Parent = areaOfConcern;
            lfsService.SaveAreaOfConcern(areaOfConcern);
            Assert.True(areaOfConcern.Id >0 && areaOfConcern.Category.First().Id>0);
        }

        [Test]
        public void Save_Lfs_SubAreaOfConcern()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();

            var filter = new LfsQueryFilter
                {
                    CategoryId = 1,
                    AreaOfConcernId = 1
                };
            var subAreaOfConcern = new AchievementPlanSubAreaOfConcern
                {
                    Active = true,
                    SubAreaOfConcernDesc = "TestDesc"
                };
            var child = new AchievementPlanAreaOfConcern2SubAreaOfConcern
                {
                    AchievementPlanAreaOfConcernId = filter.AreaOfConcernId.Value,
                    AchievementPlanSubAreaOfConcernId = subAreaOfConcern.Id
                };
            subAreaOfConcern.AreaOfConcern.Add(child);
            child.Parent = subAreaOfConcern;
            lfsService.SaveSubAreaOfConcern(subAreaOfConcern);
            Assert.True(subAreaOfConcern.Id >0 && subAreaOfConcern.AreaOfConcern.First().Id > 0);

        }

        [Test]
        public void Save_Lfs_SMARTSTarget()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();

            var filter = new LfsQueryFilter
                {
                    CategoryId = 1,
                    AreaOfConcernId = 1,
                    SubAreaOfConcernId = 10007
                };
            var target = new AchievementPlanTargetList
                {
                   Active = true,
                   TargetDesc = "Test"
                };
            var child = new AchievementPlanSubAreaofConcern2Target
                {
                    AchievementPlanSubAreaOfConcernId = filter.SubAreaOfConcernId.Value,
                    AchievementPlanTargetId = target.Id
                };
            target.SubAreaofConcern.Add(child);
            child.Parent = target;
            lfsService.SaveSmartstarget(target);
            Assert.True(target.Id > 0 && target.SubAreaofConcern.First().Id >0);

        }
        [Test]
        public void Save_Lfs_RootCause()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();

            var filter = new LfsQueryFilter
                {
                    CategoryId = 1,
                    AreaOfConcernId = 1,
                    SubAreaOfConcernId = 10007,
                    TargetId = 10019
                };
            var rootCause = new AchievementPlanRootCause
                {
                    Active = true,
                    RootCauseDesc = "test"
                };
            var child = new AchievementPlanTarget2RootCauseList
                {
                    AchievementPlanTargetId = filter.TargetId.Value,
                    AchievementPlanRootCauseId = rootCause.Id,
                    Parent = rootCause
                };
            rootCause.Target.Add(child);
            lfsService.SaveRootCause(rootCause);
            Assert.True(rootCause.Id > 0 && rootCause.Target.First().Id > 0);

        }
        [Test]
        public void Save_Lfs_ImplementationFocus()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();

            var filter = new LfsQueryFilter
            {
                CategoryId = 1,
                AreaOfConcernId = 1,
                SubAreaOfConcernId = 10007,
                TargetId = 10019,
                RootCauseId = 10013
            };
            var leveragePoint = new AchievementPlanLeveragePoint
            {
               Active = true
               ,LeveragePointDesc = "test"
            };
            var child = new AchievementPlanRootCause2LeveragePointList()
            {
                AchievementPlanRootCauseId = filter.TargetId.Value,
                AchievementPlanLeveragePointId = leveragePoint.Id,
                Parent = leveragePoint
            };
            leveragePoint.RootCause.Add(child);
            lfsService.SaveLeveragePoint(leveragePoint);
            Assert.True(leveragePoint.Id > 0 && leveragePoint.RootCause.First().Id > 0);

        }
        [Test]
        public void Save_Lfs_Actions()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();
            var filter = new LfsQueryFilter
            {
                CategoryId = 1,
                AreaOfConcernId = 1,
                SubAreaOfConcernId = 10007,
                TargetId = 10019,
                RootCauseId = 10013
            };
            var action = new AchievementPlanAction
            {
                Active = true,
                ActionDesc = "test"
            };
            var child = new AchievementPlanLeveragePoint2ActionList
            {
                AchievementPlanLeveragePointId = filter.TargetId.Value,
                AchievementPlanActionId = action.Id,
                Parent = action
            };
            action.LeveragePoint.Add(child);
            lfsService.SavePlanAction(action);
            Assert.True(action.Id > 0 && action.LeveragePoint.First().Id > 0);

        }

        [Test]
        public void Update_Lfs_Object()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();
            var obj = lfsService.GetAreaOfConcern(1);
            obj.AreaOfConcernDesc = "Letter-Sound Association";
            lfsService.SaveAreaOfConcern(obj);
        }
    }
}
