// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	class WindowsShortcut
	{
		public const string filename = "SolarBeam.url";
		public string app_path;
		public string icon_path;
		
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
			
			string app_path = Regex.Find("URL=file:///(.*)", lines);
			string icon_path = Regex.Find("IconFile=(.*)", lines);
			
			return new WindowsShortcut(app_path, icon_path);
		}
		
		public void WriteFile(string path)
		{
			string s = Codegen();
			StreamWriter writer = new StreamWriter(Path.Combine(path, filename));
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
