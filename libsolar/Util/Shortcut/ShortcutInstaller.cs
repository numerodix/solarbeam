// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	public class ShortcutInstaller
	{
		private AsmInfo asminfo;
		
		public ShortcutInstaller(AsmInfo asminfo)
		{
			this.asminfo = asminfo;
		}
		
		public void WindowsShortcutTo(string path)
		{
			string app_path = asminfo.GetAppPath();
			string app_file_path = Path.Combine(app_path, Constants.GUI_FILENAME);
			string app_icon_path = Path.Combine(app_path, Constants.WIN_ICON_FILENAME);
			WindowsShortcut ws = new WindowsShortcut(app_file_path,
			                                         app_icon_path);
			
			string file_path = Path.Combine(path, Constants.WinShortcutFile);
			WriteFile(ws.Codegen(), file_path);
		}
		
		public void UnixShortcutTo(string path)
		{
			string app_path = asminfo.GetAppPath();
			string app_file_path = Path.Combine(app_path, Constants.GUI_FILENAME);
			string app_icon_path = Path.Combine(app_path, Constants.WIN_ICON_FILENAME);
			string app_exec = string.Format("mono \"{0}\"", app_file_path);
			UnixShortcut us = new UnixShortcut(Constants.GUI_APPTITLE,
			                                   Constants.UnixShortcutGenericName,
			                                   Constants.GUI_APPDESC,
			                                   app_icon_path,
			                                   app_file_path,
			                                   app_exec,
			                                   app_path,
			                                   Constants.UnixShortcutTerminal,
			                                   Constants.UnixShortcutCategories);
			
			string file_path = Path.Combine(path, Constants.UnixShortcutFile);
			WriteFile(us.Codegen(), file_path);
		}
		
		private void WriteFile(string s, string file_path)
		{
			if ((file_path != null) && (file_path != string.Empty)) {
				StreamWriter writer = new StreamWriter(file_path);
				writer.WriteLine(s);
				writer.Close();
			}
		}
		
	}
}