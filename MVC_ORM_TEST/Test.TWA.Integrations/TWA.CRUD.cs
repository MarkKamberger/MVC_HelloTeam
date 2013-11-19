using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.TWADataModels;
using DotFramework.Tests;
using MVC_ORM_TEST.Base;

using NHibernate;
using NUnit.Framework;

namespace MVC_ORM_TEST.Test.TWA.Integrations
{
	[TestFixture]
	public class TWA :BaseIntegrationTest
	{
       
		[Test]
		public void GetMasteryDetails()
		{
		    var twaService = ServiceMiniMart.CreateTWAService();
		    var filter = new ActivityMasteryFilter
		        {
                    StudentId = 4237520
		        };
		   IList<TWAActivity2Student> results = twaService.GetMasteryDetails(filter);
		    var s1 = results[0].LastDateAssessed;
		    var s2 = results[0].TWAActivity2StudentLevelMastery[0].InCorrectInARow;
            
		    var r = results.Where(x => x.TWAActivity2StudentLevelMastery.Any(v => !v.Mastered));
		   Assert.IsTrue(results.Count >0);

		}
        [Test]
        public void Create_TWAService_True()
        {
            var twaService = ServiceMiniMart.CreateTWAService();
            Assert.IsNotNull(twaService);
        }
	}
}
