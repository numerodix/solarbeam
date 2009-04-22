// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	/**
	 * Reference: http://www.sorrowman.org/c-sharp-programmer/url-link-to-desktop.html
	 */
	class WindowsShortcut
	{
		private string filename;
		private string app_path;
		private string icon_path;
		
		public WindowsShortcut(string filename, string app_path, string icon_path)
		{
			this.filename = filename;
			this.app_path = app_path;
			this.icon_path = icon_path;
		}

		public void WriteFile(string path)
		{
			if ((path != null) && (path != string.Empty)) {
				string s = Codegen();
				StreamWriter writer = new StreamWriter(Path.Combine(path, filename));
				writer.WriteLine(s);
				writer.Close();
			}
		}
		
		public string Codegen()
		{
			string s = "[InternetShortcut]\n";
			s += string.Format("URL=file:///{0}\n", app_path);
			s += "IconIndex=0\n";
			s += string.Format("IconFile={0}\n", icon_path);
			return s;
		}
	}
}
