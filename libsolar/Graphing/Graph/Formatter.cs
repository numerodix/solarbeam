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
		
	partial class Diagram
	{
		private static string FormatDate(UTCDate dt)
		{
			DateTime date = dt.ExtractLocaltime();
			Month month = (Month) Enum.ToObject(typeof(Month), date.Month-1);
			string month_s = month.ToString().Substring(0, 3);
			return string.Format("{0} {1}", month_s, date.Day);
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