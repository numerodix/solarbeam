// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Graphing
{
	enum Month {
		January,
		February,
		March,
		April,
		May,
		June,
		July,
		August,
		September,
		October,
		November,
		December
	}
		
	/**
	 * Formatting related primitives.
	 */
	partial class Diagram
	{
		private static string FormatPosition(Position pos)
		{
			string lat = pos.LatitudeDegree.Print();
			string lon = pos.LongitudeDegree.Print();
			string s = string.Format("coordinates: {0}  {1}", lat, lon);
			return s;
		}
		
		private static string FormatTimezone(double tz, double dst)
		{
			string dst_s = UTCDate.PrintPretty(dst);
			string dst_fmt = string.Format(" standard, {0} daylight", dst_s);
			dst_fmt = tz != dst ? dst_fmt : string.Empty;
			string s = string.Format("timezone: {0}{1}",
			                         UTCDate.PrintPretty(tz),
			                         dst_fmt);
			return s;
		}
		
		private static string FormatCaptionDate(UTCDate udt)
		{
			return string.Format("date: {0}", udt.PrintDate());
		}
		
		private static string FormatCaptionTime(UTCDate udt, CaptionInfo ci)
		{
			string dst_s = string.Empty;
			if (udt.HasDST) {
				dst_s = udt.IsDST ? " daylight" : " standard";
			}
			return string.Format("time: {0}{1}", udt.PrintTime(), dst_s);
		}
		
		private static string FormatDiagramDate(UTCDate dt)
		{
			DateTime date = dt.ExtractLocal();
			Month month = (Month) Enum.ToObject(typeof(Month), date.Month-1);
			string month_s = month.ToString().Substring(0, 3);
			return string.Format("{0} {1}", month_s, date.Day);
		}
		
		private static string FormatHour(int hour)
		{
			return hour == 0 ? "24" : hour.ToString();
		}
		
		private int GetLineThickness()
		{
			return Math.Max(1, (int) ((double) grid.Diameter / 450.0));
		}
		
		private float GetLabelFontSize()
		{
			return (float) Math.Max(1.0, (double) graph.Delta * 8.0/20.0);
		}
	}
}
