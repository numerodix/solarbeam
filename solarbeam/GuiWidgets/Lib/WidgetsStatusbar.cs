// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	partial class Widgets
	{
		public static ToolStripStatusLabel GetToolStripStatusLabel(Controller.Id id,
		                                                           string s)
		{
			ToolStripStatusLabel label = new ToolStripStatusLabel();
			label.Dock = DockStyle.Fill;
            label.Text = s;
			Controller.RegisterControl(id, label);	// register control
			return label;
		}
	}
}
