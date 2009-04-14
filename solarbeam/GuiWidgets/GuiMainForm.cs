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
		private const int VIEWPORT_DIM_X = GuiViewport.IDEAL_DIM_X;
		private const int VIEWPORT_DIM_Y = GuiViewport.IDEAL_DIM_Y;
		
		// make about form accessible to logic
		public static GuiAbout aboutform;
		
		// my widgets
		private GuiMenu menu;
		private GuiControlPanel controlpanel;
		private GuiViewport viewport;
		
		public GuiMainForm(string form_title)
		{
			//		this.SuspendLayout();
			
			InitializeComponent(form_title);
			aboutform = new GuiAbout(form_title);

			// makes mono layout differently 1.9 <-> 2.0
			// VS default: 6F 13F (win ok)
			// mono 1.9: 6F 14F (win ok)
			// mono 2.0: 7F 14F
//			this.AutoScaleDimensions = new SizeF(6F, 13F);
			
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = GetFormSize();
	//		this.ResumeLayout(false);
	//		this.PerformLayout();
		}
		
		private void InitializeComponent(string form_title)
		{
			this.DoubleBuffered = true;
			this.Text = form_title;
			this.Icon = new Icon(Controller.AsmInfo.GetResource("solarbeam.ico"));
			
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
			
			this.viewport = new GuiViewport(this);
		
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(1, 2, 0, BORDER);
			
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        GuiControlPanel.WIDTH));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        VIEWPORT_DIM_X));
			
			layout.Controls.Add(controlpanel, 0, 0);
			layout.Controls.Add(viewport, 1, 0);
			
			// initial rendering
			Controller.RenderViewport(null, null);
			
			return layout;
		}
		
		private Size GetFormSize()
		{
			int width = VIEWPORT_DIM_X + GuiControlPanel.WIDTH;
			int height = Math.Max(VIEWPORT_DIM_Y, GuiControlPanel.HEIGHT)
				+ this.menu.Height;
			return new Size(width, height);
		}
		
		public Size GetViewportSize()
		{
			return new Size(this.ClientSize.Width
			                - GuiControlPanel.WIDTH
			                - BORDER*2,
			                this.ClientSize.Height
			                - this.menu.Height
			                - BORDER*2);
		}
	}
}
