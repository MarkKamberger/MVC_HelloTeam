using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.SecurityModels;
using DomainLayer.TWADataModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping.SecurityMaps
{
    public class _SALI_LoginWebUserMap : IAutoMappingOverride<_SALI_LoginWebUser>
    {
        public void Override(AutoMapping<_SALI_LoginWebUser> mapping)
        {
            mapping.Id(x => x.Id).GeneratedBy.Identity();
            mapping.Map(x => x.CustomerName);
            mapping.Map(x => x.UserType);
        }
    }
}
