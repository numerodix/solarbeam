// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Util
{
	public enum RuntimeName {
		Mono,
		NET,
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
			if (pn == RuntimeName.NET) {
				return ".NET";
			}
			return pn.ToString();
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
		
		public static string GetDesktopPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}
	}
}
