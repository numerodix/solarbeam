// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Util
{
	/**
	 * Provide information about the execution platform.
	 */
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
				
		public static string GetRuntimeVersion()
		{
			return Environment.Version.ToString();
		}
		
		public static string GetPlatform()
		{
			return Environment.OSVersion.Platform.ToString();
		}
		
		public static string GetPlatformVersion()
		{
			return Environment.OSVersion.Version.ToString();
		}
	}
}
