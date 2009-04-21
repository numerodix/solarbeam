// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	class WindowsShortcut
	{
		private string app_path;
		private string icon_path;
		
		public WindowsShortcut(string app_path, string icon_path)
		{
			this.app_path = app_path;
			this.icon_path = icon_path;
		}
		
		public static WindowsShortcut FromFile(string path)
		{
			StreamReader reader = new StreamReader(path);
			string lines = reader.ReadToEnd();
			
			// unmangle moronic newlines
			lines = Regex.Replace("\r\n", "\n", lines);
			string[] ss = lines.Split(new char[] {'\n'});
			
			string app_path = null;
			string icon_path = null;
			
			foreach (string s in ss) {
				string z = Regex.Find("URL=file:///(.*)", lines);
				if (z != null)
					app_path = z;
				z = Regex.Find("IconFile=(.*)", lines);
				if (z != null)
					icon_path = z;
			}
			
			return new WindowsShortcut(app_path, icon_path);
		}
		
		public void WriteFile(string path)
		{
			string s = Codegen();
			StreamWriter writer = new StreamWriter(path);
			writer.WriteLine(s);
			writer.Close();
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
