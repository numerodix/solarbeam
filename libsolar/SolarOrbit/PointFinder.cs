// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.SolarOrbit
{
	public class PointFinder
	{
		public static SolarTimes FindDawnDusk(Position pos, UTCDate udt)
		{
			SolarTimes st = Orbit.CalcSolarTimes(pos, udt);
			UTCDate? udt_dawn = FindPoint(pos, st.Noon, false, Orbit.CIVIL_TWIGHLIGHT);
			UTCDate? udt_dusk = FindPoint(pos, st.Noon, true, Orbit.CIVIL_TWIGHLIGHT);
			return new SolarTimes(pos, udt, 0, 0, 0, udt_dawn, st.Noon, udt_dusk);
		}
		
		private static UTCDate? FindPoint(Position pos, UTCDate udt_noon,
		                                  bool updown, double elevation)
		{
			SolarTimes st = Orbit.CalcSolarTimes(pos, udt_noon);
			
			// exit on missing times
			if (st.Sunrise == null) return null;
			if (updown) {
				if (st.Sunset == null) return null;
			}
			
			// find before noon
			UTCDate udt_low = udt_noon.AtStartOfUTCDay();
			UTCDate udt_high = st.Sunrise.Value;
			
			// find after noon
			if (updown) {
				udt_low = st.Sunset.Value;
				udt_high = udt_noon.AddDays(1).AtStartOfUTCDay();
			}
			
			SolarPosition sp = Orbit.CalcSolarPosition(pos, udt_low);
			double delta = elevation - sp.Elevation;
			double delta_p = delta > 0 ? delta + 1 : delta - 1;
			
			UTCDate udt = udt_low;
			while (Math.Abs(delta) < Math.Abs(delta_p)) {
				udt = udt.AddSeconds(30);
				sp = Orbit.CalcSolarPosition(pos, udt);
				
				delta_p = delta;
				delta = elevation - sp.Elevation;
			}
			
			return new Nullable<UTCDate>(udt);
		}
	}
}