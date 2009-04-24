// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Formatting
{
	public partial class Formatter
	{
		public static string FormatPosition(Position pos)
		{
			string lat = pos.LatitudeDegree.Print();
			if (lat.StartsWith(" ")) lat = lat.Remove(0, 1); // leading space
			string lon = pos.LongitudeDegree.Print();
			string s = string.Format("{0}  {1}", lat, lon);
			return s;
		}
		
		public static string FormatTimezone(double tz, double dst)
		{
			string dst_s = UTCDate.PrintTzOffset(dst);
			string dst_fmt = string.Format(" ST, {0} DST", dst_s);
			dst_fmt = tz != dst ? dst_fmt : string.Empty;
			string s = string.Format("{0}{1}",
			                         UTCDate.PrintTzOffset(tz),
			                         dst_fmt);
			return s;
		}
		
		public static string FormatTime(UTCDate udt)
		{
			DateTime dt_local = udt.ExtractLocal();
			string fmt = "HH':'mm";
			return dt_local.ToString(fmt);
		}
		
		public static string FormatHour(int hour)
		{
			return hour == 0 ? "24" : hour.ToString();
		}
		
		public static string FormatAngle(double ang)
		{
			return string.Format("{0:0.00}°", ang);
		}	
	}
}
