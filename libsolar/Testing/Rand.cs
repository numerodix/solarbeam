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
			
			int upper_adj = upper == Int32.MaxValue ? Int32.MaxValue : upper+1;
			int v = random.Next(lower, upper_adj);
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
			
			int scale = (int) Math.Pow(10, digits);
			
			int r = GetInt(-scale, scale);
			
			double v = r * half;
			v += midpoint * scale;
			v /= scale;
			
			return v;
		}
		
		public static DateTime GetDateTime(DateTime lower, DateTime upper)
		{
			if (lower > upper) {
				throw new ArgumentException(string.Format(
					"Lower bound must be less than upper: {0}, {1}", lower, upper));
			}
			
			double range = (upper - lower).TotalDays;
			double delta = GetDouble(9, 0, range);
			DateTime v = lower.AddDays(delta);
			
			return v;
		}
	}
}