// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
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
		private string app_path;
		private string icon_path;
		
		public WindowsShortcut(string app_path, string icon_path)
		{
			this.app_path = app_path;
			this.icon_path = icon_path;
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
