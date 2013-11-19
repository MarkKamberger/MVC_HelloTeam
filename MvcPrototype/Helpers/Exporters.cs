using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using SFAFGlobalObjects;

namespace MvcPrototype.Helpers
{
    public class Exporters :BondedContentPage
    {

        /// <summary>
        /// Configures the export to excel.
        /// </summary>
        /// <param name="hl">The hl.</param>
        /// <param name="exportDataSet">The export data set.</param>
        public void ConfigureExportToExcel(HyperLink hl, string exportDataSet)
        {
            ConfigureExportToExcel(hl, exportDataSet, true);
        }

        /// <summary>
        /// Configures the export to excel.
        /// </summary>
        /// <param name="hl">The hl.</param>
        /// <param name="exportDataSet">The export data set.</param>
        /// <param name="addIcon">if set to <c>true</c> [add icon].</param>
        public void ConfigureExportToExcel(HyperLink hl, string exportDataSet, bool addIcon)
        {
            hl.NavigateUrl = "~/FrontOffice/Extracts.aspx?ExtractType=" + exportDataSet + "&FileType=" + (int)SALIExtractFileTypes.Excel;

            if (addIcon)
                hl.CssClass += " ExcelLink";
        }
    }
}