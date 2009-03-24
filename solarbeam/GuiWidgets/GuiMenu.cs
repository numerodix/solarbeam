// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiMenu : MenuStrip
	{
		public GuiMenu()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			ToolStripItem file = GetFile();
			ToolStripItem locs = GetLocations();
			
			this.Items.AddRange(new ToolStripItem[] {file, locs});
		}

		public static ToolStripItem GetFile()
		{

			ToolStripMenuItem file = Widgets.GetToolStripMenuColumn("&File");
			ToolStripMenuItem exit = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUEXIT_ACTION,
					"Exit",
					"app-exit.png");
			file.DropDownItems.AddRange(new ToolStripItem[] {
					exit});
			return file;
		}

		public static ToolStripItem GetLocations()
		{
			ToolStripMenuItem locs = Widgets.GetToolStripMenuColumn("&Locations");
			ToolStripMenuItem loc_new = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUNEWLOC_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONNEW_ACTION),
					"loc-new.png");
			ToolStripMenuItem loc_save = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUSAVELOC_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONSAVE_ACTION),
					"loc-save.png");
			ToolStripMenuItem loc_delete = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUDELETELOC_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONDELETE_ACTION),
					"loc-delete.png");
			locs.DropDownItems.AddRange(new ToolStripItem[] {
					loc_new,
					loc_save,
					loc_delete});
			return locs;
		}
	}
}
