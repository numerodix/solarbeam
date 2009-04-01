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
		[Repeat(1000)]
		public void TestIsDST()
		{
			int tz = 0; DaylightTime dst = null;
			GetDSTCET(out dst, out tz);
			TestIsDST(tz, dst);
			
			tz = 0; dst = null;
			GetDSTAEDT(out dst, out tz);
			TestIsDST(tz, dst);
		}
		
		public void TestIsDST(int tz, DaylightTime dst)
		{
			DateTime lower = dst.Start;
			DateTime upper = dst.End;
			
			DateTime year_lower = new DateTime(lower.Year, 1, 1, 0, 0, 0);
			DateTime year_upper = new DateTime(upper.Year, 12, 31, 23, 59, 59);
			
			DateTime dt_pre = Rand.GetDateTime(year_lower, lower);
			DateTime dt_in = Rand.GetDateTime(lower, upper);
			DateTime dt_post = Rand.GetDateTime(upper, year_upper);

			UTCDate dst_pre = new UTCDate(tz, dst,
			                              dt_pre.Year, dt_pre.Month, dt_pre.Day,
			                              dt_pre.Hour, dt_pre.Minute, dt_pre.Second);
			UTCDate dst_in = new UTCDate(tz, dst,
			                             dt_in.Year, dt_in.Month, dt_in.Day,
			                             dt_in.Hour, dt_in.Minute, dt_in.Second);
			UTCDate dst_post = new UTCDate(tz, dst,
			                               dt_post.Year, dt_post.Month, dt_post.Day,
			                               dt_post.Hour, dt_post.Minute, dt_post.Second);
			
			Assert.IsFalse(dst_pre.IsDST);
			Assert.IsTrue(dst_in.IsDST);
			Assert.IsFalse(dst_post.IsDST);
		}
		
		[Test]
		public void TestDST()
		{
			int tz = 0; DaylightTime dst = null;
			GetDSTCET(out dst, out tz);
			TestDST(tz, dst);
			
			tz = 0; dst = null;
			GetDSTAEDT(out dst, out tz);
			TestDST(tz, dst);
		}
		
		private void TestDST(int tz, DaylightTime dst)
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
		
		private void GetDSTCET(out DaylightTime daytime, out int tz)
		{
			// European Summer Time 2009
			TimeSpan dst = new TimeSpan(1, 0, 0);
			DateTime lower = new DateTime(2009, 3, 29, 2, 0, 0);
			DateTime upper = new DateTime(2009, 10, 25, 2, 0, 0);
			daytime = new DaylightTime(lower, upper, dst);
			tz = 1;
		}

		private void GetDSTAEDT(out DaylightTime daytime, out int tz)
		{
			// Australian Eastern Daylight Time 2009
			TimeSpan dst = new TimeSpan(1, 0, 0);
			DateTime lower = new DateTime(2009, 4, 5, 2, 0, 0);
			DateTime upper = new DateTime(2009, 10, 4, 2, 0, 0);
			daytime = new DaylightTime(lower, upper, dst);
			tz = 10;
		}
	}
}
