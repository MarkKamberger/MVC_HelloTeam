using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.TWADataModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping
{
    public class StudentActivityMasteryMap : IAutoMappingOverride<TWAActivity2StudentLevelMastery>
    {
        public void Override(AutoMapping<TWAActivity2StudentLevelMastery> mapping)
        {
            mapping.Table("TWAActivity2StudentLevelMastery");
            mapping.Id(x => x.Id, "TWAActivity2StudentId");
            mapping.Map(x => x.Mastered);
            mapping.Map(x => x.NumberOfPresentations);
            mapping.Map(x => x.CorrectInARow);
            mapping.Map(x => x.InCorrectInARow);
            mapping.Map(x => x.DateLastAssessed);
            mapping.Map(x => x.SessionNumber);
            mapping.Map(x => x.MeasureIndex);
            mapping.Map(x => x.ActivityStartCount);
            mapping.Map(x => x.OverMastered);
            mapping.Map(x => x.SharedStoryId);
           
        }
    }
}
