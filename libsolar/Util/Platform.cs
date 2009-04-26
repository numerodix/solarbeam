// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

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
	
	public enum PathType {
		WindowsStartMenu,
		Desktop,
		UnixLocalXDGApplications,
		UnixGlobalXDGApplications,
	}
		
	/**
	 * Provide information about the execution platform.
	 */
	public static class Platform
	{
		public static string GetRuntimePlatformString()
		{
			return string.Format("{0}: {1}, {2}: {3}",
			                     "{runtime",
			                     RuntimeDetect.GetRuntimeName(),
			                     "platform",
			                     PlatformDetect.GetPlatformName() + "}");
		}
		
		public static RuntimeName GetRuntime()
		{
			RuntimeName platform = RuntimeName.Mono;
			Type t = Type.GetType("Mono.Runtime");
			if (t == null) {
				platform = RuntimeName.NET;
			}
			return platform;
		}
		
		public static string GetRuntimeString()
		{
			RuntimeName pn = GetRuntime();
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
		
		public static string GetPath(PathType type)
		{
			switch (type) {
			case PathType.Desktop:
				return Paths.GetDesktopPath();
				break;
			case PathType.WindowsStartMenu:
				return Paths.GetWindowsStartMenuPath();
				break;
			case PathType.UnixLocalXDGApplications:
				return Paths.UnixLocalXDGApplications();
				break;
			case PathType.UnixGlobalXDGApplications:
				return Paths.UnixGlobalXDGApplications();
				break;
			}
			return null;
		}
	}
}
