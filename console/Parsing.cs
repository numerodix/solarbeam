// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;

namespace SolarbeamCli
{
	static class Parsing
	{
		public static ArrayList SplitParse(string str, char c)
		{
			char[] cs = {c};
			ArrayList items = new ArrayList();
			
			foreach (string s in str.Split(cs))
			{
				try
				{
					items.Add( (int) Decimal.Parse(s) );
				} catch (FormatException) {
					items.Add( s );
				}
			}
			return items;
		}
	}
}