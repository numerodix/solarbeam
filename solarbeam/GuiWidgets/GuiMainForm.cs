// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Represents the main gui form.
	 */
	sealed class GuiMainForm : GuiBaseForm
	{	
		private const int BORDER = 0;
		private const int DIAGRAM_DIM_X = GuiDiagram.IDEAL_DIM_X;
		private const int DIAGRAM_DIM_Y = GuiDiagram.IDEAL_DIM_Y;
		
		// make about form accessible to logic
		public static GuiShortcutInstall shortcutform;
		public static GuiAbout aboutform;
		
		// my widgets
		private GuiMenu menu;
		private GuiStatusbar statusbar;
		private GuiControlPanel controlpanel;
		
		public GuiMainForm(string form_title, string icon)
			: base(form_title, icon)
		{
			shortcutform = new GuiShortcutInstall("Create shortcuts", icon);
			Controller.ShortcutPlatformChange(null, null); // refresh shortcut gui
			aboutform = new GuiAbout("About " + form_title, icon);
		}
		
		protected override void InitializeComponent()
		{
			this.Closed += new EventHandler(OnQuit);
			
			// try to bring to front somehow
			this.Load += delegate (object o, EventArgs a) {
				this.Activate();
				this.BringToFront();
				this.Focus();
			};

			// init datasources before instantiating widgets
			Controller.InitSources();
			
			Controller.SplashQueue.Enqueue("Initializing gui");
			this.Controls.Add(GetPanel());

			// makes mono layout differently 1.9 <-> 2.0
			// VS default: 6F 13F (win ok)
			// mono 1.9: 6F 14F (win ok)
			// mono 2.0: 7F 14F
//			this.AutoScaleDimensions = new SizeF(6F, 13F);
			
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = GetFormSize();
			this.MinimumSize = new Size(ClientSize.Width + (Size.Width - ClientSize.Width),
			                            ClientSize.Height + (Size.Height - ClientSize.Height));		
		}
		
		private Control GetPanel()
		{
			this.menu = new GuiMenu();
			this.statusbar = new GuiStatusbar();
			Control mainarea = GetMainArea();
			
			this.MainMenuStrip = menu;
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(3, 1, 0, BORDER);
			
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, menu.Height));
			layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, statusbar.Height));
			
			layout.Controls.Add(menu, 0, 0);
			layout.Controls.Add(mainarea, 0, 1);
			layout.Controls.Add(statusbar, 0, 2);
			
			return layout;
		}
			
		private Control GetMainArea()
		{
			this.controlpanel = new GuiControlPanel();

			Control tabcontrol = Widgets.GetTabControl(
				new Control[] {
					new GuiDiagram(this),
					new GuiDetails(),
					new GuiMap(this)},
				new string[] {
					"Diagram",
					"Details",
					"Map"});
		
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(1, 2, 0, BORDER);
			
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        GuiControlPanel.WIDTH));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        DIAGRAM_DIM_X));
			
			layout.Controls.Add(controlpanel, 0, 0);
			layout.Controls.Add(tabcontrol, 1, 0);
			
			// load autosession/init form
			LoadSession();
			
			// initial rendering
			Controller.RenderDiagram(null, null);
			Controller.UpdateMap(null, null);
			Controller.UpdateDetails(null, null);
			
			return layout;
		}
		
		public void OnQuit(object o, EventArgs a)
		{
			try {
				Controller.LocationsSource.StoreList();
				Controller.SaveAutoSession();
			} catch {} // ignore failures, too late to report to user
		}
		
		private void LoadSession()
		{	
			// fill in initial form values
			string file = Controller.AsmInfo.GetSerializePath(Constants.AutoSessionFilename);
			try {
				Controller.LoadAutoSession(file);
				Controller.Report(new Message(Result.OK, "Loaded previous session from: " + file));
			} catch {
				Controller.InitForm();
				Controller.Report(new Message(Result.Fail, "Failed to load previous session from: " + file));
			}
		}
		
		private Size GetFormSize()
		{
			int width = DIAGRAM_DIM_X + GuiControlPanel.WIDTH;
			int height = Math.Max(DIAGRAM_DIM_Y, GuiControlPanel.HEIGHT)
				+ this.menu.Height + this.statusbar.Height;
			return new Size(width, height);
		}
		
		public Size GetViewportSize()
		{
			return new Size(this.ClientSize.Width
			                - GuiControlPanel.WIDTH
			                - 14 // tabcontrol
			                - BORDER*2,
			                this.ClientSize.Height
			                - this.menu.Height
			                - this.statusbar.Height
			                - 30 // tabcontrol
			                - BORDER*2);
		}
	}
}
