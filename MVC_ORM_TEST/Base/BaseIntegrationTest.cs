using System;
using System.Dynamic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotFramework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SharpArch.NHibernate;
using Tests;

namespace MVC_ORM_TEST.Base
{


    public class BaseIntegrationTest : DynamicObject
    {
        #region Setup & TearDown

        /// <summary>
        /// Set Configuration, Mapping & init ServiceLocator
        /// These tests are SLOW...  
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            new InitSessionConnection();
            ServiceLocatorInitializer.Init();
            //Note* when using multiple databases use NHibernateSession.CurrentFor()
            NHibernateSession.Current.BeginTransaction();
            //HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
        }
        /// <summary>
        /// Tears down.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            //Note* when using multiple databases use NHibernateSession.CurrentFor()
            NHibernateSession.Current.Transaction.Rollback();
            NHibernateSession.CloseAllSessions();
            NHibernateSession.Reset();
        }

        #endregion Setup & TearDown
    }
}
