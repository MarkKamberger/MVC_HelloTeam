using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace DomainLayer.TWADataModels
{
    public class TWAActivity2Student : Entity
    {
        public TWAActivity2Student()
        {
            TWAActivity2StudentLevelMastery = new List<TWAActivity2StudentLevelMastery>();
            TwaTutoringActivitiesTemplateActivity = new TWATutoringActivitiesTemplateActivity();
        }

        public virtual int TWAActivity2StudentId { get; set; }
        public virtual int StudentId { get; set; }
        public virtual int TWAActivityId { get; set; }
        public virtual DateTime LastDateAssessed { get; set; }
        public virtual IList<TWAActivity2StudentLevelMastery> TWAActivity2StudentLevelMastery { get; set; }
        public virtual TWATutoringActivitiesTemplateActivity TwaTutoringActivitiesTemplateActivity { get; set; } 

    }
}
