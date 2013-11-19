using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotFramework.Tests;
using MVC_ORM_TEST.Base;
using NUnit.Framework;

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
    }
}
