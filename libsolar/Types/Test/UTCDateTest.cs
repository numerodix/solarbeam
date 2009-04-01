// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Globalization;

using NUnit.Framework;

using LibSolar.Testing;
using LibSolar.Types;

namespace LibSolar.Types.Test
{
	[TestFixture]
	public class UTCDateTest
	{
		[Test]
		[Repeat(1000)]
		public void TestTimezoneOffset()
		{
			int year = Rand.GetInt(2, 3000);
			int mon = Rand.GetInt(1, 12);
			int day = Rand.GetInt(1, DateTime.DaysInMonth(year, mon));
			int hour = Rand.GetInt(0, 23);
			int min = Rand.GetInt(0, 59);
			int sec = Rand.GetInt(0, 59);
			
			double tz = Rand.GetDouble(3,
			                           UTCDate.TIMEZONE_MINVALUE,
			                           UTCDate.TIMEZONE_MAXVALUE);
			
			UTCDate udt = new UTCDate(tz, year, mon, day, hour, min, sec);
			DateTime dt = new DateTime(year, mon, day, hour, min, sec);
			
			dt = UTCDate.ResolveTimezone(dt, tz);
			
			Assert.True(udt.ExtractUTC().CompareTo(dt) == 0);
			Assert.True(udt.ExtractUTC().Kind == DateTimeKind.Utc);
			Assert.True(udt.ExtractLocaltime().Kind == DateTimeKind.Local);
		}
		
		[Test]
		public void TestDST()
		{
			// European Summer Time 2009
			double tz = 1; // zone UTC+1 / CET
			TimeSpan dst = new TimeSpan(1, 0, 0);
			DateTime lower = new DateTime(2009, 3, 29, 2, 0, 0);
			DateTime upper = new DateTime(2009, 10, 25, 2, 0, 0);
			DaylightTime dayl = new DaylightTime(lower, upper, dst);
			
			TestDate((int) tz, dayl);
		}
		
		private void TestDate(int tz, DaylightTime dst)
		{
			DateTime lower = dst.Start;
			DateTime upper = dst.End;
			TimeSpan dst_span = dst.Delta;
			
			int day = 15;
			int hour = 12;
			int min = 0;
			int sec = 0;
			
			for (int i=UTCDate.MONTH_MINVALUE; i<=UTCDate.MONTH_MAXVALUE; i++) {
				// compute dates using UTCDate
				UTCDate udt = new UTCDate(tz, dst, lower.Year, i, day, hour, min, sec);
				DateTime dt_utc = udt.ExtractUTC();
				DateTime dt_loc = udt.ExtractLocaltime();
				
				// compute dates manually
				DateTime dt_utc2 = new DateTime(lower.Year, i, day, hour, min, sec,
				                                DateTimeKind.Utc);
				dt_utc2 = dt_utc2.AddHours(-tz); // resolve tz offset
				if (udt.IsDST) dt_utc2 = dt_utc2.Add(-dst_span); // resolve dst
				
				DateTime dt_loc2 = new DateTime(lower.Year, i, day, hour, min, sec,
				                                DateTimeKind.Local);
				
				Assert.True(dt_utc.CompareTo(dt_utc2) == 0);
				Assert.True(dt_loc.CompareTo(dt_loc2) == 0);
			}
		}
	}
}
