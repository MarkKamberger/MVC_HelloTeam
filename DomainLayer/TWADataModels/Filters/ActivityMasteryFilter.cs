using DomainLayer.Base;

namespace DomainLayer.TWADataModels
{
    public class ActivityMasteryFilter : BaseFilter
    {
        public int? StudentId { get; set; }
        public int? ActivityId { get; set; }
        public int? Activity2StudentId { get; set; }

    }
}
