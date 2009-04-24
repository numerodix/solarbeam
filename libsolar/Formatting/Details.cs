// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Formatting
{
	public partial class Formatter
	{
		public static string FormatDateTimeLong(UTCDate udt, bool secs)
		{
			string fmt = "HH':'mm':'ss' 'dd'.'MM'.'yyyy";
			
			DateTime local = udt.ExtractLocal();
			string local_s = local.ToString(fmt);
			
			DateTime std = UTCDate.ResolveDST(local, udt.DST);
			string std_s = std.ToString(fmt);

			DateTime utc = UTCDate.ResolveTimezone(std, udt.Timezone);
			string utc_s = utc.ToString(fmt);
			
			string s = string.Format("{0} DST  {1} ST  {2} UTC",
			                         local_s,
			                         std_s,
			                         utc_s);
			
			return s;
		}
	}
}
