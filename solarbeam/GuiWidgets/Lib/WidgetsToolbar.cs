// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide initialization of toolbar widgets.
	 */
	partial class Widgets
	{
		public static ToolStripButton GetToolStripButton(string s)
		{
			ToolStripButton btn = new ToolStripButton();
			btn.Text = s;
			return btn;
		}
	}
}