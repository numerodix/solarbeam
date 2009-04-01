// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using NUnit.Framework;

using LibSolar.Types;

namespace LibSolar.SolarOrbit.Test
{
	[TestFixture]
	public class UTCDateTest
	{
		[Test]
		public void TestTimezoneOffset()
		{
			int year = 2009;
			int mon = 6;
			int day = 12;
			int hour = 11;
			int min = 45;
			int sec = 13;
			
			double tz = 3.5; // Tehran UTC+3:30
			
			UTCDate udt = new UTCDate(tz, year, mon, day, hour, min, sec);
			DateTime dt = new DateTime(year, mon, day, hour, min, sec);
			dt = dt.AddHours(-tz); // 11:45:31 in Tehran, 3.5h earlier UTC
			
			Assert.True(udt.ExtractUTC().CompareTo(dt) == 0);
			Assert.True(udt.ExtractUTC().Kind == DateTimeKind.Utc);
			Assert.True(udt.ExtractLocaltime().Kind == DateTimeKind.Local);
		}
	}
}
