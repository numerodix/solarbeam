// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
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
			string fmt_long = "HH':'mm':'ss' 'dd'.'MM'.'yyyy";
			if (!secs) {
				fmt = "HH':'mm";
			}
			
			DateTime dst = udt.ExtractLocal();
			string dst_s = dst.ToString(fmt);
			
			DateTime std = UTCDate.ResolveDST(dst, udt.DST);
			string std_s = std.ToString(fmt_long);
			
			string local_s = string.Format("{0} ST", dst_s);
			if (udt.HasDST) {
				local_s = string.Format("{0} DST  [{1} ST]", dst_s, std_s);
			}

			DateTime utc = UTCDate.ResolveTimezone(std, udt.Timezone);
			string utc_s = utc.ToString(fmt_long);
			
			string s = string.Format("{0}  [{1} UTC]", local_s, utc_s);
			
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
