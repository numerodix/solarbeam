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
			ToolStripItem new_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONNEW_ACTION,
			                                                   "New");
			ToolStripItem save_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONSAVE_ACTION,
			                                                    "Save");
			ToolStripItem del_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONDELETE_ACTION,
			                                                   "Delete");
			
			this.Items.AddRange(new ToolStripItem[] {new_btn, save_btn, del_btn});
		}
	}
}