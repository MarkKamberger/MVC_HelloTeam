using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Domain.DomainModel;

namespace DomainLayer.NavigationModels
{

    /// <summary>
    /// MvcApplication.NavigationLink Data Model 
    /// </summary>
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
    /// <summary>
    /// MvcApplication.NavigationChild Data Model 
    /// </summary>
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
    /// <summary>
    /// MvcApplication.NavigationSpecialCustomer Data Model 
    /// </summary>
    public class NavigationSpecialCustomer : Entity
    {
        public virtual int CustomerId { get; set; }
    }
    /// <summary>
    /// MvcApplication.NavigationSpecialUser Data Model 
    /// </summary>
    public class NavigationSpecialUser : Entity
    {
        public virtual int UserId { get; set; }
    }
    /// <summary>
    /// MvcApplication.NavigationChildSpecialCustomer Data Model 
    /// </summary>
    public class NavigationChildSpecialCustomer : Entity
    {
        public virtual int CustomerId { get; set; }
    }
    /// <summary>
    /// MvcApplication.NavigationChildSpecialUser Data Model 
    /// </summary>
    public class NavigationChildSpecialUser : Entity
    {
        public virtual int UserId { get; set; }
    }
    /// <summary>
    /// MvcApplication.NavigationLinkRole Data Model 
    /// </summary>
    public class NavigationLinkRole : Entity
    {
        public virtual RoleSSO Role { get; set; }
    }
    /// <summary>
    /// MvcApplication.NavigationChildRole Data Model 
    /// </summary>
    public class NavigationChildRole : Entity
    {
        public virtual RoleSSO Role { get; set; }

    }
}
