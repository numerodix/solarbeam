// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Text.RegularExpressions;

namespace LibSolar.Util
{
	public static class Regex
	{
		public static string Replace(string find, string replace, string s)
		{
			return System.Text.RegularExpressions.Regex.Replace(find, replace, s);
		}
		
		public static string Find(string find, string s)
		{
			Match m = System.Text.RegularExpressions.Regex.Match(s, find,
			                                                     RegexOptions.Multiline);
			if (m.Success) {
				GroupCollection gc = m.Groups;
				return gc[1].Value;
			}
			return null;
		}
	}
}
