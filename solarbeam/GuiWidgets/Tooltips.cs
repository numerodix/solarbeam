// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace SolarbeamGui
{
	static class Tooltips
	{
		// Location
		
		public static string LocTipTitle
		{ get { return "Location"; } }
		
		public static string LocTip
		{ get { return "Select an existing location or enter a new one"; } }
		
		// Latitude
		
		public static string LatTipTitle
		{ get { return "Latitude"; } }
		
		public static string LatTipDegree
		{ get { return String.Format(
					"Enter the number of degrees latitude ({0}-{1})",
					Position.LATDEGS_MINVALUE,
					Position.LATDEGS_MAXVALUE);
		} }

		public static string LatTipMinute
		{ get { return String.Format(
					"Enter the number of minutes latitude ({0}-{1})",
					Position.LATMINS_MINVALUE,
					Position.LATMINS_MAXVALUE);
		} }

		public static string LatTipSecond
		{ get { return String.Format(
					"Enter the number of seconds latitude ({0}-{1})",
					Position.LATSECS_MINVALUE,
					Position.LATSECS_MAXVALUE);
		} }

		// Longitude
		
		public static string LonTipTitle
		{ get { return "Longitude"; } }

		public static string LonTipDegree
		{ get { return String.Format(
					"Enter the number of degrees longitude ({0}-{1})",
					Position.LONDEGS_MINVALUE,
					Position.LONDEGS_MAXVALUE);
		} }

		public static string LonTipMinute
		{ get { return String.Format(
					"Enter the number of minutes longitude ({0}-{1})",
					Position.LONMINS_MINVALUE,
					Position.LONMINS_MAXVALUE);
		} }

		public static string LonTipSecond
		{ get { return String.Format(
					"Enter the number of seconds longitude ({0}-{1})",
					Position.LONSECS_MINVALUE,
					Position.LONSECS_MAXVALUE);
		} }
		
		// Timezone
		
		public static string TzTipTitle
		{ get { return "Timezone"; } }
		
		public static string TzTipOffset
		{ get { return "Select the UTC offset your timezone is located in"; } }
		
		public static string TzTipZone
		{ get { return "Select your timezone by name"; } }
		
		// Date
		
		public static string DateTipTitle
		{ get { return "Date"; } }

		public static string DateTipDay
		{ get { return String.Format(
					"Enter the day of the month ({0}-{1})",
					UTCDate.DAY_MINVALUE,
					UTCDate.DAY_MAXVALUE);
		} }

		public static string DateTipMonth
		{ get { return String.Format(
					"Enter the month of the year ({0}-{1})",
					UTCDate.MONTH_MINVALUE,
					UTCDate.MONTH_MAXVALUE);
		} }

		public static string DateTipYear
		{ get { return String.Format(
					"Enter the year ({0}-{1})",
					UTCDate.YEAR_MINVALUE,
					UTCDate.YEAR_MAXVALUE);
		} }
		
		// Time
		
		public static string TimeTipTitle
		{ get { return "Time"; } }

		public static string TimeTipHour
		{ get { return String.Format(
					"Enter the number of hours ({0}-{1})",
					UTCDate.HOUR_MINVALUE,
					UTCDate.HOUR_MAXVALUE);
		} }

		public static string TimeTipMinute
		{ get { return String.Format(
					"Enter the number of minutes ({0}-{1})",
					UTCDate.MINUTE_MINVALUE,
					UTCDate.MINUTE_MAXVALUE);
		} }

		public static string TimeTipSecond
		{ get { return String.Format(
					"Enter the number of seconds ({0}-{1})",
					UTCDate.SECOND_MINVALUE,
					UTCDate.SECOND_MAXVALUE);
		} }
	}
}
