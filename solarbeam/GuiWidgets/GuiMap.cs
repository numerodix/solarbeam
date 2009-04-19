// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.Graphing;
using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Represents diagram viewport as widget.
	 */
	sealed class GuiMap : Control
	{
		public const int IDEAL_DIM_X = 569;
		public const int IDEAL_DIM_Y = 526;
		
		private const int BORDER = 5;
	
		public static readonly Colors colors = new Colors();
		public const string font_face = "Arial";
		
		private Position position;
		private UTCDate? date;
		private GraphBitmap graphbitmap;
		private Bitmap bitmape_base;
		private Bitmap bitmape_final;
		
		GuiMainForm gui;
		
		private BufferedGraphicsContext buffercontext;
		
		
		public GuiMap(GuiMainForm gui)
		{
			this.gui = gui;
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			Controller.RegisterControl(Controller.Id.DIAGRAM, this);	// register control
			
			this.Dock = DockStyle.Fill;
			this.BackColor = colors.GridBg;
			
			this.Paint += delegate { RePaint(); };
			this.Resize += delegate { RePaint(); };
			
			// init double buffer
			buffercontext = BufferedGraphicsManager.Current;
		}
		
		public void Update(UTCDate date)
		{
			this.date = date;
			
			this.bitmape_final = null;
			RePaint();
		}
		
		public void ReRender(Position pos, UTCDate date)
		{
			this.position = pos;
			this.date = date;
			
			this.bitmape_base = null;
			this.bitmape_final = null;
			RePaint();
		}
		
		private void RePaint()
		{
			if ((position != null) && (date != null))
			{
				int dim = GetCanvasDimensions();
				
				// bitmap exists, wrong dimensions
				if ((this.bitmape_final != null)
				    && ((this.bitmape_final.Width != dim)
				        || (this.bitmape_final.Height != dim)))
				{
					this.bitmape_base = GenerateBaseImageBitmap();
					this.bitmape_final = GenerateFinalizedBitmap();
					
				// bitmap is null
				} else if (this.bitmape_final == null)	{
					if (this.bitmape_base == null)
					{
						this.bitmape_base = GenerateBaseImageBitmap();
					}
					this.bitmape_final = GenerateFinalizedBitmap();
				}
				
				// render
				RenderBitmap(this.bitmape_final);
			}
		}
		
		private void RenderBitmap(Bitmap bitmap)
		{
			Size vp_size = gui.GetViewportSize();
			int canvas_size = GetCanvasDimensions();
			int canvas_pos_x = (vp_size.Width/2) - (canvas_size/2);
			int canvas_pos_y = (vp_size.Height/2) - (canvas_size/2);
				
			using (Graphics gr = this.CreateGraphics())
			using (BufferedGraphics frame = 
				      buffercontext.Allocate(gr, this.ClientRectangle))
			{
				// explicitly repaint whole control surface to prevent pixel noise
				using (SolidBrush brush = new SolidBrush(colors.GridBg))
				{
					frame.Graphics.FillRectangle(brush, 0, 0, 
					                             vp_size.Width, vp_size.Height);
				}
				frame.Graphics.DrawImage(bitmap, 
				                         canvas_pos_x, canvas_pos_y, 
				                         canvas_size, canvas_size);
				frame.Render();
			}
		}
		
		private Bitmap GenerateBaseImageBitmap()
		{
			return new Bitmap(Controller.AsmInfo.GetResource("worldmap.png"));
//			int dim = GetCanvasDimensions();
//			this.graphbitmap = new GraphBitmap(false, dim, colors, font_face);
//			return graphbitmap.RenderBaseImage(position, date.Value);
		}
		
		private Bitmap GenerateFinalizedBitmap()
		{
			return this.bitmape_base;
//			int dim = GetCanvasDimensions();
//			return graphbitmap.RenderCurrentDayCloned(dim, 
//			                                          position, date.Value);
		}
		
		private int GetCanvasDimensions()
		{
			Size vp_size = gui.GetViewportSize();
			return Math.Max(1,
				(Math.Min(vp_size.Width, vp_size.Height) - BORDER*2)); // make sure >=1
		}
	}
}
