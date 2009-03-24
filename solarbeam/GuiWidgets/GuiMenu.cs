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
			ToolStripItem help = GetHelp();
			
			this.Items.AddRange(new ToolStripItem[] {file, locs, help});
		}

		public static ToolStripItem GetFile()
		{

			ToolStripMenuItem file = Widgets.GetToolStripMenuColumn("&File");
			
			ToolStripMenuItem exit = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUEXIT_ACTION,
					"E&xit",
					"app-exit.png");
			exit.ShortcutKeys = ((Keys) ((Keys.Control | Keys.Q)));
			
			file.DropDownItems.AddRange(new ToolStripItem[] {
					exit});
			return file;
		}

		public static ToolStripItem GetLocations()
		{
			ToolStripMenuItem locs = Widgets.GetToolStripMenuColumn("&Locations");
			
			ToolStripMenuItem loc_new = Widgets.GetToolStripMenuItem(
					Controller.Id.MENULOCNEW_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONNEW_ACTION),
					"loc-new.png");
			loc_new.ShortcutKeys = ((Keys) ((Keys.Alt | Keys.A)));
			
			ToolStripMenuItem loc_save = Widgets.GetToolStripMenuItem(
					Controller.Id.MENULOCSAVE_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONSAVE_ACTION),
					"loc-save.png");
			loc_save.ShortcutKeys = ((Keys) ((Keys.Control | Keys.S)));
			
			ToolStripMenuItem loc_delete = Widgets.GetToolStripMenuItem(
					Controller.Id.MENULOCDELETE_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONDELETE_ACTION),
					"loc-delete.png");
			loc_delete.ShortcutKeys = ((Keys) ((Keys.Alt | Keys.D)));
			
			locs.DropDownItems.AddRange(new ToolStripItem[] {
					loc_new,
					loc_save,
					loc_delete});
			return locs;
		}
		
		public static ToolStripItem GetHelp()
		{
			ToolStripMenuItem help = Widgets.GetToolStripMenuColumn("&Help");
			
			ToolStripMenuItem about = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUABOUT_ACTION,
					"&About",
			        "app-about.png");
			help.DropDownItems.AddRange(new ToolStripItem[] {
					about});
			
			return help;
		}
	}
}
