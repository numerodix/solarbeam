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
			                                                   "New", "Add new location");
			ToolStripItem save_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONSAVE_ACTION,
			                                                    "Save", "Save this location");
			ToolStripItem del_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONDELETE_ACTION,
			                                                   "Delete", "Delete this location");
			
			this.Items.AddRange(new ToolStripItem[] {new_btn, save_btn, del_btn});
		}
	}
}