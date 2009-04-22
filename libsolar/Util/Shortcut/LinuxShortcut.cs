// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace LibSolar.Util
{
	/**
	 * Reference: http://standards.freedesktop.org/desktop-entry-spec/latest
	 */
	class LinuxShortcut
	{
		private string filename;
		private string app_name;
		private string app_name_generic;
		private string app_comment;
		private string icon_path;
		private string app_path_file;
		private string app_exec;
		private string app_path;
		private string app_terminal;
		private string[] app_categories;
		
		public LinuxShortcut(string filename, string app_name,
		                     string app_name_generic, string app_comment,
		                     string icon_path, string app_path_file,
		                     string app_exec, string app_path,
		                     string app_terminal, string[] app_categories)
		{
			this.filename = filename;
			this.app_name = app_name;
			this.app_name_generic = app_name_generic;
			this.app_comment = app_comment;
			this.icon_path = icon_path;
			this.app_path_file = app_path_file;
			this.app_exec = app_exec;
			this.app_path = app_path;
			this.app_terminal = app_terminal;
			this.app_categories = app_categories;
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
			string s = "[Desktop Entry]\n";
			s += "Type=Application\n";
			s += string.Format("Name={0}\n", app_name);
			s += string.Format("GenericName={0}\n", app_name_generic);
			s += string.Format("Comment={0}\n", app_comment);
			s += string.Format("Icon={0}\n", icon_path);
			s += string.Format("TryExec={0}\n", app_path_file);
			s += string.Format("Exec={0}\n", app_exec);
			s += string.Format("Path={0}\n", app_path);
			s += string.Format("Terminal={0}\n", app_terminal);
			s += string.Format("Categories={0};\n", string.Join(";", app_categories));
			return s;
		}
	}
}
