// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	public class WindowsShortcutInstall
	{
		private AsmInfo asminfo;
		
		public WindowsShortcutInstall(AsmInfo asminfo)
		{
			this.asminfo = asminfo;
		}
		
		public string GetStartMenuPath()
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
		
		public void ShortcutTo(string path)
		{
			string app_path = asminfo.GetAppPath();
			WindowsShortcut ws = new WindowsShortcut(Path.Combine(app_path, Constants.GUI_FILENAME),
			                                         Path.Combine(app_path, Constants.ICON_FILENAME));
			ws.WriteFile(path);
		}
	}
}