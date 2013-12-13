using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.LFSTools;
using Infrastructure.LFSRepository;

namespace Services.LFSService
{
    public class LFSService :ILFSService
    {
        private ILFSRepository _lfsRepository;
        public LFSService(ILFSRepository lfsRepository)
        {
            _lfsRepository = lfsRepository;
        }

        public IList<LFSGuideTypes> GetLFSGuideTypes()
        {
            return _lfsRepository.GetLFSGuideTypes();
        }
    }
}
