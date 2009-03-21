// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace SolarbeamGui
{
	class DateSource
	{
		public static string TipTitle
		{ get { return "Date"; } }

		public static string TipDay
		{ get { return String.Format(
					"Enter the day of the month ({0}-{1})",
					UTCDate.DAY_MINVALUE,
					UTCDate.DAY_MAXVALUE);
		} }

		public static string TipMonth
		{ get { return String.Format(
					"Enter the month of the year ({0}-{1})",
					UTCDate.MONTH_MINVALUE,
					UTCDate.MONTH_MAXVALUE);
		} }

		public static string TipYear
		{ get { return String.Format(
					"Enter the year ({0}-{1})",
					UTCDate.YEAR_MINVALUE,
					UTCDate.YEAR_MAXVALUE);
		} }
	}
}
