// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Diagnostics;

namespace LibSolar.Util
{
	public static class Processes
	{
		public static string Run(string bin, string args)
		{
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = bin;
			p.StartInfo.Arguments = args;
			p.Start();
			string output = p.StandardOutput.ReadToEnd().Trim();
			p.WaitForExit();
			return output;
		}
	}
}
