using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace DomainLayer.NavigationModels
{
    public class _Mvc_ListNavigationLinks : Entity
    {
        public _Mvc_ListNavigationLinks()
        {
            NavigationChildren = new List<_Mvc_ListNavigationChild>();
            SpecialUsers = new List<_Mvc_ListNavigationSpecialUser>();
            SpecialCustomers = new List<_Mvc_ListNavigationSpecialCustomer>();
        }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Object { get; set; }
        public virtual NavigationType TypeId { get; set; }
        public virtual IList<_Mvc_ListNavigationChild> NavigationChildren { get; set; }
        public virtual IList<_Mvc_ListNavigationSpecialCustomer> SpecialCustomers { get; set; }
        public virtual IList<_Mvc_ListNavigationSpecialUser> SpecialUsers { get; set; } 
    }

   

    public class _Mvc_ListNavigationChild :Entity
    {
        public _Mvc_ListNavigationChild()
        {
            SpecialCustomers = new List<_Mvc_ListNavigationChildSpecialCustomer>();
            SpecialUsers = new List<_Mvc_ListNavigationChildSpecialUser>();
        }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Object { get; set; }
        public virtual NavigationType TypeId { get; set; }
        public virtual IList<_Mvc_ListNavigationChildSpecialCustomer> SpecialCustomers { get; set; }
        public virtual IList<_Mvc_ListNavigationChildSpecialUser> SpecialUsers { get; set; } 
    }
    public class _Mvc_ListNavigationSpecialCustomer :Entity
    {
        public virtual int CustomerId { get; set; }
    }
    public class _Mvc_ListNavigationSpecialUser : Entity
    {
        public virtual int UserId { get; set; }
    }
    public class _Mvc_ListNavigationChildSpecialCustomer : Entity
    {
        public virtual int CustomerId { get; set; }
    }
    public class _Mvc_ListNavigationChildSpecialUser : Entity
    {
        public virtual int UserId { get; set; }
    }
    public class _Mvc_ListNavigationRole :Entity
    {
        public virtual RoleSSO Role { get; set; }
    }
    public class _Mvc_ListNavigationChildRole : Entity
    {
        public virtual RoleSSO Role { get; set; }
        
    }
}
