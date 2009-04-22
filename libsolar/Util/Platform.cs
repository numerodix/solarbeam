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
		LinuxLocalXDGApplications,
		LinuxGlobalXDGApplications,
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
				return GetDesktopPath();
				break;
			case PathType.WindowsStartMenu:
				return GetWindowsStartMenuPath();
				break;
			case PathType.LinuxLocalXDGApplications:
				return LinuxLocalXDGApplications();
				break;
			case PathType.LinuxGlobalXDGApplications:
				return LinuxGlobalXDGApplications();
				break;
			}
			return null;
		}
		
		private static string GetDesktopPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}
		
		private static string GetWindowsStartMenuPath()
		{
			try {
				string path = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
				string[] dirs = Directory.GetDirectories(path);
				if (dirs.Length > 0) {
					path = dirs[0];
				}
				return path;
			} catch {
				return null;
			}
		}
		
		private static string LinuxLocalXDGApplications()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			return Path.Combine(path, Constants.LinXDGApplicationsDirName);
		}
		
		private static string LinuxGlobalXDGApplications()
		{
			foreach (string path in Constants.LinGlobalXDGBasePaths) {
				string p = Path.Combine(path, Constants.LinXDGApplicationsDirName);
				if (Directory.Exists(p)) return p;
			}
			return null;
		}
	}
}
