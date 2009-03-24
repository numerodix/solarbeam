// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	partial class Widgets
	{
		public static ToolStripMenuItem GetToolStripMenuColumn(string s)
		{
			ToolStripMenuItem item = new ToolStripMenuItem();
			item.Name = s;
            //item.Size = new System.Drawing.Size(152, 22);
            item.Text = s;
			return item;
		}

		public static ToolStripMenuItem GetToolStripMenuItem(Controller.Id id,
		                                                     string s, string img)
		{
			ToolStripMenuItem item = new ToolStripMenuItem();
			item.Name = s;
            //item.Size = new System.Drawing.Size(152, 22);
            item.Text = s;
			item.Image = new Bitmap(Controller.AsmInfo.GetResource(img));
			Controller.RegisterControl(id, item);	// register control
			return item;
		}
	}
}
