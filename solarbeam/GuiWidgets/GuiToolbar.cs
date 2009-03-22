// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiToolbar : ToolStrip
	{
		public GuiToolbar()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			ToolStripItem btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONNEW_ACTION,
			                                               "B");
			
			this.Items.AddRange(new ToolStripItem[] {btn});
		}
	}
}