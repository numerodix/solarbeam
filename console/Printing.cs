// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace SolarbeamCli
{
	static class Printing
	{
		public static void Print(SolarPosition sp, SolarTimes sns)
		{
			string template = "{0,-12} : {1}\n";
	
			string s = String.Empty;
			s += sp.Print(template, true);
			s += "\n";
			s += sns.Print(template, false);
			Console.WriteLine(s);
		}
	}
}