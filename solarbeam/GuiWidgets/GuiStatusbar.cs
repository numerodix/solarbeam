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
			ToolStripStatusLabel icon =
				Widgets.GetToolStripStatusLabel(Controller.Id.STATUSBAR_ICON,
				                                string.Empty);
			icon.ImageAlign = ContentAlignment.BottomCenter;
			
			ToolStripStatusLabel label =
				Widgets.GetToolStripStatusLabel(Controller.Id.STATUSBAR_LABEL,
				                                "Status");
			
			this.Items.Add(icon);
			this.Items.Add(label);
		}
	}
}
