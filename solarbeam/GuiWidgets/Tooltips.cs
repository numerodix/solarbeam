// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace SolarbeamGui
{
	static class Tooltips
	{
		// Location
		
		public static string LocationTitle
		{ get { return "Location"; } }
		
		public static string Location
		{ get { return "Select an existing location or enter a new one"; } }
		
		// Latitude
		
		public static string LatitudeTitle
		{ get { return "Latitude"; } }
		
		public static string LatitudeDegree
		{ get { return String.Format(
					"Enter the number of degrees latitude ({0}-{1})",
					Position.LATDEGS_MINVALUE,
					Position.LATDEGS_MAXVALUE);
		} }

		public static string LatitudeMinute
		{ get { return String.Format(
					"Enter the number of minutes latitude ({0}-{1})",
					Position.LATMINS_MINVALUE,
					Position.LATMINS_MAXVALUE);
		} }

		public static string LatitudeSecond
		{ get { return String.Format(
					"Enter the number of seconds latitude ({0}-{1})",
					Position.LATSECS_MINVALUE,
					Position.LATSECS_MAXVALUE);
		} }

		// Longitude
		
		public static string LongitudeTitle
		{ get { return "Longitude"; } }

		public static string LongitudeDegree
		{ get { return String.Format(
					"Enter the number of degrees longitude ({0}-{1})",
					Position.LONDEGS_MINVALUE,
					Position.LONDEGS_MAXVALUE);
		} }

		public static string LongitudeMinute
		{ get { return String.Format(
					"Enter the number of minutes longitude ({0}-{1})",
					Position.LONMINS_MINVALUE,
					Position.LONMINS_MAXVALUE);
		} }

		public static string LongitudeSecond
		{ get { return String.Format(
					"Enter the number of seconds longitude ({0}-{1})",
					Position.LONSECS_MINVALUE,
					Position.LONSECS_MAXVALUE);
		} }
		
		// Timezone
		
		public static string TimezoneTitle
		{ get { return "Timezone"; } }
		
		public static string TimezoneOffset
		{ get { return "Select the UTC offset your timezone is located in"; } }
		
		public static string TimezoneZone
		{ get { return "Select your timezone by name"; } }
		
		// Date
		
		public static string DateTitle
		{ get { return "Date"; } }

		public static string DateDay
		{ get { return String.Format(
					"Enter the day of the month ({0}-{1})",
					UTCDate.DAY_MINVALUE,
					UTCDate.DAY_MAXVALUE);
		} }

		public static string DateMonth
		{ get { return String.Format(
					"Enter the month of the year ({0}-{1})",
					UTCDate.MONTH_MINVALUE,
					UTCDate.MONTH_MAXVALUE);
		} }

		public static string DateYear
		{ get { return String.Format(
					"Enter the year ({0}-{1})",
					UTCDate.YEAR_MINVALUE,
					UTCDate.YEAR_MAXVALUE);
		} }
		
		// Time
		
		public static string TimeTitle
		{ get { return "Time"; } }

		public static string TimeHour
		{ get { return String.Format(
					"Enter the number of hours ({0}-{1})",
					UTCDate.HOUR_MINVALUE,
					UTCDate.HOUR_MAXVALUE);
		} }

		public static string TimeMinute
		{ get { return String.Format(
					"Enter the number of minutes ({0}-{1})",
					UTCDate.MINUTE_MINVALUE,
					UTCDate.MINUTE_MAXVALUE);
		} }

		public static string TimeSecond
		{ get { return String.Format(
					"Enter the number of seconds ({0}-{1})",
					UTCDate.SECOND_MINVALUE,
					UTCDate.SECOND_MAXVALUE);
		} }
	}
}
