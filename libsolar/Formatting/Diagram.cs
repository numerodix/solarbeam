// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Formatting
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
	
	public partial class Formatter
	{		
		public static string FormatDiagramDate(UTCDate dt)
		{
			DateTime date = dt.ExtractLocal();
			Month month = (Month) Enum.ToObject(typeof(Month), date.Month-1);
			string month_s = month.ToString().Substring(0, 3);
			return string.Format("{0} {1}", month_s, date.Day);
		}	
	}
}
