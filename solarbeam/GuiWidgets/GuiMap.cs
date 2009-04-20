// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.Mapping;
using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Represents map viewport as widget.
	 */
	sealed class GuiMap : Control
	{
		private const int BORDER = 5;
	
		public const string font_face = "Arial";
		
		private Position position;
		private MapBitmap mapbitmap;
		private Bitmap bitmap_base;
		private Bitmap bitmap_final;
		
		GuiMainForm gui;
		
		private BufferedGraphicsContext buffercontext;
		
		
		public GuiMap(GuiMainForm gui)
		{
			this.gui = gui;
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			Controller.RegisterControl(Controller.Id.MAP, this);	// register control
			
			this.Dock = DockStyle.Fill;
			this.BackColor = Color.White;
			
			this.Paint += delegate { RePaint(); };
			this.Resize += delegate { RePaint(); };
			
			// init double buffer
			buffercontext = BufferedGraphicsManager.Current;
		}
		
		public void Update(Position pos)
		{			
			this.position = pos;
			
			this.bitmap_final = null;
			RePaint();
		}
		
		private void RePaint()
		{
			if (position != null)
			{
				Size dim = GetCanvasDimensions();
				
				// bitmap exists, wrong dimensions
				if ((this.bitmap_final != null)
				    && ((this.bitmap_final.Width != dim.Width)
				        || (this.bitmap_final.Height != dim.Height)))
				{
					this.bitmap_base = GenerateBaseImageBitmap();
					this.bitmap_final = GenerateFinalizedBitmap();
					
				// bitmap is null
				} else if (this.bitmap_final == null)	{
					if (this.bitmap_base == null)
					{
						this.bitmap_base = GenerateBaseImageBitmap();
					}
					this.bitmap_final = GenerateFinalizedBitmap();
				}
				
				// render
				RenderBitmap(this.bitmap_final);
			}
		}
		
		private void RenderBitmap(Bitmap bitmap)
		{
			Size vp_size = gui.GetViewportSize();
			Size canvas_size = GetCanvasDimensions();
			
			double aspect = (double) bitmap.Width / (double) bitmap.Height;
			
			int w = canvas_size.Width;
			int h = canvas_size.Height;
			
			if (aspect >= 1)
				h = (int) ((double) w / aspect);
			else
				w = (int) ((double) h / aspect);
			
			int canvas_pos_x = (vp_size.Width/2) - (w/2);
			int canvas_pos_y = (vp_size.Height/2) - (h/2);
			
			using (Graphics gr = this.CreateGraphics())
			using (BufferedGraphics frame = 
				      buffercontext.Allocate(gr, this.ClientRectangle))
			{
				// explicitly repaint whole control surface to prevent pixel noise
				using (SolidBrush brush = new SolidBrush(this.BackColor)) {
					frame.Graphics.FillRectangle(brush, 0, 0, 
					                             vp_size.Width, vp_size.Height);
				}
				frame.Graphics.DrawImage(bitmap,
				                         canvas_pos_x, canvas_pos_y, 
				                         w, h);
				frame.Render();
			}
		}
		
		private Bitmap GenerateBaseImageBitmap()
		{
			this.mapbitmap = new MapBitmap(font_face);
			return mapbitmap.RenderBaseImage();
		}
		
		private Bitmap GenerateFinalizedBitmap()
		{
			return mapbitmap.RenderCurrentPositionCloned(position);
		}
		
		private Size GetCanvasDimensions()
		{
			Size vp_size = gui.GetViewportSize();
			int w = Math.Max(1, vp_size.Width - BORDER*2);
			int h = Math.Max(1, vp_size.Height - BORDER*2);
			return new Size(w, h);
		}
	}
}
