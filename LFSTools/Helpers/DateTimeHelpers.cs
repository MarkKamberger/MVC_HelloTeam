using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFAFGlobalObjects;

namespace LFSTools.Helpers
{
    public static class DateTimeHelpers 
    {
        /// <summary>
        /// Converts the date time to short string.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static string ConvertDateTimeToShortString(DateTime dt)
        {
            string result = string.Empty;

            if (dt.CompareTo(SFAFConstants.SFAFNullDate) > 0 && dt.CompareTo(SFAFConstants.SFAFNullDate2) > 0)
                result = dt.ToShortDateString();

            return result;
        }

        /// <summary>
        /// Returns as date time.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static DateTime ReturnAsDateTime(string s)
        {
            DateTime result = SFAFConstants.SFAFNullDate;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToDateTime(s);

                    //if (IsDateBefore(result, SFAFConstants.SFAFNullDate))
                        result = SFAFConstants.SFAFNullDate;
                }
                catch
                {
                    result = SFAFConstants.SFAFNullDate;
                }
            }

            return result;
        }
    }
}