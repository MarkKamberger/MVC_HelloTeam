using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace DomainLayer.TWADataModels
{
    public class TWAActivity2StudentLevelMastery : Entity
    {
        public virtual int TWAActivity2StudentId { get; set; }
        public virtual int ActivityLevel { get; set; }
        public virtual bool Mastered { get; set; }
        public virtual int NumberOfPresentations { get; set; }
        public virtual int CorrectInARow { get; set; }
        public virtual int InCorrectInARow { get; set; }
        public virtual DateTime DateLastAssessed { get; set; }
        public virtual int SessionNumber { get; set; }
        public virtual bool IncrementNew { get; set; }
        public virtual int MeasureIndex { get; set; }
        public virtual int ActivityStartCount { get; set; }
        public virtual int OverMastered { get; set; }
        public virtual int SharedStoryId { get; set; }
        



    }
}
