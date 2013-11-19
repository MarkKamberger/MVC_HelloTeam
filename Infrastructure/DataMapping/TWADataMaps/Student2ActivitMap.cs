using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.TWADataModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping
{
    public class Student2ActivitMap : IAutoMappingOverride<TWAActivity2Student>
    {
        public void Override(AutoMapping<TWAActivity2Student> mapping)
        {
            mapping.Table("TWAActivity2Student");
            mapping.Id(x => x.Id, "TWAActivity2StudentId");
            mapping.Map(x => x.TWAActivityId).Not.Nullable();
            mapping.Map(x => x.StudentId).Not.Nullable();
            mapping.Map(x => x.LastDateAssessed).Not.Nullable();
            mapping.HasMany(x => x.TWAActivity2StudentLevelMastery)
                   .KeyColumn("TWAActivity2StudentId")
                   .Inverse()
                   .Cascade.AllDeleteOrphan();
            mapping.References(x => x.TwaTutoringActivitiesTemplateActivity).Columns("TWAActivityId");
            
        }
    }
}
