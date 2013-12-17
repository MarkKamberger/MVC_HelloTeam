using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Components.DictionaryAdapter;
using DomainLayer.LFSTools;
using LFSTools.BaseModels;

namespace LFSTools.Models.LFSModels
{
    public class LfsViewModel :BaseModel
    {
        public LfsViewModel()
        {
            GuideTypes = new GuideTypeModel {SelectList = new BindingList<LFSGuideTypes>()};
            SelectList = new OptionSelectList{SelectList = new BindingList<EnumerableSelectList>()};
           /* AreasOfConcern = new AreaOfConcern { SelectList = new BindingList<EnumerableSelectList>() };
            SubAreaaOfConcern = new SubAreaOfConcern { SelectList = new BindingList<EnumerableSelectList>() };
            RootCauses = new RootCause { SelectList = new BindingList<EnumerableSelectList>() };
            Targets = new TargetList { SelectList = new BindingList<EnumerableSelectList>() };
            Focus = new LeveragePoint { SelectList = new BindingList<EnumerableSelectList>() };
            Action = new Action { SelectList = new BindingList<EnumerableSelectList>() };*/
        }
        public GuideTypeModel GuideTypes { get; set; }
        public OptionSelectList SelectList { get; set; }
       /* public SubAreaOfConcern SubAreaaOfConcern { get; set; }
        public RootCause RootCauses { get; set; } 
        public TargetList Targets { get; set; }
        public LeveragePoint Focus { get; set; }
        public Action Action { get; set; }
        public LfsQueryFilter InputModel { get; set; }*/
    }

    public class OptionSelectList
    {
        public int SelectedItemId { get; set; }
        public IEnumerable<EnumerableSelectList> SelectList { get; set; }
    }
    public class GuideTypeModel
    {
        public int SelectedItemId { get; set; }
        public IEnumerable<LFSGuideTypes> SelectList { get; set; } 
    }
    /*
    public class AreaOfConcern
    {
      
        public int SelectedItemId { get; set; }
        public IEnumerable<EnumerableSelectList> SelectList { get; set; } 
    }
    public class SubAreaOfConcern
    {
        public int SelectedItemId { get; set; }
        public IEnumerable<EnumerableSelectList> SelectList { get; set; }
    }
    public class RootCause
    {
        public int SelectedItemId { get; set; }
        public IEnumerable<EnumerableSelectList> SelectList { get; set; } 
    }
    public class TargetList
    {
        public int SelectedItemId { get; set; }
        public IEnumerable<EnumerableSelectList> SelectList { get; set; } 
    }
    public class LeveragePoint
    {
        public int SelectedItemId { get; set; }
        public IEnumerable<EnumerableSelectList> SelectList { get; set; } 
    }
    public class Action
    {
        public int SelectedItem { get; set; }
        public IEnumerable<EnumerableSelectList> SelectList { get; set; } 
    }*/
    public class EnumerableSelectList
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}