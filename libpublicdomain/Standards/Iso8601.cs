using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicDomain
{
    /// <summary>
    /// http://www.w3.org/TR/NOTE-datetime
    /// http://www.cl.cam.ac.uk/~mgk25/iso-time.html
    /// </summary>
    public class Iso8601
    {
        /// <summary>
        /// 
        /// </summary>
        public const char UtcZuluIdentifier = 'Z';

        private static Regex FormatYear, FormatYearAndMonth,
            FormatComplete, FormatCompleteHM,
            FormatCompleteHMS, FormatCompleteHMSF;

        static Iso8601()
        {
            string format = @"^(\d\d\d\d)";
            string tzd = @"(Z|((\+|-)\d\d:\d\d))";

            FormatYear = new Regex(format + "$");

            format += @"-?(\d\d)";
            FormatYearAndMonth = new Regex(format + "$");

            format += @"-?(\d\d)";
            FormatComplete = new Regex(format + "$");

            format += @"T(\d\d):(\d\d)";
            FormatCompleteHM = new Regex(format + tzd + "$");

            format += @":(\d\d)";
            FormatCompleteHMS = new Regex(format + tzd + "$");

            format += @".(\d+)";
            FormatCompleteHMSF = new Regex(format + tzd + "$");
        }
		
        private static void ThrowInvalidFormatException(string str)
        {
            throw new TzDatabase.TzParseException("Date/time does not conform to ISO 8601 format ({0}).", str);
        }

        /// <summary>
        /// Gets the time zone data.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="useZuluModifier">if set to <c>true</c> [use zulu modifier].</param>
        /// <returns></returns>
        public static string GetTimeZoneData(TimeSpan timeSpan, bool useZuluModifier)
        {
            string result;
            if (useZuluModifier && timeSpan.Hours == 0 && timeSpan.Minutes == 0 && timeSpan.Seconds == 0 && timeSpan.Milliseconds == 0)
            {
                result = UtcZuluIdentifier.ToString();
            }
            else
            {
                result = (DateTimeUtlities.IsTimeSpanNegative(timeSpan) ? "-" : "+") + string.Format("{0:##}:{1:##}", timeSpan.Hours, timeSpan.Minutes);
            }
            return result;
        }
    }
}
