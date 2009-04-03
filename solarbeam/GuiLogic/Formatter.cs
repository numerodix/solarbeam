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
		private static string FormatTime(UTCDate udt)
		{
			DateTime dt_loc = udt.ExtractLocal();
			DateTime dt_std = udt.ExtractStandard();
			DateTime dt_utc = udt.ExtractUTC();
			string s_loc = string.Format("{0:HH:mm}", dt_loc);
			string s_std = string.Format("{0:HH:mm}", dt_std);
			string s_utc = string.Format("{0:HH:mm}", dt_utc);
			string s = string.Format("{0} -- {1} ST -- {2} UTC", s_loc, s_std, s_utc);
			return s;
		}
	
		private static string FormatDayLength(SolarTimes st, SolarPosition sp)
		{
			DateTime? begin = null, end = null;
			if (st.Sunrise != null) {
				begin = st.Sunrise.Value.ExtractLocal();
			}
			if (st.Sunset != null) {
				end = st.Sunset.Value.ExtractLocal();
			}
			
			double diff = 0;
			if ((begin != null) && (end != null)) {
				diff = (end.Value - begin.Value).TotalSeconds;
			}
		
			double fullday = 24*3600;
			int h = (int) (diff / 3600.0);
			int m = (int) ((diff - h * 3600.0) / 60.0);
			// can be all day or all night
			if ((diff >= fullday) && (sp.Elevation < 0)) {
				h = 0;
			} else if ((diff == 0) && (sp.Elevation >= 0)) {
				h = 24;
			}
			return string.Format("{0}h {1}m", h, m);
		}
	
		private static string FormatAngle(double ang)
		{
			return string.Format("{0:0.00}°", ang);
		}
	}
}
