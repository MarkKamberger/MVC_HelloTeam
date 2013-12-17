using System;
using SharpArch.Domain.DomainModel;

namespace DomainLayer.Members
{
    /// <summary>
    /// Testing - Not Complete 
    /// </summary>
    public class LifetimeIndividualScore : Entity
    {
        public virtual int StudentId { get; set; }
        public virtual int SecondaryId { get; set; }
        public virtual int TertiaryId { get; set; }
        public virtual int ClassroomMeasureId { get; set; }
        public virtual decimal Score { get; set; }
        public virtual DateTime LastModified { get; set; }
        public virtual int LastModifiedBy { get; set; }
        public virtual int ScoreId { get; set; }
        public virtual DateTime MasteryDate { get; set; }
        public virtual int InstructionalLevelId { get; set; }
    }
}
