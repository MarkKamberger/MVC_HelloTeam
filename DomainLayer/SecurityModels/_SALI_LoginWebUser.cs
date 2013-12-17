using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.Base;
using SharpArch.Domain.DomainModel;


namespace DomainLayer.SecurityModels
{
    /// <summary>
    /// StoredProcedure Mapping CustomerMgmt
    /// </summary>
   public class _SALI_LoginWebUser :Entity
    {
        public virtual string CustomerName { get; set; }
        public virtual int UserType { get; set; }

    }
}
