using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// Methods to work with characters, such as an indexable ASCII table.
    /// </summary>
    public class CharUtilities
    {
        /// <summary>
        /// Determines whether [is character one of] [the specified c].
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="compare">The compare.</param>
        /// <returns>
        /// 	<c>true</c> if [is character one of] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCharacterOneOf(char c, params char[] compare)
        {
            if (compare != null)
            {
                for (int i = 0; i < compare.Length; i++)
                {
                    if (c == compare[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
