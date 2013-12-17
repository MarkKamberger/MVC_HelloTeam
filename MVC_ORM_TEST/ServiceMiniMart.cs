using Infrastructure;
using Infrastructure.ApplicationRepository;
using Infrastructure.LFSRepository;
using Infrastructure.SecutiryRepository;
using Infrastructure.TWARepository;
using Services;
using Services.LFSService;
using Services.SecurityService;


namespace DotFramework.Tests
{
    public class ServiceMiniMart
    {
        #region UnitTesting Setup MockObjects
      
        #endregion

        #region Integration Testing Setup

        /// <summary>
        /// Creates the TWA Service.
        /// </summary>
        /// <returns></returns>
        public static TWAService CreateTWAService()
        {
            return new TWAService(new Student2ActivityRepository()
                , new ListNavigationLinksRepository(), new ListNavigationLinksOrmRepository());
        }
        /// <summary>
        /// Creates the security service.
        /// </summary>
        /// <returns></returns>
        public static SecurityService CreateSecurityService()
        {
            return new SecurityService(new _SALI_LoginWebUserRepository());
        }

        /// <summary>
        /// Creates the LFS service.
        /// </summary>
        /// <returns></returns>
        public static LFSService CreateLFSService()
        {
            return new LFSService(new LFSGuideTypesRepository(), new AchievementPlanAreaOfConcernRepository(), new AchievementPlanSubAreaOfConcernRepository(), new AchievementPlanTargetListRepository(), new AchievementPlanRootCauseRepository(), new AchievementPlanLeveragePointRepository(), new AchievementPlanActionRepository());
        }
        #endregion
    }
}
