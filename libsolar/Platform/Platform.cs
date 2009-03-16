// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Platform
{
	public static class Platform
	{
		public static string GetRuntime()
		{
			string platform = "Mono";
			Type t = Type.GetType("Mono.Runtime");
			if (t == null) {
				platform = ".NET";
			}
			return platform;
		}
		
		public static string GetPlatform()
		{
			return Environment.OSVersion.Platform.ToString();
		}
	}
}