using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.Members;
using DomainLayer.TWADataModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping
{
    public class LifetimeIndividualScoreMap : IAutoMappingOverride<LifetimeIndividualScore>
    {
        public void Override(AutoMapping<LifetimeIndividualScore> mapping)
        {
            mapping.Table("Members.dbo.LifetimeIndividualScore");
            mapping.Id(x => x.Id, "StudentId");
            mapping.Map(x => x.SecondaryId);
            mapping.Map(x => x.TertiaryId);
            mapping.Map(x => x.ClassroomMeasureId);
            mapping.Map(x => x.Score);
            mapping.Map(x => x.LastModified).Not.Nullable();
            mapping.Map(x => x.LastModifiedBy).Not.Nullable();
            mapping.Map(x => x.ScoreId).Not.Nullable();
            mapping.Map(x => x.MasteryDate);
            mapping.Map(x => x.InstructionalLevelId);
        }
    }
}
