using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.NavigationModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping.NavigationMaps
{
    public class _Mvc_ListNavigationLinksMap : IAutoMappingOverride<_Mvc_ListNavigationLinks>
    {
        /// <summary>
        /// Overrides the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Override(AutoMapping<_Mvc_ListNavigationLinks> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationLinkId").GeneratedBy.Assigned();
            mapping.Map(x => x.Name);
            mapping.Map(x => x.Url);
            mapping.Map(x => x.Object);
            mapping.Map(x => x.TypeId);
        }
    }
    public class _Mvc_ListNavigationChildMap : IAutoMappingOverride<_Mvc_ListNavigationChild>
    {
        /// <summary>
        /// Overrides the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Override(AutoMapping<_Mvc_ListNavigationChild> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationChildId").GeneratedBy.Assigned();
            mapping.Map(x => x.Name);
            mapping.Map(x => x.Url);
            mapping.Map(x => x.Object);
            mapping.Map(x => x.TypeId);
        }
    }
    public class _Mvc_ListNavigationSpecialUserMap : IAutoMappingOverride<_Mvc_ListNavigationSpecialUser>
    {
        /// <summary>
        /// Overrides the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Override(AutoMapping<_Mvc_ListNavigationSpecialUser> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationSpecialUserId").GeneratedBy.Assigned();
            mapping.Map(x => x.UserId);
        }
    }
    public class _Mvc_ListNavigationSpecialCustomerMap : IAutoMappingOverride<_Mvc_ListNavigationSpecialCustomer>
    {
        /// <summary>
        /// Overrides the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Override(AutoMapping<_Mvc_ListNavigationSpecialCustomer> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationSpecialCustomerId").GeneratedBy.Assigned();
            mapping.Map(x => x.CustomerId);
        }
    }

    public class _Mvc_ListNavigationChildSpecialCustomerMap : IAutoMappingOverride<_Mvc_ListNavigationChildSpecialCustomer>
    {
        /// <summary>
        /// Overrides the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Override(AutoMapping<_Mvc_ListNavigationChildSpecialCustomer> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationChildSpecialCustomerId").GeneratedBy.Assigned();
            mapping.Map(x => x.CustomerId);
        }
    }
    public class _Mvc_ListNavigationChildSpecialUserMap : IAutoMappingOverride<_Mvc_ListNavigationChildSpecialUser>
    {
        /// <summary>
        /// Overrides the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Override(AutoMapping<_Mvc_ListNavigationChildSpecialUser> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationChildSpecialUserId").GeneratedBy.Assigned();
            mapping.Map(x => x.UserId);
        }
    }

    public class _Mvc_ListNavigationRoleMap :IAutoMappingOverride<_Mvc_ListNavigationRole>
    {
        public void Override(AutoMapping<_Mvc_ListNavigationRole> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationLinkRoleId").GeneratedBy.Assigned();
            mapping.Map(x => x.Role);
        }
    }

    public class _Mvc_ListNavigationChildRoleMap :IAutoMappingOverride<_Mvc_ListNavigationChildRole>
    {
        public void Override(AutoMapping<_Mvc_ListNavigationChildRole> mapping)
        {
            mapping.Id(x => x.Id).Column("NavigationChildRoleId").GeneratedBy.Assigned();
            mapping.Map(x => x.Role);
        }
    }
}
