using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.LFSTools;
using SharpArch.Domain.PersistenceSupport;

namespace Infrastructure.LFSRepository
{
    public interface ILFSRepository : ILinqRepositoryWithTypedId<LFSGuideTypes, int>
    {
        List<LFSGuideTypes> GetLFSGuideTypes();
        #region AreaOfConcern CRUD
        List<AchievementPlanAreaOfConcern> ListAreaOfConcern();
        void SaveAreaOfConcern(AchievementPlanAreaOfConcern areaOfConcern);
        #endregion AreaOfConcern
    }
}
