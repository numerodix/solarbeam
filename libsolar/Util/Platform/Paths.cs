// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	static class Paths
	{
		public static string GetDesktopPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		}
		
		public static string GetWindowsStartMenuPath()
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
		
		public static string UnixLocalXDGApplications()
		{
			string fallback = null; // return if no paths exist

			string path_s = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string[] paths = path_s.Split(new char[] {':'});
			for (int i=0; i<paths.Length; i++) {
				string p = Path.Combine(paths[i], Constants.UnixXDGApplicationsDirName);
				if (i == 0) fallback = p;
				if (Directory.Exists(p)) return p;
			}
			return fallback;
		}
		
		public static string UnixGlobalXDGApplications()
		{
			string fallback = null; // return if no paths exist
			
			string[] paths = Constants.UnixGlobalXDGBasePaths;
			for (int i=0; i<paths.Length; i++) {
				string p = Path.Combine(paths[i], Constants.UnixXDGApplicationsDirName);
				if (i == 0) fallback = p;
				if (Directory.Exists(p)) return p;
			}
			return fallback;
		}
	}
}
