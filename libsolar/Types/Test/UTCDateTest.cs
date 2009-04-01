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
			
			// timezone is UTC+x -> x hours ahead of UTC -> subtract tz
			// timezone is UTC-x -> x hours behind UTC -> add tz
			dt = dt.AddHours(-tz);
			
			Assert.True(udt.ExtractUTC().CompareTo(dt) == 0);
			Assert.True(udt.ExtractUTC().Kind == DateTimeKind.Utc);
			Assert.True(udt.ExtractLocaltime().Kind == DateTimeKind.Local);
		}
		
		[Test]
		public void TestDST()
		{
			// European Summer Time 2009
			double tz = 1; // zone UTC+1 / CET
			int dst = 1;
			DateTime lower = new DateTime(2009, 3, 29, 2, 0, 0);
			DateTime upper = new DateTime(2009, 10, 25, 2, 0, 0);
			DaylightTime dayl = new DaylightTime(lower, upper,
			                                     new TimeSpan(dst, 0, 0));
			
			UTCDate udt_pre = new UTCDate(tz, dayl, 2009, 2, 21, 12, 0, 0);
			DateTime dt_pre = udt_pre.ExtractLocaltime();
			DateTime dt_pre2 = new DateTime(2009, 2, 21, 12, 0, 0,
			                                DateTimeKind.Local);
			
			UTCDate udt_in = new UTCDate(tz, dayl, 2009, 5, 21, 12, 0, 0);
			DateTime dt_in = udt_in.ExtractLocaltime();
			DateTime dt_in2 = new DateTime(2009, 5, 21, 12, 0, 0, 
			                               DateTimeKind.Local).AddHours(dst);
			
			UTCDate udt_post = new UTCDate(tz, dayl, 2009, 11, 21, 12, 0, 0);
			DateTime dt_post = udt_post.ExtractLocaltime();
			DateTime dt_post2 = new DateTime(2009, 11, 21, 12, 0, 0, 
			                                 DateTimeKind.Local);
			
			Assert.True(dt_pre.CompareTo(dt_pre2) == 0);
			Assert.True(dt_in.CompareTo(dt_in2) == 0);
			Assert.True(dt_post.CompareTo(dt_post2) == 0);
		}
	}
}
