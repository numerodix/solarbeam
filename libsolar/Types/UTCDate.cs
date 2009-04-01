// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Globalization;

namespace LibSolar.Types
{
	/**
	 * This struct overlays the built in DateTime struct in order to provide a UTC
	 * safe date object. Internally, it holds relevant data, like timezone and dst
	 * info.
	 * For the purpose of Solarbeam, the struct implements the same exact interface,
	 * except it's timezone-safe.
	 */
	public struct UTCDate
	{
		public const int TIMEZONE_MINVALUE = -12;
		public const int TIMEZONE_MAXVALUE = 14;
		public const int YEAR_MINVALUE = 2;
		public const int YEAR_MAXVALUE = 3000;
		public const int MONTH_MINVALUE = 1;
		public const int MONTH_MAXVALUE = 12;
		public const int DAY_MINVALUE = 1;
		public const int DAY_MAXVALUE = 31;
		public const int HOUR_MINVALUE = 0;
		public const int HOUR_MAXVALUE = 23;
		public const int MINUTE_MINVALUE = 0;
		public const int MINUTE_MAXVALUE = 59;
		public const int SECOND_MINVALUE = 0;
		public const int SECOND_MAXVALUE = 59;
		
		private double tz;
		private DateTime dt;
		private DaylightTime dst;

		/**
		 * Create an instance resetting the time to UTC
		 */
		public UTCDate(double tz,
					int year, int month, int day,
					int hour, int min, int sec)
		{
			this.tz = tz;
			this.dst = null;
			this.dt = new DateTime(year, month, day, hour, min, sec,
								   DateTimeKind.Utc).AddHours(-tz);

			CheckTimezone(this.tz);
		}
		
		public UTCDate(double tz, DaylightTime dst,
					int year, int month, int day,
					int hour, int min, int sec)
		{
			this.tz = tz;
			this.dst = dst;
			this.dt = new DateTime(year, month, day, hour, min, sec,
								   DateTimeKind.Utc).AddHours(-tz);

			CheckTimezone(this.tz);
		}

		private UTCDate(double tz, DateTime dt)
		{
			this.tz = tz;
			this.dst = null;
			this.dt = dt;

			CheckTimezone(this.tz);
		}

		/**
		 * Reference: http://en.wikipedia.org/wiki/List_of_time_zones
		 */
		private void CheckTimezone(double tz)
		{
			if ( (tz < TIMEZONE_MINVALUE) || (tz > TIMEZONE_MAXVALUE) )
			{
				throw new ArgumentException(
						string.Format("Bad value for timezone: {0}", tz));
			}
		}
    
		/**
		 * Clone current date, but set time to null.
		 */
		public UTCDate AtStartOfDay()
		{
			// build a Date with null time
			// but we cannot return this, the timezone setting is invalid
			UTCDate new_dt = new UTCDate(0, this.Year, this.Month, this.Day, 0, 0, 0);
			
			// use manufactured Date to extract correct DateTime object,
			// then build another Date with the right DateTime and timezone
			return new UTCDate(tz, new_dt.ExtractUTC());
		}

		// Extract DateTime result
		public DateTime ExtractLocaltime()
		{
			double dst_delta = 0;
			if ((this.dst != null) && (dst.Start.CompareTo(dst.End) != 0)) {
				if ((dst.Start < this.dt) && (this.dt < dst.End)) {
					dst_delta = dst.Delta.TotalHours;
				}
			}
			DateTime dt_n = new DateTime(dt.Year, dt.Month, dt.Day, 
			                             dt.Hour, dt.Minute, dt.Second, 
			                             DateTimeKind.Local);
			dt_n = dt_n.AddHours(tz+dst_delta);
			return dt_n;
		}

		public DateTime ExtractUTC()
		{
			return dt;
		}

		// Provide partial DateTime interface. Everything is UTC based

		public UTCDate AddDays(double days)
		{
			DateTime new_dt = dt.AddDays(days);
			return new UTCDate(tz, new_dt);
		}
		
		public UTCDate AddHours(double hours)
		{
			DateTime new_dt = dt.AddHours(hours);
			return new UTCDate(tz, new_dt);
		}

		/**
		 * Overlay the AddMinutes method in DateTime, but careful not to touch the
		 * instance attributes here, so do all work on fresh object!
		 */
		public UTCDate AddMinutes(double mins)
		{
			DateTime new_dt = dt.AddMinutes(mins);
			return new UTCDate(tz, new_dt);
		}

		public UTCDate AddSeconds(double secs)
		{
			DateTime new_dt = dt.AddSeconds(secs);
			return new UTCDate(tz, new_dt);
		}
		
		public int CompareTo(UTCDate dt)
		{
			return this.ExtractLocaltime().CompareTo(dt.ExtractLocaltime());
		}

		public double Timezone
		{ get { return tz; } }

		public int Year
		{ get { return dt.Year; } }

		public int Month
		{ get { return dt.Month; } }

		public int Day
		{ get { return dt.Day; } }

		public int Hour
		{ get { return dt.Hour; } }

		public int Minute
		{ get { return dt.Minute; } }

		public int Second
		{ get { return dt.Second; } }

		// Helpers
		public string Print()
		{
			DateTime dt_utc = ExtractUTC();
			DateTime dt_local = ExtractLocaltime();
			string fmt = "HH':'mm':'ss' 'dd'.'MM'.'yyyy";
			return string.Format("{0}  [{1} UTC]",
								 dt_local.ToString(fmt), dt_utc.ToString(fmt));
		}

		public string PrintTimezone()
		{
			char sign = '+';
			if (tz < 0)
			{
				sign = '-';
			}
			double val = Math.Abs(tz);
			return string.Format("{0}{1}", sign, val);
		}
	}
}
