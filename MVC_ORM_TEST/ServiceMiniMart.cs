using Infrastructure;
using Infrastructure.SecutiryRepository;
using Infrastructure.TWARepository;
using Services;
using Services.SecurityService;


namespace DotFramework.Tests
{
    public class ServiceMiniMart
    {
        #region UnitTesting Setup MockObjects
      
        #endregion

        #region Integration Testing Setup

        /// <summary>
        /// Creates the TWAservice.
        /// </summary>
        /// <returns></returns>
        public static TWAService CreateTWAService()
        {
            return new TWAService(new Student2ActivityRepository());
        }
        public static SecurityService CreateSecurityService()
        {
            return new SecurityService(new _SALI_LoginWebUserRepository());
        }
        #endregion
    }
}
