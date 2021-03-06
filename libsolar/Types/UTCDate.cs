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
		public const double TIMEZONE_MIN = -12;
		public const double TIMEZONE_MAX = 14;
		public const int YEAR_MIN = 2;
		public const int YEAR_MAX = 3000;
		public const int MONTH_MIN = 1;
		public const int MONTH_MAX = 12;
		public const int DAY_MIN = 1;
		public const int DAY_MAX = 31;
		public const int HOUR_MIN = 0;
		public const int HOUR_MAX = 23;
		public const int MINUTE_MIN = 0;
		public const int MINUTE_MAX = 59;
		public const int SECOND_MIN = 0;
		public const int SECOND_MAX = 59;
		
		private double tz;
		private DaylightTime dst;
		private DateTime dt;

		// ##########################################################
		// ### Constructors 
		// ##########################################################
		
		/**
		 * Create an instance resetting the time to UTC
		 */
		public UTCDate(double tz, DaylightTime dst,
		               int year, int month, int day,
		               int hour, int min, int sec)
		{
			this.tz = tz;
			this.dst = dst;
			DateTime dt = new DateTime(year, month, day, hour, min, sec,
			                           DateTimeKind.Utc);
			dt = ResolveTimezone(dt, tz);
			this.dt = ResolveDST(dt, dst);

			CheckTimezone(this.tz);
		}

		private UTCDate(double tz, DaylightTime dst, DateTime dt)
		{
			this.tz = tz;
			this.dst = dst;
			this.dt = dt;
		}

		// ##########################################################
		// ### Construction checkers
		// ##########################################################
		
		/**
		 * Reference: http://en.wikipedia.org/wiki/List_of_time_zones
		 */
		private void CheckTimezone(double tz)
		{
			if ( (tz < TIMEZONE_MIN) || (tz > TIMEZONE_MAX) )
			{
				throw new ArgumentException(
						string.Format("Bad value for timezone: {0}", tz));
			}
		}
		
		// ##########################################################
		// ### Adjuster methods
		// ##########################################################
		
		/**
		 * Obtain absolute date (utc) from relative (local).
		 */
		public static DateTime ResolveTimezone(DateTime dt, double tz)
		{
			// timezone is UTC+x -> x hours ahead of UTC -> subtract tz
			// timezone is UTC-x -> x hours behind UTC -> add tz
			return dt.AddHours(-tz);
		}
		
		/**
		 * Obtain relative date (local) from absolute (utc).
		 */
		public static DateTime ApplyTimezone(DateTime dt, double tz)
		{
			return dt.AddHours(tz);
		}
		
		/**
		 * Obtain absolute date (utc) from relative (local).
		 */
		public static DateTime ResolveDST(DateTime dt, DaylightTime dst)
		{
			if (IsWithinDST(dt, dst)) {
				dt = dt.Add(-dst.Delta);
			}
			return dt;
		}
		
		/**
		 * Obtain relative date (local) from absolute (utc).
		 */
		public static DateTime ApplyDST(DateTime dt, DaylightTime dst)
		{
			if (IsWithinDST(dt, dst)) {
				dt = dt.Add(dst.Delta);
			}
			return dt;
		}
    
		// ##########################################################
		// ### Common public API static methods
		// ##########################################################
		
		public static bool IsWithinDST(DateTime dt, DaylightTime dst)
		{
			bool v = false;
			if (IsNonZero(dst)) {
				if ((dst.Start < dt) && (dt < dst.End)) {
					v = true;
				}
			}
			return v;
		}
		
		private static bool IsNonZero(DaylightTime dst)
		{
			bool v = false;
			if ((dst != null) && (dst.Start.CompareTo(dst.End) != 0)) {
				v = true;
			}
			return v;
		}

		// ##########################################################
		// ### Common public API instance methods
		// ##########################################################
		
		
		public UTCDate SetDate(int day, int month)
		{
			return new UTCDate(this.Timezone, this.DST,
			                   this.Year, month, day,
			                   this.Hour, this.Minute, this.Second);
		}
		
		public UTCDate SetHour(int hour)
		{
			return new UTCDate(this.Timezone, this.DST,
			                   this.Year, this.Month, this.Day,
			                   hour, this.Minute, this.Second);
		}
		
		
		/**
		 * Clone current date, but set time to null.
		 */
		public UTCDate AtStartOfUTCDay()
		{
			// build a Date with null time
			// set tz and dst to nil to prevent time adjustments, this time is
			// given as utc
			// but we cannot return this, the timezone setting is invalid
			UTCDate new_udt = new UTCDate(0, null, 
			                             this.Year, this.Month, this.Day, 
			                             0, 0, 0);
			
			// use manufactured Date to extract correct DateTime object,
			// then build another Date with the right DateTime and timezone
			return new UTCDate(tz, dst, new_udt.ExtractUTC());
		}
		
		/**
		 * Shift time to disregard dst, output standard time.
		 */
		public UTCDate AsStandard()
		{
			DateTime new_dt = dt.AddSeconds(0); // clone dt
			new_dt = ApplyDST(new_dt, dst);
			return new UTCDate(tz, dst, new_dt);
		}

		/**
		 * Get a fully applied local time.
		 */
		public DateTime ExtractLocal()
		{
			// create new DateTime object with kind Local
			DateTime dt_n = new DateTime(dt.Year, dt.Month, dt.Day, 
			                             dt.Hour, dt.Minute, dt.Second, 
			                             DateTimeKind.Local);
			dt_n = ApplyTimezone(dt_n, this.tz);
			dt_n = ApplyDST(dt_n, this.dst);
			return dt_n;
		}
		
		/**
		 * Get local time but without dst applied.
		 */
		public DateTime ExtractStandard()
		{
			// create new DateTime object with kind Local
			DateTime dt_n = new DateTime(dt.Year, dt.Month, dt.Day, 
			                             dt.Hour, dt.Minute, dt.Second, 
			                             DateTimeKind.Local);
			dt_n = ApplyTimezone(dt_n, this.tz);
			return dt_n;
		}

		public DateTime ExtractUTC()
		{
			return dt;
		}
		
		/**
		 * This time object has dst applicable to some period of the year.
		 */
		public bool HasDST
		{ get {
			return IsNonZero(this.dst);
		} }
		
		/**
		 * This concrete time is daylight saving time.
		 */
		public bool IsDST
		{ get {
			bool v = false;
			DateTime dt = ExtractLocal(); // dst is local time
			if (IsWithinDST(dt, dst)) {
				v = true;
			}
			return v;
		} }
		
		// ##########################################################
		// ### Partial adaption of DateTime interface
		// ##########################################################
		
		public UTCDate AddDays(double days)
		{
			DateTime new_dt = dt.AddDays(days);
			return new UTCDate(tz, dst, new_dt);
		}
		
		public UTCDate AddHours(double hours)
		{
			DateTime new_dt = dt.AddHours(hours);
			return new UTCDate(tz, dst, new_dt);
		}

		public UTCDate AddMinutes(double mins)
		{
			DateTime new_dt = dt.AddMinutes(mins);
			return new UTCDate(tz, dst, new_dt);
		}

		public UTCDate AddSeconds(double secs)
		{
			DateTime new_dt = dt.AddSeconds(secs);
			return new UTCDate(tz, dst, new_dt);
		}
		
		// ##########################################################
		// ### Operators
		// ##########################################################
		
		public static TimeSpan operator -(UTCDate a, UTCDate b)
		{
			return a.ExtractUTC() - b.ExtractUTC();
		}
		
		public static bool operator <(UTCDate a, UTCDate b)
		{
			return a.ExtractUTC() < b.ExtractUTC();
		}
		
		public static bool operator <=(UTCDate a, UTCDate b)
		{
			return a.ExtractUTC() <= b.ExtractUTC();
		}
		
		public static bool operator >(UTCDate a, UTCDate b)
		{
			return a.ExtractUTC() > b.ExtractUTC();
		}
		
		public static bool operator >=(UTCDate a, UTCDate b)
		{
			return a.ExtractUTC() >= b.ExtractUTC();
		}

		// ##########################################################
		// ### Properties
		// ##########################################################
		
		public double Timezone
		{ get { return tz; } }

		public DaylightTime DST
		{ get { return dst; } }
		
		public double GetDST()
		{
			return HasDST ? DST.Delta.TotalHours : 0;
		}
		
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

		// ##########################################################
		// ### Printers
		// ##########################################################
		
		public string Print()
		{
			DateTime dt_utc = ExtractUTC();
			DateTime dt_local = ExtractLocal();
			string fmt = "HH':'mm':'ss' 'dd'.'MM'.'yyyy";
			return string.Format("{0}  [{1} UTC]",
								 dt_local.ToString(fmt), dt_utc.ToString(fmt));
		}

		public string PrintDate()
		{
			DateTime dt_local = ExtractLocal();
			string fmt = "dd'.'MM'.'yyyy";
			return dt_local.ToString(fmt);
		}
		
		public string PrintTime()
		{
			DateTime dt_local = ExtractLocal();
			string fmt = "HH':'mm':'ss";
			return dt_local.ToString(fmt);
		}

		public string PrintTimezone()
		{
			char sign = '+';
			if (tz < 0) {
				sign = '-';
			}
			double val = Math.Abs(tz);
			return string.Format("{0}{1}", sign, val);
		}
		
		public static string PrintTzOffset(double hours)
		{
			int hours_i = (int) Math.Floor(Math.Abs(hours));
			int mins_i = (int) Math.Floor(Math.Abs(hours)*60 - hours_i*60);
			string mins_s = string.Format(":{0:00}", mins_i);
			mins_s = mins_i > 0 ? mins_s : string.Empty;
			string sign = hours >= 0 ? "+" : "-";
			return string.Format("UTC{0}{1}{2}", sign, hours_i, mins_s);
		}
	}
}
