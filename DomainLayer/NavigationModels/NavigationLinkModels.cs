using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace DomainLayer.NavigationModels
{
  
    public class NavigationLink : Entity 
    {
        public NavigationLink()
        {
            NavigationChildren = new List<NavigationChild>();
            SpecialUsers = new List<NavigationSpecialUser>();
            SpecialCustomers = new List<NavigationSpecialCustomer>();
            Roles = new List<NavigationLinkRole>();
        }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Object { get; set; }
        public virtual NavigationType TypeId { get; set; }
        public virtual int ApplicationId { get; set; }
        public virtual IList<NavigationChild> NavigationChildren { get; set; }
        public virtual IList<NavigationSpecialCustomer> SpecialCustomers { get; set; }
        public virtual IList<NavigationSpecialUser> SpecialUsers { get; set; }
        public virtual IList<NavigationLinkRole> Roles { get; set; } 
    }
    public class NavigationChild : Entity
    {
        public NavigationChild()
        {
            SpecialCustomers = new List<NavigationChildSpecialCustomer>();
            SpecialUsers = new List<NavigationChildSpecialUser>();
            ChildRoles = new List<NavigationChildRole>();
        }
        public virtual string NavigationLinkId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual string Object { get; set; }
        public virtual NavigationType TypeId { get; set; }
        public virtual IList<NavigationChildSpecialCustomer> SpecialCustomers { get; set; }
        public virtual IList<NavigationChildSpecialUser> SpecialUsers { get; set; }
        public virtual IList<NavigationChildRole> ChildRoles { get; set; }
    }
    public class NavigationSpecialCustomer : Entity
    {
        public virtual int CustomerId { get; set; }
    }
    public class NavigationSpecialUser : Entity
    {
        public virtual int UserId { get; set; }
    }
    public class NavigationChildSpecialCustomer : Entity
    {
        public virtual int NavigationChildId { get; set; }
        public virtual int CustomerId { get; set; }
    }
    public class NavigationChildSpecialUser : Entity
    {
        public virtual int NavigationChildId { get; set; }
        public virtual int UserId { get; set; }
    }
    public class NavigationLinkRole : Entity
    {
        public virtual RoleSSO Role { get; set; }
    }
    public class NavigationChildRole : Entity
    {
        public virtual int NavigationChildId { get; set; }
        public virtual RoleSSO Role { get; set; }

    }
}
