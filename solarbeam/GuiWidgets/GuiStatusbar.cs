// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiStatusbar : StatusStrip
	{
		public GuiStatusbar()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			ToolStripStatusLabel label =
				Widgets.GetToolStripStatusLabel(Controller.Id.STATUSBAR,
				                                "Status");
			label.ImageAlign = ContentAlignment.BottomLeft;
			
			this.Items.Add(label);
		}
	}
}
