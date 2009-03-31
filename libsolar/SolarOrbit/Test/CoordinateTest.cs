// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using NUnit.Framework;

namespace LibSolar.SolarOrbit.Test
{
	[TestFixture]
	public class CoordinateTest
	{
		/**
		 * Test roundtrip conversion.
		 */
		[Test]
		public void TestDegToRad()
		{
			Random random = new Random();
			for (int i = 0; i < 100; i++)
			{
				int deg = random.Next(-360, 361);
				int new_deg = (int) 
					Math.Round( Coordinate.RadToDeg( Coordinate.DegToRad(deg) ) );

				Assert.AreEqual(deg, new_deg);
			}
		}

		/**
		 * Test roundtrip conversion.
		 */
		[Test]
		public void TestRadToDeg()
		{
			Random random = new Random();
			for (int i = 0; i < 100; i++)
			{
				// number of digits to check for : 9
				int scale = 1000000000;
				double rad = (random.Next(-scale, scale+1) * Math.PI) / scale;

				double new_rad =
					Coordinate.DegToRad( Coordinate.RadToDeg(rad) );

//				Console.WriteLine("\nrad : " + rad);
//				Console.WriteLine("new_rad : " + new_rad);

				double diff = Math.Abs(rad - new_rad);
				Assert.LessOrEqual(diff, (double) 1 / (double) scale);
			}
		}
	}
}
