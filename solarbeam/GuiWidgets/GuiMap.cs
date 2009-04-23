// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.Mapping;
using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Represents map viewport as widget.
	 */
	sealed class GuiMap : Control
	{
		private const int BORDER = 2;
		private const int BORDER_W = BORDER*2;
		private const int BORDER_H = BORDER;
	
		public static readonly Colors colors = new Colors();
		public const string font_face = "Arial";
		
		private string location;
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
			this.BackColor = colors.MapBg;
			
			this.Paint += delegate { RePaint(); };
			this.Resize += delegate { RePaint(); };
			
			// mouse tracking
			this.MouseMove += delegate (object sender, MouseEventArgs args) {
				Position pos = FindPosition(args.X, args.Y);
				Update(null, pos);
			};
			
			// init double buffer
			buffercontext = BufferedGraphicsManager.Current;
		}

		public void Update(string location, Position pos)
		{			
			this.position = pos;
			this.location = location;
			
			NullifyFinalizedBitmap();
			
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
					if (this.bitmap_base == null) {
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
			int canvas_pos_x = (vp_size.Width/2) - (canvas_size.Width/2);
			int canvas_pos_y = (vp_size.Height/2) - (canvas_size.Height/2);

			using (Graphics gr = this.CreateGraphics())
			using (BufferedGraphics frame = 
				      buffercontext.Allocate(gr, this.ClientRectangle))
			{
				// explicitly repaint whole control surface to prevent pixel noise
				using (SolidBrush brush = new SolidBrush(colors.MapBg)) {
					frame.Graphics.FillRectangle(brush, 0, 0, 
					                             vp_size.Width, vp_size.Height);
				}
				frame.Graphics.DrawImage(bitmap,
				                         canvas_pos_x, canvas_pos_y, 
				                         canvas_size.Width, canvas_size.Height);
				frame.Render();
			}
		}
		
		private Bitmap GenerateBaseImageBitmap()
		{
			Size dim = GetCanvasDimensions();
			this.mapbitmap = new MapBitmap(dim.Width, dim.Height, colors, font_face);
			return mapbitmap.RenderBaseImage();
		}
		
		private Bitmap GenerateFinalizedBitmap()
		{
			return mapbitmap.RenderCurrentPositionCloned(location, position);
		}
		
		private void NullifyFinalizedBitmap()
		{
			Memory.Collect(this.bitmap_final);
			this.bitmap_final = null;
		}
		
		private Size GetCanvasDimensions()
		{
			Size vp_size = gui.GetViewportSize();
			int w = Math.Max(1, vp_size.Width - BORDER_W*5);
			int h = Math.Max(1, vp_size.Height - BORDER_H*2);
			
			return new Size(w, h);
		}
		
		public Position FindPosition(int x, int y)
		{
			return mapbitmap.FindPosition(new Point(x - BORDER_W, y - BORDER_H));
		}
	}
}
