using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.TWADataModels;

namespace Services
{
    public interface ITWAService
    {
        void SaveMastery(TWAActivity2Student twaActivity2Student);
        IList<TWAActivity2Student> GetMasteryDetails(ActivityMasteryFilter filter);
    }
}
