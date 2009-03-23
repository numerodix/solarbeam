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
		                                                 string s)
		{
			ToolStripButton btn = new ToolStripButton();
			btn.Text = s;
			Controller.RegisterControl(id, btn);	// register control
			return btn;
		}

		public static ToolStripLabel GetToolStripLabel(string s)
		{
			ToolStripLabel label = new ToolStripLabel();
			label.Text = s;
			return label;
		}

		public static ToolStripTextBox GetToolStripTextBox(Controller.Id id, 
		                                                   int sz)
		{
			ToolStripTextBox txt = new ToolStripTextBox();
			txt.Text = sz.ToString();
			Controller.RegisterControl(id, txt);	// register control
			return txt;
		}
	}
}
