// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Format values for display in gui.
	 */
	partial class Controller
	{
		private static string FormatTime(UTCDate dt)
		{
			DateTime d = dt.ExtractLocaltime();
			return string.Format("{0:HH:mm}", d);
		}
	
		private static string FormatDayLength(SolarTimes st, SolarPosition sp)
		{
			double fullday = 24*3600;
			double begin = 0, end = fullday;
			if (st.Sunrise != null) {
				DateTime dt = st.Sunrise.Value.ExtractLocaltime();
				begin = dt.Hour * 3600 + dt.Minute * 60 + dt.Second;
			}
			if (st.Sunset != null) {
				DateTime dt = st.Sunset.Value.ExtractLocaltime();
				end = dt.Hour * 3600 + dt.Minute * 60 + dt.Second;
			}
			double diff = end - begin;
			int h = (int) (diff / 3600.0);
			int m = (int) ((diff - h * 3600.0) / 60.0);
			// can be all day or all night
			if ((diff == fullday) && (sp.Elevation < 0)) {
				h = 0;
			}
			return string.Format("{0}h {1}m", h, m);
		}
	
		private static string FormatAngle(double ang)
		{
			return string.Format("{0:0.00}°", ang);
		}
	}
}