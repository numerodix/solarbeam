// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;

using LibSolar.Types;
using LibSolar.Util;

namespace LibSolar.Mapping
{
	public class MapBitmap
	{
		private int dimension_x;
		private int dimension_y;
		private string font_face;
		private Colors colors;
		
		private Bitmap bitmap;
		private static AsmInfo asminfo;
		
		static MapBitmap()
		{
			MapBitmap.asminfo = new AsmInfo(Assembly.GetExecutingAssembly());
		}
		
		public MapBitmap(int dim_x, int dim_y, Colors colors, string font_face)
		{
			this.dimension_x = dim_x;
			this.dimension_y = dim_y;
			this.font_face = font_face;
			this.colors = colors;
		}
		
		/**
		 * Produce a bitmap of the requested size, with the map rendered in
		 * the center of it, preserving aspect ratio.
		 */
		public Bitmap RenderBaseImage()
		{
			bitmap = new Bitmap(dimension_x, dimension_y);
			
			Bitmap map = new Bitmap(asminfo.GetResource("worldmap.png"));
			double aspect = (double) map.Width / (double) map.Height;
			
			int w = dimension_x;
			int h = dimension_y;
			
			if (aspect >= 1)
				h = (int) ((double) w / aspect);
			else
				w = (int) ((double) h / aspect);
			
			int canvas_pos_x = (dimension_x/2) - (w/2);
			int canvas_pos_y = (dimension_y/2) - (h/2);
			
			using (Graphics g = Graphics.FromImage(bitmap)) {
				g.DrawImage(map,
				            canvas_pos_x, canvas_pos_y, 
				            w, h);
			}
			
			return this.bitmap;
		}
		
		public Bitmap RenderCurrentPositionCloned(Position pos)
		{
			return RenderCurrentPosition((Bitmap) this.bitmap.Clone(), pos);
		}
		
		public Bitmap RenderCurrentPosition(Bitmap bitmap, Position pos)
		{
			using (Graphics g = Graphics.FromImage(bitmap))
			using (SolidBrush brush = new SolidBrush(Color.Red))
			using (Pen pen = new Pen(brush)) {
				g.DrawEllipse(pen, 10, 10, 40, 40);
			}
			return bitmap;
		}
		
		public void SaveBitmap(Bitmap bitmap, string path)
		{
			bitmap.Save(path, ImageFormat.Png);	
		}
	}
}
