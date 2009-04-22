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
		
		public void ShortcutTo(string path)
		{
			string app_path = asminfo.GetAppPath();
			WindowsShortcut ws = new WindowsShortcut(Constants.WinShortcutFile,
			                                         Path.Combine(app_path, Constants.GUI_FILENAME),
			                                         Path.Combine(app_path, Constants.WIN_ICON_FILENAME));
			ws.WriteFile(path);
		}
	}
}