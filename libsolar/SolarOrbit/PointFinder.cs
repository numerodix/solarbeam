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
			UTCDate? dawn_s, dusk_s;
			SolarTimes st = Orbit.CalcSolarTimes(pos, udt);
			
			dawn_s = st.Sunrise;
			dusk_s = st.Sunset;
			
			// set wide bounds
			UTCDate lower = st.Noon.AtStartOfUTCDay().AddDays(-1);
			UTCDate upper = lower.AddDays(3);
			
			dawn_s = FindPoint(pos, dawn_s, lower, Orbit.CIVIL_TWIGHLIGHT, -5.0);
			dusk_s = FindPoint(pos, dusk_s, upper, Orbit.CIVIL_TWIGHLIGHT, 5.0);

			return new SolarTimes(pos, udt, 0, 0, 0, dawn_s, st.Noon, dusk_s);
		}
		
		private static UTCDate? FindPoint(Position pos, 
		                                  UTCDate? nudt, UTCDate bound, 
		                                  double target, double inc)
		{
			if (nudt != null) {
				UTCDate udt_s = nudt.Value;
				
				UTCDate udt = udt_s;
				while (WithinBound(bound, udt, inc)) {
					udt = udt.AddSeconds(inc);
					
					if (Math.Abs(Compute(pos, udt) - target) < 0.01)
						return udt;
				}
			}
			return null;
		}
		
		private static bool WithinBound(UTCDate bound, UTCDate cur, double inc)
		{
			bool within = false;
			if (inc > 0) {
				within = cur <= bound;
			} else {
				within = bound <= cur;
			}
			return within;
		}
		
		private static double Compute(Position pos, UTCDate udt)
		{
			SolarPosition sp = Orbit.CalcSolarPosition(pos, udt);
			return sp.Elevation;
		}
	}
}
