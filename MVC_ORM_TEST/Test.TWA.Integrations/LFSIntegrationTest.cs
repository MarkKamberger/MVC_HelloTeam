using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DotFramework.Tests;
using MVC_ORM_TEST.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace MVC_ORM_TEST.Test.TWA.Integrations
{
    [TestFixture]
    public class LFSIntegrationTest : BaseIntegrationTest
    {
        [Test]
        public void Get_Lfs_GuideTypes()
        {
            var lfsService = ServiceMiniMart.CreateLFSService();
            var results =  lfsService.GetLFSGuideTypes();
        }
    }
}
