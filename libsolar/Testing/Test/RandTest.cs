// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace LibSolar.Testing.Test
{
	[TestFixture]
	public class RandTest
	{
		/**
		 * Make sure bools are not all the same.
		 */
		[Test]
		public void TestGetBoolUniqueness()
		{
			List<bool> vs = new List<bool>();
			for (int i=0; i<1000; i++) {
				vs.Add(Rand.GetBool());
			}
			vs.Sort();
			Assert.AreNotEqual(vs[0], vs[vs.Count-1]);
		}
		
		/**
		 * Check upper bound.
		 */
		[Test]
		public void TestGetIntUpperBound()
		{
			int v = Rand.GetInt(1, 1);
			Assert.AreEqual(v, 1);
		}
		
		/**
		 * Check that double is within bounds.
		 */
		[Test]
		[Repeat(1000)]
		public void TestGetDoubleBoundsStatic()
		{
			double lower = -Math.PI;
			double upper = Math.E;
			double v = Rand.GetDouble(9, lower, upper);
			Assert.GreaterOrEqual(upper, v);
			Assert.LessOrEqual(lower, v);
		}

		[Test]
		[Repeat(1000)]
		public void TestGetDoubleBoundsDynamic()
		{
			int numerator = Rand.GetInt(Int32.MinValue, Int32.MaxValue);
			int denominator = Rand.GetInt(Int32.MinValue, Int32.MaxValue);
			denominator = denominator == 0 ? denominator+1 : denominator; // div by 0
			double lower = (double) numerator / (double) denominator;
			
			int width = Rand.GetInt(0, Int32.MaxValue);
			double upper = lower + width;
			
			double v = Rand.GetDouble(9, lower, upper);
			Assert.GreaterOrEqual(upper, v);
			Assert.LessOrEqual(lower, v);
		}
		
		[Test]
		[Repeat(1000)]
		public void TestGetDateTimeBoundsStatic()
		{
			DateTime lower = new DateTime(1312, 1, 1, 12, 0, 0);
			DateTime upper = new DateTime(1736, 1, 1, 12, 0, 0);
			
			DateTime v = Rand.GetDateTime(lower, upper);
			
			Assert.GreaterOrEqual(upper, v);
			Assert.LessOrEqual(lower, v);
		}
	}
}
