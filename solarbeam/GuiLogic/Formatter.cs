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
			DateTime dt_utc = udt.ExtractUTC();
			string s_loc = string.Format("{0:HH:mm}", udt.ExtractLocal());
			string s_utc = string.Format("{0:HH:mm} {1:dd.mm.yyyy}", dt_utc, dt_utc);
			string s = string.Format("{0}   [{1} UTC]", s_loc, s_utc);
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
		
		private static string FormatFilename(string loc, Position pos, UTCDate dt)
		{
			string dt_s = String.Format("{0:0000}-{1:00}-{2:00}",
			                            dt.Year, dt.Month, dt.Day);
			string tm_s = String.Format("{0:00}-{1:00}-{2:00}",
			                            dt.Hour, dt.Minute, dt.Second);
			string loc_s = loc;
			if (loc == String.Empty) {
				string latdir_s = (pos.LatitudeDegree.Direction
						== PositionDirection.North) ? "N" : "S";
				string lat_s = String.Format("{0}{1:00}-{2:00}-{3:00}",
				                             latdir_s,
				                             pos.LatitudeDegree.Deg,
				                             pos.LatitudeDegree.Min,
				                             pos.LatitudeDegree.Sec);
				string londir_s = (pos.LongitudeDegree.Direction
						== PositionDirection.East) ? "E" : "W";
				string lon_s = String.Format("{0}{1:00}-{2:00}-{3:00}",
				                             londir_s,
				                             pos.LongitudeDegree.Deg,
				                             pos.LongitudeDegree.Min,
				                             pos.LongitudeDegree.Sec);
				loc_s = String.Format("{0} {1}", lat_s, lon_s);
			}
			string s = String.Format("{0} - {1} {2}", loc_s, dt_s, tm_s);
			return s;
		}
	}
}
