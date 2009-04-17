// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using LibSolar.Types;

namespace LibSolar.SolarOrbit
{
	public class PointFinder
	{
		public static SolarTimes FindDawnDusk(Position pos, UTCDate udt)
		{
			SolarTimes st = Orbit.CalcSolarTimes(pos, udt);
			UTCDate lower = st.Noon.AtStartOfUTCDay();
			UTCDate upper = lower.AddDays(1);
			
			List<UTCDate> times = FindPoint(Orbit.CIVIL_TWIGHLIGHT, 0.01,
			                                pos, lower, upper);
			
			UTCDate? udt_dawn = null;
			UTCDate? udt_dusk = null;
			foreach (UTCDate i in times) {
				if (i < st.Noon)
					udt_dawn = i;
				if (i > st.Noon)
					udt_dusk = i;
			}
			return new SolarTimes(pos, udt, 0, 0, 0, udt_dawn, st.Noon, udt_dusk);
		}
		
		private static List<UTCDate> FindPoint(double target, double slack,
		                                       Position pos,
		                                       UTCDate lower, UTCDate upper)
		{
			double low = Compute(pos, lower);
			double high = Compute(pos, upper);
			
			// target is outside range, exit
			if ((target < low) || (target > high)) return new List<UTCDate>();
			
			// target is within slack, return
			if (Math.Abs(low - target) < slack) return list(lower);
			if (Math.Abs(high - target) < slack) return list(upper);
			
			// always keep low to lower bound for less-than comparisons
			if (low > high) {
				double t = high;
				UTCDate ut = upper;
				
				high = low;
				upper = lower;
				
				low = t;
				lower = ut;
			}
			
			// find middle
			UTCDate middle = GetMidpoint(lower, upper);
			double mid = Compute(pos, middle);
			
			// middle is on the curve between bounds
			if ((low <= mid) && (mid <= high)) {
				if (target < mid)
					return FindPoint(target, slack, pos, lower, middle);
				else
					return FindPoint(target, slack, pos, middle, upper);
				
			// middle is a local maximum/minimum
			} else {
				List<UTCDate> left = FindPoint(target, slack, pos, lower, middle);
				List<UTCDate> right = FindPoint(target, slack, pos, middle, upper);
				return join(left, right);
			}
			
			return null;
		}
		
		private static double Compute(Position pos, UTCDate udt)
		{
			SolarPosition sp = Orbit.CalcSolarPosition(pos, udt);
			return sp.Elevation;
		}
		
		private static UTCDate GetMidpoint(UTCDate lower, UTCDate upper)
		{
			double half = (upper - lower).TotalSeconds / 2;
			return lower.AddSeconds(half);
		}
		
		private static List<UTCDate> list(UTCDate udt)
		{
			List<UTCDate> lst = new List<UTCDate>();
			lst.Add(udt);
			return lst;
		}
				
		private static List<UTCDate> join(List<UTCDate> left, List<UTCDate> right)
		{
			foreach (UTCDate item in right) {
				left.Add(item);
			}
			return left;
		}
	}
}