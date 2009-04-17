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
				if (i.CompareTo(st.Noon) == -1)
					udt_dawn = i;
				if (i.CompareTo(st.Noon) == 1)
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
			
			if ((target < low) || (target > high)) return new List<UTCDate>();
			
			if (Math.Abs(low - target) < slack) return list(lower);
			if (Math.Abs(high - target) < slack) return list(upper);
			
			UTCDate middle = GetMidpoint(lower, upper);
			double mid = Compute(pos, middle);
			if (IsBetween(mid, low, high)) {
				if (low <= high) {
					if (target < mid)
						return FindPoint(target, slack, pos, lower, middle);
					else
						return FindPoint(target, slack, pos, middle, upper);
				} else {
					if (target > mid)
						return FindPoint(target, slack, pos, middle, lower);
					else
						return FindPoint(target, slack, pos, upper, middle);
				}
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
		
		private static bool IsBetween(double point, double lower, double upper)
		{
			bool between = false;
			if (upper.CompareTo(lower) >= 0) {
				between = ((lower.CompareTo(point) <= 0) && (upper.CompareTo(point) >= 0));
			} else {
				between = ((upper.CompareTo(point) <= 0) && (lower.CompareTo(point) >= 0));
			}
			return between;
		}
		
		private static UTCDate GetMidpoint(UTCDate lower, UTCDate upper)
		{
			double half = (upper.ExtractUTC() - lower.ExtractUTC()).TotalSeconds / 2;
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