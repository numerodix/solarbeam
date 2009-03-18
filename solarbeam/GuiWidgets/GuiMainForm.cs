// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.Platform;
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
		
		private GuiControlPanel controlpanel;
		private GuiViewport viewport;
		
		public GuiMainForm(string form_title)
		{
			//		this.SuspendLayout();
			
			Size sz = InitializeComponent(form_title);

			// makes mono layout differently 1.9 <-> 2.0
			// VS default: 6F 13F (win ok)
			// mono 1.9: 6F 14F (win ok)
			// mono 2.0: 7F 14F
//			this.AutoScaleDimensions = new SizeF(6F, 13F);
			
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = sz;
	//		this.ResumeLayout(false);
	//		this.PerformLayout();
		}
		
		private Size InitializeComponent(string form_title)
		{
			this.Text = String.Format("{0} ({1}/{2})", form_title, 
			                          Platform.GetRuntime(),
			                          Platform.GetPlatform());
			
			// init datasources before instantiating widgets
			Controller.InitSources();
			
			this.controlpanel = new GuiControlPanel();
			Controller.InitForm(); // fill in initial form values
			this.viewport = new GuiViewport(this);
		
			TableLayoutPanel layout = GuiCommon.GetTableLayoutPanel(1, 2, 0, BORDER);
			
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        GuiControlPanel.WIDTH));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,
			                                        VIEWPORT_DIM_X));
			
			layout.Controls.Add(controlpanel, 0, 0);
			layout.Controls.Add(viewport, 1, 0);
			
			this.Controls.Add(layout);
			
			// initial rendering
			Controller.RenderViewport(null, null);
			
			// report my size
			int width = VIEWPORT_DIM_X + GuiControlPanel.WIDTH;
			int height = Math.Max(VIEWPORT_DIM_Y, GuiControlPanel.HEIGHT);
			return new Size(width, height);
		}
		
		public Size GetViewportSize()
		{
			return new Size(this.ClientSize.Width - GuiControlPanel.WIDTH - BORDER*2,
			                this.ClientSize.Height - BORDER*2);
		}
	}
}
