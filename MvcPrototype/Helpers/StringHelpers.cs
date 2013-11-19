using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPrototype
{
    public static class StringHelpers 
    {
        /// <summary>
        /// Returns as string.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static string ReturnAsString(object o)
        {
            string result = string.Empty;

            if (o != null)
                result = o.ToString();

            return result;
        }

        /// <summary>
        /// Returns as HTML string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string ReturnAsHtmlString(string s)
        {
            string result = s.Trim();

            if (string.IsNullOrEmpty(result))
                result = "&nbsp;";

            return result;
        }
    }
}