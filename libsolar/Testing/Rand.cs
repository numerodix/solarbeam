// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Testing
{
	public static class Rand
	{
		private static Random random;
		
		static Rand()
		{
			random = new Random();
		}
		
		public static bool GetBool()
		{
			bool v = (GetInt(0, 1) == 0) ? true : false;
			return v;
		}
		
		/**
		 * Preempt unintuitive upper bound logic [a,b) in System.Random. Here
		 * both bounds are inclusive: [a,b]
		 */
		public static int GetInt(int lower, int upper)
		{
			if (lower > upper) {
				throw new ArgumentException(string.Format(
					"Lower bound must be less than upper: {0}, {1}", lower, upper));
			}
			
			int v = random.Next(lower, upper+1);
			return v;
		}
		
		/**
		 * Get a random double to a given precision.
		 * Algorithm:
		 *   input -> precision: 3, lower: -3, upper: 2
		 * half = 2.5
		 * mid  = -0.5
		 * scale : 1e(precision) = 1000
		 *
		 * random int range: -1000,1000
		 *   x half       = -2500,2500
		 *   + mid*scale  = -3000,2000
		 *   / scale      = -3.000,2.000
		 */
		public static double GetDouble(uint digits, double lower, double upper)
		{
			if (lower > upper) {
				throw new ArgumentException(string.Format(
					"Lower bound must be less than upper: {0}, {1}", lower, upper));
			}
			
			double half = Math.Abs((upper - lower) / 2.0);
			double midpoint = lower + half;
			
			int scale = 1;
			for (int i=0; i<digits; i++) scale *= 10;
			
			int r = GetInt(-scale, scale);
			
			double v = r * half;
			v += midpoint * scale;
			v /= scale;
			
			return v;
		}
	}
}