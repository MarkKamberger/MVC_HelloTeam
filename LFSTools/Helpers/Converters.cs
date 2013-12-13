using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LFSTools.Helpers
{
    public class Converters : BondedContentPage
    {
        /// <summary>
        /// Converts the enum to data table.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public DataTable ConvertEnumToDataTable(Type enumType)
        {
            return BondedMasterPage.BusinessLogicObject.ConvertEnumToDataTable(enumType);
        }
    }
}