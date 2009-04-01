// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
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
		public void TestGetDoubleBounds()
		{
			double lower = -Math.PI;
			double upper = Math.E;
			double v = Rand.GetDouble(9, lower, upper);
			Assert.GreaterOrEqual(upper, v);
			Assert.LessOrEqual(lower, v);
		}
	}
}
