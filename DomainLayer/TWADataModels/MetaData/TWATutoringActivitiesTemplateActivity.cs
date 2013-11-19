using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace DomainLayer.TWADataModels
{
    public class TWATutoringActivitiesTemplateActivity :Entity
    {
        public virtual int TutoringActivitiesTemplateActivityId { get; set; }
        public virtual string TutoringActivitiesTemplateActivityName { get; set; }
        public virtual string ShortCode { get; set; }
        public virtual int ObjectiveId { get; set; }
        public virtual string TimeSpent { get; set; }
    }
}
