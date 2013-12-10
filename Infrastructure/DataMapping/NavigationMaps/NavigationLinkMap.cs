using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainLayer.NavigationModels;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Infrastructure.DataMapping.NavigationMaps
{
    public class NavigatioLinkMap : IAutoMappingOverride<NavigationLink>
    {

        public void Override(AutoMapping<NavigationLink> mapping)
        {
            mapping.Table("NavigationLink");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.Name);
            mapping.Map(x => x.Object);
            mapping.Map(x => x.Url);
            mapping.Map(x => x.TypeId);
            mapping.HasMany(x => x.NavigationChildren).Inverse()
                   .Cascade.AllDeleteOrphan().KeyColumn("NavigationLinkId").Not.LazyLoad();
            mapping.HasMany(x => x.SpecialUsers).Inverse()
                   .Cascade.AllDeleteOrphan().KeyColumn("NavigationLinkId").Not.LazyLoad();
            mapping.HasMany(x => x.SpecialCustomers).Inverse()
                   .Cascade.AllDeleteOrphan().KeyColumn("NavigationLinkId").Not.LazyLoad();
            mapping.HasMany(x => x.Roles).Inverse()
                   .Cascade.AllDeleteOrphan().KeyColumn("NavigationLinkId").Not.LazyLoad();

        }
    }

    public class NavigatioChildLinksnMap : IAutoMappingOverride<NavigationChild>
    {
        public void Override(AutoMapping<NavigationChild> mapping)
        {
            mapping.Table("NavigationChild");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.Name);
            mapping.Map(x => x.Object);
            mapping.Map(x => x.Url);
            mapping.Map(x => x.TypeId);
            mapping.HasMany(x => x.SpecialUsers).KeyColumn("NavigationChildId")
                  .Inverse()
                  .Cascade.AllDeleteOrphan().Not.LazyLoad();
            mapping.HasMany(x => x.SpecialCustomers).KeyColumn("NavigationChildId")
                  .Inverse()
                  .Cascade.AllDeleteOrphan().Not.LazyLoad();
            mapping.HasMany(x => x.ChildRoles).KeyColumn("NavigationChildId")
                  .Inverse()
                  .Cascade.AllDeleteOrphan().Not.LazyLoad();
            

        }
    }

    public class NavigationSpecialCustomerMap : IAutoMappingOverride<NavigationSpecialCustomer>
    {
        public void Override(AutoMapping<NavigationSpecialCustomer> mapping)
        {
            mapping.Table("NavigationSpecialCustomer");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.CustomerId);
        }
    }

    public class NavigationSpecialUserMap : IAutoMappingOverride<NavigationSpecialUser>
    {
        public void Override(AutoMapping<NavigationSpecialUser> mapping)
        {
            mapping.Table("NavigationSpecialUser");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.UserId);
        }
    }

    public class NavigationChildSpecialCustomerMap : IAutoMappingOverride<NavigationChildSpecialCustomer>
    {
        public void Override(AutoMapping<NavigationChildSpecialCustomer> mapping)
        {
            mapping.Table("NavigationChildSpecialCustomer");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.CustomerId);
        }
    }

    public class NavigationChildSpecialUserMap : IAutoMappingOverride<NavigationChildSpecialUser>
    {
        public void Override(AutoMapping<NavigationChildSpecialUser> mapping)
        {
            mapping.Table("NavigationChildSpecialUser");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.UserId);
        }
    }

    public class NavigationRoleMap : IAutoMappingOverride<NavigationLinkRole>
    {
        public void Override(AutoMapping<NavigationLinkRole> mapping)
        {
            mapping.Table("NavigationLinkRole");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.Role);
        }
    }

    public class NavigationChildRoleMap : IAutoMappingOverride<NavigationChildRole>
    {
        public void Override(AutoMapping<NavigationChildRole> mapping)
        {
            mapping.Table("NavigationChildRole");
            mapping.Id(x => x.Id);
            mapping.Map(x => x.Role);
        }
    }



}
