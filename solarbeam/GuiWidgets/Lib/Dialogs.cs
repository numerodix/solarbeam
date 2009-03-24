// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Windows.Forms;

namespace SolarbeamGui
{
	partial class Widgets
	{
		public static SaveFileDialog GetSaveFileDialog(string filename,
		                                               string filter_name,
		                                               string filter_pattern)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.InitialDirectory = ".";
			dlg.Filter = String.Format("{0} ({1})|{2}", filter_name,
			                           filter_pattern, filter_pattern);
			dlg.FileName = filename;
			return dlg;
		}
	}
}