using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace PublicDomain
{
    /// <summary>
    /// Common conversion tasks such as parsing string values into various types.
    /// </summary>
    public static class ConversionUtilities
    {
        /// <summary>
        /// Determines whether [is string an integer] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger(string str)
        {
            return IsStringAnInteger64(str);
        }

        /// <summary>
        /// Determines whether [is string an integer64] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        /// 	<c>true</c> if [is string an integer64] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringAnInteger64(string str)
        {
            Int64 trash;
            return Int64.TryParse(str, out trash);
        }
    }
}
