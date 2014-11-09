// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
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
			label.Text = s;
			label.Dock = DockStyle.Fill;
			Controller.RegisterControl(id, label);	// register control
			return label;
		}
	}
}
