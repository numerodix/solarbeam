// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Formatting
{
	/**
	 * String formatting functions.
	 */
	public partial class Formatter
	{
		public static string FormatDayLength(SolarTimes st, SolarPosition sp)
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

		public static string FormatMaybeTime(UTCDate? udt)
		{
			string s = "##:##";
			if (udt != null) {
				s = Formatter.FormatTime(udt.Value);
			}
			return s;
		}
	}
}
