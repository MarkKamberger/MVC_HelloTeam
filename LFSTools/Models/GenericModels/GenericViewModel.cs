using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LFSTools.BaseModels;

namespace LFSTools.Models.GenericModels
{
    public class GenericViewModel :BaseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}