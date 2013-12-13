using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.LFSTools;

namespace Services.LFSService
{
    public interface ILFSService
    {
        IList<LFSGuideTypes> GetLFSGuideTypes();
    }
}
