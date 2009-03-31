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
			bool v = true;
			if (GetInt(0, 1) == 1) v = false;
			return v;
		}
		
		/**
		 * Preempt unintuitive upper bound logic [a,b) in System.Random. Here
		 * both bounds are inclusive: [a,b]
		 */
		public static int GetInt(int lower, int upper)
		{
			return random.Next(lower, upper+1);
		}
		
		public static double GetDouble(double lower, double upper, uint digits)
		{
			return 0;
		}
	}
}
