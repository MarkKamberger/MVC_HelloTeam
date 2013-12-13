using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer;
using DotFramework.Tests;
using Infrastructure.DataMapping;
using MVC_ORM_TEST.Base;
using NUnit.Framework;
using SharpArch.NHibernate.Web.Mvc;

namespace MVC_ORM_TEST.Test.TWA.Integrations
{
    [TestFixture]
    public class SecurityTest :BaseIntegrationTest
    {
        [Test]
        public void Test_Login()
        {
            var service = ServiceMiniMart.CreateSecurityService();
            var results = service.LoginWebUser("ptwdemo", "ptwdemo1", false);
        }
        [Test]
        public void Test_NavigationLinks()
        {
            var service = ServiceMiniMart.CreateTWAService();
            var results = service.LisNavigationLinks(1, Mock.MockStrongSecurityObject(), 19394, 261711);
        }

        [Test]
        public void Test_NavigationLinkORM()
        {
            var service = ServiceMiniMart.CreateTWAService();
            var results = service.ListNavigationLinkORM(1);
        }

    }
}
