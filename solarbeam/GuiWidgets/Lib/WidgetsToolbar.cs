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
		public static ToolStripButton GetToolStripButton(Controller.Id id, 
		                                                 string s, string tip)
		{
			ToolStripButton btn = new ToolStripButton();
			btn.Text = s;
			btn.ToolTipText = tip;
			Controller.RegisterControl(id, btn);	// register control
			return btn;
		}
	}
}