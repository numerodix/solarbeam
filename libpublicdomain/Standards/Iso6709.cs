using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicDomain
{
    /// <summary>
    /// Represents a latitude and longitude.
    /// </summary>
    [Serializable]
    public struct Iso6709
    {
        /// <summary>
        /// 
        /// </summary>
        public int LatitudeDegrees;

        /// <summary>
        /// 
        /// </summary>
        public int LatitudeMinutes;

        /// <summary>
        /// 
        /// </summary>
        public int LatitudeSeconds;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLatitudeNorth;

        /// <summary>
        /// 
        /// </summary>
        public int LongitudeDegrees;

        /// <summary>
        /// 
        /// </summary>
        public int LongitudeMinutes;

        /// <summary>
        /// 
        /// </summary>
        public int LongitudeSeconds;

        /// <summary>
        /// 
        /// </summary>
        public bool IsLongitudeEast;

        /// <summary>
        /// 
        /// </summary>
        public static Regex Iso6709Form1 = new Regex(@"(\+|-)(\d\d)(\d\d)(\+|-)(\d\d\d)(\d\d)", RegexOptions.Compiled);

        /// <summary>
        /// 
        /// </summary>
        public static Regex Iso6709Form2 = new Regex(@"(\+|-)(\d\d)(\d\d)(\d\d)(\+|-)(\d\d\d)(\d\d)(\d\d)", RegexOptions.Compiled);

    
        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            if (LatitudeSeconds != 0 && LongitudeSeconds != 0)
            {
                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    IsLatitudeNorth ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LatitudeDegrees, 2),
                    StringUtilities.PadIntegerLeft(LatitudeMinutes, 2),
                    StringUtilities.PadIntegerLeft(LatitudeSeconds, 2),
                    IsLongitudeEast ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LongitudeDegrees, 3),
                    StringUtilities.PadIntegerLeft(LongitudeMinutes, 2),
                    StringUtilities.PadIntegerLeft(LongitudeSeconds, 2)
                );
            }
            else
            {
                return string.Format("{0}{1}{2}{3}{4}{5}",
                    IsLatitudeNorth ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LatitudeDegrees, 2),
                    StringUtilities.PadIntegerLeft(LatitudeMinutes, 2),
                    IsLongitudeEast ? '+' : '-',
                    StringUtilities.PadIntegerLeft(LongitudeDegrees, 3),
                    StringUtilities.PadIntegerLeft(LongitudeMinutes, 2)
                );
            }
        }
    }
}
