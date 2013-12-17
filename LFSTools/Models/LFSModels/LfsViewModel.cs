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
        }
        public GuideTypeModel GuideTypes { get; set; }
        public OptionSelectList SelectList { get; set; }

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
   
    public class EnumerableSelectList
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}