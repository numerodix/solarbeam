// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Util
{
	public enum RuntimeName {
		Mono,
		NET,
	}
	
	public enum PlatformName {
		Windows,
		Unix,
	}
		
	/**
	 * Provide information about the execution platform.
	 */
	public static class Platform
	{
		public static RuntimeName GetRuntime()
		{
			RuntimeName platform = RuntimeName.Mono;
			Type t = Type.GetType("Mono.Runtime");
			if (t == null) {
				platform = RuntimeName.NET;
			}
			return platform;
		}
		
		public static string ToString(RuntimeName pn)
		{
			if (pn == RuntimeName.NET)
				return ".NET";
			return pn.ToString();
		}
		
		public static PlatformName GetPlatform()
		{
			int p = (int) Environment.OSVersion.Platform;
			if ((p == 4) || (p == 6) || (p == 128)) {
				return PlatformName.Unix;
			} else {
				return PlatformName.Windows;
			}
		}
		
		public static string GetDesktopPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}
	}
}
