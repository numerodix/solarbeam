// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Drawing;
using System.Windows.Forms;

using LibSolar.Util;

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
*/
/*			ToolStripItem bit_lbl = Widgets.GetToolStripLabel("Save bitmap:");
			ToolStripItem bit_in = Widgets.GetToolStripTextBox(Controller.Id.BITMAP_SIZE,
			                                                   500);
			bit_in.Size = new Size(40, bit_in.Height);
			ToolStripItem bit_lblu = Widgets.GetToolStripLabel("pixels");
			ToolStripItem bit_btn = Widgets.GetToolStripButton(Controller.Id.BITMAPSAVE_ACTION,
			                                                   "Save");
			
			this.Items.AddRange(new ToolStripItem[] {bit_lbl, bit_in, bit_lblu, bit_btn});
*/		}
	}
}