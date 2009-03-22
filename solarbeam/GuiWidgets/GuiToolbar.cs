// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Drawing;
using System.Windows.Forms;

using LibSolar.Assemblies;

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
/*			ToolStripItem new_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONNEW_ACTION,
			                                                   "New", "Add new location");
			ToolStripItem save_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONSAVE_ACTION,
			                                                    "Save", "Save this location");
			ToolStripItem del_btn = Widgets.GetToolStripButton(Controller.Id.LOCATIONDELETE_ACTION,
			                                                   "Delete", "Delete this location");
			
			new_btn.Image = new Bitmap(Controller.AsmInfo.GetResource("render.png"));
			save_btn.Image = new Bitmap(Controller.AsmInfo.GetResource("loc-save.png"));
			del_btn.Image = new Bitmap(Controller.AsmInfo.GetResource("loc-delete.png"));
			this.Items.AddRange(new ToolStripItem[] {new_btn, save_btn, del_btn});
*/		}
	}
}