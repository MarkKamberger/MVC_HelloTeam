﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor.Installer;
using Infrastructure.DataMapping;
using Infrastructure.NHibernateMaps;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Metadata;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;

namespace Tests
{
    class InitSessionConnection
    {

        #region Fields
        private readonly NHibernate.Cfg.Configuration _configuration;
        private ISession _session;
        private IDictionary<string, IClassMetadata> _allClassMetadata;
        #endregion Fields

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="InitSessionConnection"/> class.
        /// </summary>
        public InitSessionConnection()
        {
            
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize(); 
            _configuration = NHibernateSession.Init(
                  new SimpleSessionStorage(),
                  new[] { "Infrastructure.dll" },
                  new AutoPersistenceModelGenerator().Generate(),
                  "../../../../MvcHelloTeam/MvcHelloTeam/NHibernate.config");
            IWindsorContainer container = new WindsorContainer();

            container.Register(
                    Component
                        .For(typeof(IEntityDuplicateChecker))
                        .ImplementedBy(typeof(EntityDuplicateChecker))
                        .Named("entityDuplicateChecker"));

            container.Register(
                    Component
                        .For(typeof(ISessionFactoryKeyProvider))
                        .ImplementedBy(typeof(DefaultSessionFactoryKeyProvider))
                        .Named("sessionFactoryKeyProvider"));

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
        #endregion Constructor

        #region Public Returns
        /// <summary>
        /// Configurations this instance.
        /// </summary>
        /// <returns></returns>
        public NHibernate.Cfg.Configuration Configuration()
        {
            return _configuration;
        }
        /// <summary>
        /// Sessions this instance.
        /// </summary>
        /// <returns></returns>
        public ISession Session()
        {
            return _session = NHibernateSession.GetDefaultSessionFactory().OpenSession();
        }

        /// <summary>
        /// Alls the class metadata.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IClassMetadata> AllClassMetadata()
        {
            return _allClassMetadata = NHibernateSession.GetDefaultSessionFactory().GetAllClassMetadata();
        }
        #endregion Public Returns
    }
}
