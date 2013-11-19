using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.TWADataModels;
using Infrastructure.TWARepository;


namespace Services
{
    public class TWAService : ITWAService
    {
        #region Fields
        private readonly IStudent2ActivityRepository _repository;
        #endregion

        #region Constructor
        public TWAService(IStudent2ActivityRepository repository)
        {
            _repository = repository;
        }
        #endregion
        

        public void SaveMastery(TWAActivity2Student twaActivity2Student)
        {
           _repository.Save(twaActivity2Student);
        }

        public IList<TWAActivity2Student> GetMasteryDetails(ActivityMasteryFilter filter)
        {
            var student2Activities = _repository.Search(filter).ToList();
            return student2Activities;
        }
    }
}
