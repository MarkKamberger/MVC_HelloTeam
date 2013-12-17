using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace DomainLayer.Helper
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T ConvertToEnum<T>(this string value)
        {
            Contract.Requires(typeof(T).IsEnum);
            Contract.Requires(value != null);
            Contract.Requires(Enum.IsDefined(typeof(T), value));
            if (value == "Full Access")
                value = "FullAccess";
            if (value == "Default Role")
                value = "DefaultRole";
            if (value == "View Only")
                value = "ViewOnly";
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
