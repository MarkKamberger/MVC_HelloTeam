using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Helper
{
   public static class HelperClass
    {
        /// <summary>
        /// Parses the enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
       public static T ParseEnum<T>(string value)
       {
           return (T)Enum.Parse(typeof(T), value, true);
       }
    }
}
