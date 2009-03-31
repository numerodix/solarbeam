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
			Random random = new Random();
			// number of digits to check for : 9
			int scale = 1000000000;
			double rad = (random.Next(-scale, scale+1) * Math.PI) / scale;

			double new_rad =
				Coordinate.DegToRad( Coordinate.RadToDeg(rad) );

//			Console.WriteLine("\nrad : " + rad);
//			Console.WriteLine("new_rad : " + new_rad);

			double diff = Math.Abs(rad - new_rad);
			Assert.LessOrEqual(diff, (double) 1 / (double) scale);
		}
	}
}
