// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Formatting
{
	public partial class Formatter
	{
		public static string FormatTimeLong(UTCDate udt, bool secs)
		{
			string fmt = "HH':'mm':'ss";
			if (!secs) {
				fmt = "HH':'mm";
			}
			
			DateTime dst = udt.ExtractLocal();
			string dst_s = dst.ToString(fmt);
			
			DateTime std = UTCDate.ResolveDST(dst, udt.DST);
			string std_s = std.ToString(fmt);
			
			string local_s = dst_s;
			if (udt.HasDST) {
				local_s = string.Format("{0} DST  {1}", dst_s, std_s);
			}

			DateTime utc = UTCDate.ResolveTimezone(std, udt.Timezone);
			string utc_s = utc.ToString(fmt);
			
			string s = string.Format("{0} ST  [{1} UTC]", local_s, utc_s);
			
			return s;
		}
		
		public static string FormatMaybeTimeLong(UTCDate? udt, bool secs)
		{
			string s = "##:##";
			if (udt != null) {
				s = Formatter.FormatTimeLong(udt.Value, secs);
			}
			return s;
		}
	}
}
