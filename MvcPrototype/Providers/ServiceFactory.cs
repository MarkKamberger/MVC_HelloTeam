using Infrastructure.ApplicationRepository;
using Infrastructure.SecutiryRepository;
using Infrastructure.TWARepository;
using Services;
using Services.SecurityService;

namespace MvcPrototype.Providers
{
    public class ServiceFactory
    {
        public static TWAService CreateTWAService()
        {
            return new TWAService(new Student2ActivityRepository()
                , new ListNavigationLinksRepository(), new ListNavigationLinksOrmRepository());
        }
        public static SecurityService CreateSecurityService()
        {
            return new SecurityService(new _SALI_LoginWebUserRepository());
        }
    }
}