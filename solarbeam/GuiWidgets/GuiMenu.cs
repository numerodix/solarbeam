// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
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
			ToolStripItem actions = GetActions();
			ToolStripItem help = GetHelp();
			
			this.Items.AddRange(new ToolStripItem[] {
				file,
				locs,
				actions,
				help});
		}

		public static ToolStripItem GetFile()
		{
			ToolStripMenuItem file = Widgets.GetToolStripMenuColumn("&File");
			
			ToolStripMenuItem save = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUSESSIONSAVE_ACTION,
					"&Save session...",
					"save.png");
			save.ShortcutKeys = (Keys) (Keys.Control | Keys.S);
			
			ToolStripMenuItem load = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUSESSIONLOAD_ACTION,
					"&Load session...",
					"open.png");
			load.ShortcutKeys = (Keys) (Keys.Control | Keys.O);
			
			ToolStripSeparator sep = Widgets.GetToolStripSeparator();
			
			ToolStripMenuItem exit = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUEXIT_ACTION,
					"E&xit",
					"app-exit.png");
			exit.ShortcutKeys = (Keys) (Keys.Control | Keys.Q);
			
			file.DropDownItems.AddRange(new ToolStripItem[] {
				save,
				load,
				sep,
				exit});
			return file;
		}

		public static ToolStripItem GetLocations()
		{
			ToolStripMenuItem locs = Widgets.GetToolStripMenuColumn("&Locations");
			
			ToolStripMenuItem loc_new = Widgets.GetToolStripMenuItem(
					Controller.Id.MENULOCNEW_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONNEW_ACTION),
					"new.png");
			loc_new.ShortcutKeys = (Keys) (Keys.Alt | Keys.N);
			
			ToolStripMenuItem loc_save = Widgets.GetToolStripMenuItem(
					Controller.Id.MENULOCSAVE_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONSAVE_ACTION),
					"save.png");
			loc_save.ShortcutKeys = (Keys) (Keys.Alt | Keys.S);
			
			ToolStripMenuItem loc_delete = Widgets.GetToolStripMenuItem(
					Controller.Id.MENULOCDELETE_ACTION,
					Tooltips.GetTip(Controller.Id.LOCATIONDELETE_ACTION),
					"delete.png");
			loc_delete.ShortcutKeys = (Keys) (Keys.Alt | Keys.D);
			
			locs.DropDownItems.AddRange(new ToolStripItem[] {
				loc_new,
				loc_save,
				loc_delete});
			return locs;
		}
		
		public static ToolStripItem GetActions()
		{
			ToolStripMenuItem actions = Widgets.GetToolStripMenuColumn("&Actions");
			
			ToolStripMenuItem action_settimenow = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUTIMENOW_ACTION,
					Tooltips.GetTip(Controller.Id.TIMENOW_ACTION),
					"time-now.png");
			
			ToolStripSeparator sep = Widgets.GetToolStripSeparator();
			
			ToolStripMenuItem action_resetform = Widgets.GetToolStripMenuItem(
					Controller.Id.MENURESETFORM_ACTION,
					Tooltips.GetTip(Controller.Id.RESETFORM_ACTION),
					"reset.png");
			
			ToolStripMenuItem action_render = Widgets.GetToolStripMenuItem(
					Controller.Id.MENURENDER_ACTION,
					Tooltips.GetTip(Controller.Id.RENDER_ACTION),
					"render.png");
			action_render.ShortcutKeys = (Keys) Keys.F5;
			
			ToolStripSeparator sep2 = Widgets.GetToolStripSeparator();
						
			ToolStripMenuItem action_saveimage = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUSAVEIMAGE_ACTION,
					Tooltips.GetTip(Controller.Id.IMAGESAVE_ACTION)+"...",
					"image-save.png");
			action_saveimage.ShortcutKeys = (Keys) (Keys.Alt | Keys.I);
			
			actions.DropDownItems.AddRange(new ToolStripItem[] {
				action_settimenow,
				sep,
				action_resetform,
				action_render,
				sep2,
				action_saveimage});
			return actions;
		}
		
		public static ToolStripItem GetHelp()
		{
			ToolStripMenuItem help = Widgets.GetToolStripMenuColumn("&Help");
						
			ToolStripMenuItem desc = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUDESC_ACTION,
					"&How does this work?",
			        "app-desc.png");
			
			ToolStripSeparator sep = Widgets.GetToolStripSeparator();

			ToolStripMenuItem shortcut = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUSHORTCUT_ACTION,
					"&Create shortcuts...",
			        "shortcut-install.png");
			
			ToolStripSeparator sep2 = Widgets.GetToolStripSeparator();
			
			ToolStripMenuItem about = Widgets.GetToolStripMenuItem(
					Controller.Id.MENUABOUT_ACTION,
					"&About...",
			        "app-about.png");
			
			help.DropDownItems.AddRange(new ToolStripItem[] {
				desc,
				sep,
				shortcut,
				sep2,
				about});
			
			return help;
		}
	}
}
