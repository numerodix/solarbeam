// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using NUnit.Framework;

using LibSolar.Testing;
using LibSolar.Types;

namespace LibSolar.SolarOrbit.Test
{
	[TestFixture]
	public class PositionTest
	{
		/**
		 * Make sure position given as tuple of integer values is not
		 * distorted by internal conversions.
		 */
		[Test]
		public void TestPositionCoordinateConversion()
		{
			for (int i = 0; i < 1000; i++)
			{
				// upper 89 59 59
				int ladeg = Rand.GetInt(0, 89);
				int lamin = Rand.GetInt(0, 59);
				int lasec = Rand.GetInt(0, 59);
				// upper 179 59 59
				int lodeg = Rand.GetInt(0, 179);
				int lomin = Rand.GetInt(0, 59);
				int losec = Rand.GetInt(0, 59);

				PositionDirection ladir;
				if ( Rand.GetBool() ) {
					ladir = Position.LATITUDE_POS;
				} else {
					ladir = Position.LATITUDE_NEG;
				}

				PositionDirection lodir;
				if ( Rand.GetBool() ) {
					lodir = Position.LONGITUDE_POS;
				} else {
					lodir = Position.LONGITUDE_NEG;
				}

				Position pos = new Position(ladir, ladeg, lamin, lasec,
				                            lodir, lodeg, lomin, losec);

				Degree dla = pos.LatitudeDegree;
				Degree dlo = pos.LongitudeDegree;
/*
				Console.WriteLine("ladir : {0}", ladir);
				Console.WriteLine("ladeg : {0}", ladeg);
				Console.WriteLine("lamin : {0}", lamin);
				Console.WriteLine("lasec : {0}", lasec);
				Console.WriteLine("\nlodir : {0}", lodir);
				Console.WriteLine("lodeg : {0}", lodeg);
				Console.WriteLine("lomin : {0}", lomin);
				Console.WriteLine("losec : {0}", losec);

				Console.WriteLine("\nrandom latitude  : " + pos.PrintLatitude());
				Console.WriteLine("random longitude : " + pos.PrintLongitude());
*/
				Assert.AreEqual(dla.Direction, ladir);
				Assert.AreEqual(dla.Deg, Math.Abs(ladeg) );
				Assert.AreEqual(dla.Min, lamin);
				Assert.AreEqual(dla.Sec, lasec);

				Assert.AreEqual(dlo.Direction, lodir);
				Assert.AreEqual(dlo.Deg, Math.Abs(lodeg) );
				Assert.AreEqual(dlo.Min, lomin);
				Assert.AreEqual(dlo.Sec, losec);
			}
		}
	}
}
