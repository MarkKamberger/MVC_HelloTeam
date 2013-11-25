﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.DataMapping
{
    public static class DataTablePrimaryKeyTranslator
    {
        /// <summary>
        /// Translates the primary id.
        /// </summary>
        /// <param name="name">The table name.</param>
        /// <returns>A manually set name or an autogenerated name</returns>
        /// <remarks>This is what happens when naming conventions are not standardied & followed</remarks>
        public static string TranslatePrimaryId(string name)
        {
            string identityName;  
            switch (name)
            {
                case "TWATutoringActivitiesTemplateActivity":
                    identityName = "TutoringActivitiesTemplateActivityId";
                    break;
                case "_SALI_LoginWebUser":
                    identityName = "CustomerId";
                    break;
                case "_Mvc_ListNavigationLinks":
                    identityName = "Id";
                    break;
                case "_Mvc_ListNavigationChild":
                    identityName = "Id";
                    break;
                case "_Mvc_ListNavigationSpecialCustomer":
                    identityName = "Id";
                    break;
                case "_Mvc_ListNavigationSpecialUser":
                    identityName = "Id";
                    break;
                case "_Mvc_ListNavigationRole":
                    identityName = "Id";
                    break;
                case "_Mvc_ListNavigationChildRole":
                    identityName = "Id";
                    break;
                case "Members.dbo.LifetimeIndividualScore":
                    identityName = "StudentId";
                    break;
                default:
                    identityName = name + "Id";
                    break;
            }
            return identityName;
        }
    }
}
