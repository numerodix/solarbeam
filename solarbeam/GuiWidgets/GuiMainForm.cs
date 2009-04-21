// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.SolarOrbit;

namespace SolarbeamGui
{
	/**
	 * Represents the main gui form.
	 */
	sealed class GuiMainForm : Form
	{	
		private const int BORDER = 0;
		private const int DIAGRAM_DIM_X = GuiDiagram.IDEAL_DIM_X;
		private const int DIAGRAM_DIM_Y = GuiDiagram.IDEAL_DIM_Y;
		
		// make about form accessible to logic
		public static GuiShortcutInstall shortcutform;
		public static GuiAbout aboutform;
		
		// my widgets
		private GuiMenu menu;
		private GuiControlPanel controlpanel;
		private GuiDiagram diagram;
		
		public GuiMainForm(string form_title, string icon)
		{
//			this.SuspendLayout();
			
			InitializeComponent(form_title, icon);
			shortcutform = new GuiShortcutInstall(form_title, icon);
			aboutform = new GuiAbout(form_title, icon);

			// makes mono layout differently 1.9 <-> 2.0
			// VS default: 6F 13F (win ok)
			// mono 1.9: 6F 14F (win ok)
			// mono 2.0: 7F 14F
//			this.AutoScaleDimensions = new SizeF(6F, 13F);
			
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = GetFormSize();
//			this.ResumeLayout(false);
//			this.PerformLayout();
		}
		
		private void InitializeComponent(string form_title, string icon)
		{
			this.DoubleBuffered = true;
			this.Text = form_title;
			this.Icon = new Icon(Controller.AsmInfo.GetResource(icon));
			
			// init datasources before instantiating widgets
			Controller.InitSources();
			
			Controller.SplashQueue.Enqueue("Initializing gui");
			this.menu = new GuiMenu();
			Control mainarea = GetMainArea();
			
			this.MainMenuStrip = menu;
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(2, 1, 0, BORDER);
			
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        menu.Height));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        mainarea.Height));
			
			layout.Controls.Add(menu, 0, 0);
			layout.Controls.Add(mainarea, 0, 1);
			
			this.Controls.Add(layout);
			
			this.Closed += new EventHandler(OnQuit);
			
			// try to bring to front somehow
			this.Load += delegate (object o, EventArgs a) {
				this.Activate();
				this.BringToFront();
				this.Focus();
			};

		}
			
		private void OnQuit(object o, EventArgs a)
		{
			Controller.SaveAutoSession();
		}
		
		private Control GetMainArea()
		{
			this.controlpanel = new GuiControlPanel();
			
			// fill in initial form values
			try {
				Controller.LoadAutoSession();
			} catch {
				Controller.InitForm();
			}
			
			Control tabcontrol = Widgets.GetTabControl(
				new Control[] {
					new GuiDiagram(this),
					new GuiMap(this)},
				new string[] {
					"Diagram",
					"Map"});
		
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(1, 2, 0, BORDER);
			
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        GuiControlPanel.WIDTH));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        DIAGRAM_DIM_X));
			
			layout.Controls.Add(controlpanel, 0, 0);
			layout.Controls.Add(tabcontrol, 1, 0);
			
			// initial rendering
			Controller.RenderDiagram(null, null);
			Controller.UpdateMap(null, null);
			
			return layout;
		}
		
		private Size GetFormSize()
		{
			int width = DIAGRAM_DIM_X + GuiControlPanel.WIDTH;
			int height = Math.Max(DIAGRAM_DIM_Y, GuiControlPanel.HEIGHT)
				+ this.menu.Height;
			return new Size(width, height);
		}
		
		public Size GetViewportSize()
		{
//			Console.WriteLine("GuiMainForm this.ClientSize.Width {0}", this.ClientSize.Width);
//			Console.WriteLine("GuiMainForm GuiControlPanel.WIDTH {0}", GuiControlPanel.WIDTH);
//			Console.WriteLine("GuiMainForm tabcontrol 10");
//			Console.WriteLine("GuiMainForm BORDER*2 {0}", BORDER*2);
//			Console.WriteLine("GuiMainForm width {0}", this.ClientSize.Width - GuiControlPanel.WIDTH - 14 - BORDER*2);

//			Console.WriteLine("GuiMainForm this.ClientSize.Height {0}", this.ClientSize.Height);
//			Console.WriteLine("GuiMainForm this.menu.Height {0}", this.menu.Height);
///			Console.WriteLine("GuiMainForm tabcontrol 10");
//			Console.WriteLine("GuiMainForm BORDER*2 {0}", BORDER*2);
//			Console.WriteLine("GuiMainForm height {0}", this.ClientSize.Height - this.menu.Height - 30 - BORDER*2);

//			Console.WriteLine("");
			return new Size(this.ClientSize.Width
			                - GuiControlPanel.WIDTH
			                - 14 // tabcontrol
			                - BORDER*2,
			                this.ClientSize.Height
			                - this.menu.Height
			                - 30 // tabcontrol
			                - BORDER*2);
		}
	}
}
