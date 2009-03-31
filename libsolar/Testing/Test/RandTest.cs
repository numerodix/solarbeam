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
		public void TestGetBool()
		{
			List<bool> vs = new List<bool>();
			for (int i=0; i<1000; i++) {
				vs.Add(Rand.GetBool());
			}
			vs.Sort();
			Assert.AreNotEqual(vs[0], vs[vs.Count-1]);
		}
		
		[Test]
		public void TestGetInt()
		{
			// check upper bound
			int v = Rand.GetInt(1, 1);
			Assert.AreEqual(v, 1);
		}
	}
}
