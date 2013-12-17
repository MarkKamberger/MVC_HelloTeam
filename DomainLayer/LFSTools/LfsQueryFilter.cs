using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.Base;

namespace DomainLayer.LFSTools
{
    /// <summary>
    /// filter with optinal parameters for querying 
    /// </summary>
    public class LfsQueryFilter : BaseFilter
    {
        public int? CategoryId { get; set; }
        public int? AreaOfConcernId { get; set; }
        public int? SubAreaOfConcernId { get; set; }
        public int? TargetId { get; set; }
        public int? RootCauseId { get; set; }
        public int? LeveragePointId { get; set; }
 
    }
}
