// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace SolarbeamGui
{
	class TimeSource
	{
		public static string TipTitle
		{ get { return "Time"; } }

		public static string TipHour
		{ get { return String.Format(
					"Enter the number of hours ({0}-{1})",
					UTCDate.HOUR_MINVALUE,
					UTCDate.HOUR_MAXVALUE);
		} }

		public static string TipMinute
		{ get { return String.Format(
					"Enter the number of minutes ({0}-{1})",
					UTCDate.MINUTE_MINVALUE,
					UTCDate.MINUTE_MAXVALUE);
		} }

		public static string TipSecond
		{ get { return String.Format(
					"Enter the number of seconds ({0}-{1})",
					UTCDate.SECOND_MINVALUE,
					UTCDate.SECOND_MAXVALUE);
		} }
	}
}
