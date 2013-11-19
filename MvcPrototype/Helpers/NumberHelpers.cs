using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFAFUtilityObjects;

namespace MvcPrototype
{
    public static class NumberHelpers
    {
        /// <summary>
        /// Returns as int.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static int ReturnAsInt(object o)
        {
            string s = string.Empty;

            if (o != null)
                s = o.ToString();

            return ReturnAsInt(s);
        }

        /// <summary>
        /// Returns as int.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static int ReturnAsInt(string s)
        {
            int result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToInt32(s);
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }



        /// <summary>
        /// Returns as decimal.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static decimal ReturnAsDecimal(string s)
        {
            decimal result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToDecimal(s);
                    result += 0.00m;
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns as double.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static double ReturnAsDouble(string s)
        {
            double result = -1;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    result = Convert.ToDouble(s);
                }
                catch
                {
                    result = -1;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns as bool.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static bool ReturnAsBool(string s)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    s = s.ToLower().Trim();

                    switch (s)
                    {
                        case "1":
                        case "true":
                        case "yes":
                            result = true;
                            break;
                        default:
                            result = false;
                            break;
                    }
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }
    }
}