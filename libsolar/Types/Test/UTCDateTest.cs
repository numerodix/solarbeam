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
			
			double tz = Rand.GetDouble(3, UTCDate.TIMEZONE_MIN, UTCDate.TIMEZONE_MAX);
			
			UTCDate udt = new UTCDate(tz, year, mon, day, hour, min, sec);
			DateTime dt = new DateTime(year, mon, day, hour, min, sec);
			
			dt = UTCDate.ResolveTimezone(dt, tz);
			
			Assert.True(udt.ExtractUTC().CompareTo(dt) == 0);
			Assert.True(udt.ExtractUTC().Kind == DateTimeKind.Utc);
			Assert.True(udt.ExtractLocal().Kind == DateTimeKind.Local);
		}
		
		[Test]
		public void TestIsDST()
		{
			int tz = 1;
			DaylightTime dst = GetDSTCET();
			DateTime lower = dst.Start;
			DateTime upper = dst.End;

			UTCDate dst_pre = new UTCDate(tz, dst,
			                              lower.Year, lower.Month, lower.Day,
			                              lower.Hour, lower.Minute, lower.Second).AddDays(-14);
			UTCDate dst_in = new UTCDate(tz, dst,
			                             lower.Year, lower.Month, lower.Day,
			                             lower.Hour, lower.Minute, lower.Second).AddDays(14);
			UTCDate dst_post = new UTCDate(tz, dst,
			                               upper.Year, upper.Month, upper.Day,
			                               upper.Hour, upper.Minute, upper.Second).AddDays(14);
			
			Assert.IsTrue(dst_in.IsDST);
		}
		
		[Test]
		public void TestDST()
		{
			// European Summer Time 2009
			int tz = 1; // zone UTC+1 / CET
			DaylightTime dst = GetDSTCET();
			
			TestDate(tz, dst);
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
			
			for (int i=UTCDate.MONTH_MIN; i<=UTCDate.MONTH_MAX; i++) {
				// compute dates using UTCDate
				UTCDate udt = new UTCDate(tz, dst, lower.Year, i, day, hour, min, sec);
				DateTime dt_utc = udt.ExtractUTC();
				DateTime dt_loc = udt.ExtractLocal();
				
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
		
		private DaylightTime GetDSTCET()
		{
			// European Summer Time 2009
			TimeSpan dst = new TimeSpan(1, 0, 0);
			DateTime lower = new DateTime(2009, 3, 29, 2, 0, 0);
			DateTime upper = new DateTime(2009, 10, 25, 2, 0, 0);
			DaylightTime daytime = new DaylightTime(lower, upper, dst);
			return daytime;
		}
	}
}
