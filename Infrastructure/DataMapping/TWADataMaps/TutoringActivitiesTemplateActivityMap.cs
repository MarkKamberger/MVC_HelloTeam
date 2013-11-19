using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.TWADataModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping.TWADataMaps
{
    class TutoringActivitiesTemplateActivityMap : IAutoMappingOverride<TWATutoringActivitiesTemplateActivity>
    {
        public void Override(AutoMapping<TWATutoringActivitiesTemplateActivity> mapping)
        {
            mapping.Table("TWATutoringActivitiesTemplateActivity");

            mapping.Id(x => x.Id).GeneratedBy.GetGeneratorMapping().IsSpecified("TutoringActivitiesTemplateActivityId");
            mapping.Map(x => x.TutoringActivitiesTemplateActivityName);
            mapping.Map(x => x.ShortCode);
            mapping.Map(x => x.ObjectiveId);
            mapping.Map(x => x.TimeSpent);
        }
    }
}
