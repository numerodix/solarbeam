// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
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
			return string.Format("timezone: {0}", FormatTimezone(tz, dst));
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
