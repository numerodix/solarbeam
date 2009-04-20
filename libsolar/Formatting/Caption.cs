// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Formatting
{
	public partial class Formatter
	{		
		public static string FormatCaptionPosition(Position pos)
		{
			string lat = pos.LatitudeDegree.Print();
			string lon = pos.LongitudeDegree.Print();
			string s = string.Format("coordinates: {0}  {1}", lat, lon);
			return s;
		}
		
		public static string FormatCaptionTimezone(double tz, double dst)
		{
			string dst_s = UTCDate.PrintPretty(dst);
			string dst_fmt = string.Format(" ST, {0} DST", dst_s);
			dst_fmt = tz != dst ? dst_fmt : string.Empty;
			string s = string.Format("timezone: {0}{1}",
			                         UTCDate.PrintPretty(tz),
			                         dst_fmt);
			return s;
		}
		
		public static string FormatCaptionDate(UTCDate udt)
		{
			return string.Format("date: {0}", udt.PrintDate());
		}
		
		public static string FormatCaptionTime(UTCDate udt, CaptionInfo ci)
		{
			string dst_s = string.Empty;
			if (udt.HasDST) {
				dst_s = udt.IsDST ? " DST" : " ST";
			}
			return string.Format("time: {0}{1}", udt.PrintTime(), dst_s);
		}
		
		public static string FormatTime(UTCDate? udt)
		{
			string tm_s = "-----";
			if (udt != null) {
				tm_s = FormatTime(udt.Value);
			}
			return tm_s;
		}
	}
}
