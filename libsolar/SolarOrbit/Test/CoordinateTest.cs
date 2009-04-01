// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using NUnit.Framework;

using LibSolar.Testing;

namespace LibSolar.SolarOrbit.Test
{
	[TestFixture]
	public class CoordinateTest
	{
		/**
		 * Test roundtrip conversion.
		 */
		[Test]
		[Repeat(100)]
		public void TestDegToRad()
		{
			int deg = Rand.GetInt(-360, 360);
			int new_deg = (int) 
				Math.Round( Coordinate.RadToDeg( Coordinate.DegToRad(deg) ) );
			Assert.AreEqual(deg, new_deg);
		}

		/**
		 * Test roundtrip conversion.
		 */
		[Test]
		[Repeat(100)]
		public void TestRadToDeg()
		{
			uint digits = 9;
			double rad = Rand.GetDouble(digits, -Math.PI, Math.PI);
			double new_rad = Coordinate.DegToRad( Coordinate.RadToDeg(rad) );
			Asserter.Equal(digits, rad, new_rad);
		}
	}
}