using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using LFSTools;
using LFSTools.Helpers;
using MVC_ORM_TEST.Base;

using NUnit.Framework;

namespace MVC_ORM_TEST.LFStoolsTests
{
    [TestFixture]
    public class LFSToolsUnitTesting : BaseIntegrationTest
    {
        [Test]
        public void _ReturnInt()
        {
            var sample = "1";
            int expected = 1;
            int returned = NumberHelpers.ReturnAsInt(sample);
            Assert.True(expected == returned);
        }
        [Test]
        public void _ReturnDecimal()
        {
            var sample = "0.999";
            decimal expected = 0.999m;
            decimal returned = NumberHelpers.ReturnAsDecimal(sample);
            Assert.True(expected == returned);
        }
        [Test]
        public void _ReturnDouble()
        {
            var sample = "1234567891012134";
            double expected = 1234567891012134;
            double returned = NumberHelpers.ReturnAsDouble(sample);
            Assert.True(expected == returned);
        }
        [Test]
        public void _ReturnBool()
        {
            var sample1 = "false";
            bool expected1 = false;
            bool returned1 = NumberHelpers.ReturnAsBool(sample1);
            var sample2 = "0";
            bool expected2 = false;
            bool returned2 = NumberHelpers.ReturnAsBool(sample2);
            Assert.True(expected1 == returned1 && expected2 == returned2);
        }
    }
}
