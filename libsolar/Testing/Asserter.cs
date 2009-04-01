// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using NUnit.Framework;

namespace LibSolar.Testing
{
	public static class Asserter
	{
		public static void Equal(uint digits, double a, double b)
		{
			int scale = 1;
			for (int i=0; i<digits; i++) scale *= 10;
			double scale_inv = 1.0 / (double) scale;
			
			double diff = Math.Abs(a - b);
			Assert.LessOrEqual(diff, scale_inv);
		}
	}
}