// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Formatting
{
	public partial class Formatter
	{			
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
