// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
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
		[Repeat(100)]
		public void TestDayToCentury()
		{
			int day = Rand.GetInt(1, 2455853); // 2009 + 1000
			int new_day = (int) Math.Round( JulianDate.CalcJulianDay(
						JulianDate.CalcJulianCentury(day) ) );
			Assert.AreEqual(day, new_day);
		}

		/**
		 * Test roundtrip conversion.
		 */
		[Test]
		[Repeat(100)]
		public void TestCenturyToDay()
		{
			uint digits = 9;
			double cent = Rand.GetDouble(digits, -1, 1);
			double new_cent = JulianDate.CalcJulianCentury(
					JulianDate.CalcJulianDay(cent) );
			Asserter.Equal(digits, cent, new_cent);
		}
	}
}
