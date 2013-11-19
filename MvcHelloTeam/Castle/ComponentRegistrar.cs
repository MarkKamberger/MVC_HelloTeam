using System.Configuration;


using MvcHelloTeam.Web.Mvc.Providers;



namespace MvcHelloTeam.Web.Mvc.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using SharpArch.Domain.Commands;
    using SharpArch.Domain.Events;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;
    using SharpArch.NHibernate.Contracts.Repositories;
    using SharpArch.Web.Mvc.Castle;

    public class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddGenericRepositoriesTo(container);
            AddCustomRepositoriesTo(container);
            AddServicesTo(container);
            AddMembershipTo(container);
        }

        private static void AddMembershipTo(IWindsorContainer container)
        {
            container.Register(
                    Component.For<IAuth>().ImplementedBy<FormsAuthenticationWrapper>()
                );
        }

        private static void AddServicesTo(IWindsorContainer container)
        {
           /* container.Register(
               Component.For<IEmailService>()
                .ImplementedBy<EmailService>()
                .DependsOn(new
                {
                    templateFileRoot = ConfigurationManager.AppSettings["EmailTemplateRoot"]
                }));
            container.Register(
               Component.For<INotificationEngine>()
                .ImplementedBy<NotificationEngine>()
                .DependsOn(new
                {
                    templateFileRoot = ConfigurationManager.AppSettings["EmailTemplateRoot"]
                }));*/

        
            
            container.Register(
                AllTypes
                    .FromAssemblyNamed("Services")
                    .Pick().If(t => t.Name.EndsWith("Service"))
                    .WithService
                    .FirstNonGenericCoreInterface("Services"));

        }

        private static void AddCustomRepositoriesTo(IWindsorContainer container) 
        {
            container.Register(
                AllTypes
                    .FromAssemblyNamed("Infrastructure")
                    .BasedOn(typeof(ILinqRepositoryWithTypedId<,>))
                    .WithService.FirstNonGenericCoreInterface("Infrastructure"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(
                Component.For(typeof(IEntityDuplicateChecker))
                    .ImplementedBy(typeof(EntityDuplicateChecker))
                    .Named("entityDuplicateChecker"));

            container.Register(
                Component.For(typeof(INHibernateRepository<>))
                    .ImplementedBy(typeof(NHibernateRepository<>))
                    .Named("nhibernateRepositoryType")
                    .Forward(typeof(IRepository<>)));

            container.Register(
                Component.For(typeof(INHibernateRepositoryWithTypedId<,>))
                    .ImplementedBy(typeof(NHibernateRepositoryWithTypedId<,>))
                    .Named("nhibernateRepositoryWithTypedId")
                    .Forward(typeof(IRepositoryWithTypedId<,>)));

            container.Register(
                    Component.For(typeof(ISessionFactoryKeyProvider))
                        .ImplementedBy(typeof(DefaultSessionFactoryKeyProvider))
                        .Named("sessionFactoryKeyProvider"));

            container.Register(
                    Component.For(typeof(ICommandProcessor))
                        .ImplementedBy(typeof(CommandProcessor))
                        .Named("commandProcessor"));
        }

    }
}