using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LFSTools.Models
{
    
    public class NavigationLinkModel
    {
        public NavigationLinkModel()
        {
            NavigationChildren = new List<NavigationChildren>();
        }
        public string Name { get; set; }
        public NavigationType Type { get; set; }
        public string Url { get; set; }
        public string Object { get; set; }
        public IList<NavigationChildren> NavigationChildren { get; set; } 
    }

    public class NavigationChildren
    {
        public string Name { get; set; }
        public NavigationType Type { get; set; }
        public string Url { get; set; }
        public string Object { get; set; }
    }
    public class SpecialLogins
    {
        public string UserName { get; set; }
    }
    public class SpecialCustomers
    {
        public string CustomerId { get; set; }
    }

    public enum NavigationType
    {
        Link = 1,
        DropDown
    }
}
