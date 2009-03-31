// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using NUnit.Framework;

using LibSolar.Testing;

namespace LibSolar.SolarOrbit.Test
{
	[TestFixture]
	public class JulianDateTest
	{
		/**
		 * Test roundtrip conversion.
		 */
		[Test]
		public void TestDayToCentury()
		{
			for (int i = 0; i < 100; i++)
			{
				int day = Rand.GetInt(1, 2455853); // 2009 + 1000
				int new_day = (int) Math.Round( JulianDate.CalcJulianDay(
						JulianDate.CalcJulianCentury(day) ) );

				Assert.AreEqual(day, new_day);
			}
		}

		/**
		 * Test roundtrip conversion.
		 */
		[Test]
		public void TestCenturyToDay()
		{
			Random random = new Random();
			for (int i = 0; i < 100; i++)
			{
				int scale = 10000;
				double cent = (double) random.Next(-scale, scale+1) / (double) scale;
				double new_cent = JulianDate.CalcJulianCentury(
						JulianDate.CalcJulianDay(cent) );

				double diff = Math.Abs(cent - new_cent);
				Assert.LessOrEqual(diff, (double) 1 / (double) scale);
			}
		}
	}
}
