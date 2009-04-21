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
		
		private static Bitmap bitmap_map;
		private Bitmap bitmap;
		private Mapper mapper;
		
		static MapBitmap()
		{
			AsmInfo asminfo = new AsmInfo(Assembly.GetExecutingAssembly());
			MapBitmap.bitmap_map = new Bitmap(asminfo.GetResource("worldmap.png"));
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
			
			double aspect = (double) bitmap_map.Width / (double) bitmap_map.Height;
			
			int w = dimension_x;
			int h = dimension_y;
			
			if (aspect >= 1)
				h = (int) Math.Max(1, ((double) w / aspect));
			else
				w = (int) Math.Max(1, ((double) h / aspect));
			
			int a = (dimension_x/2) - (w/2);
			int b = (dimension_y/2) - (h/2);
			
			mapper = new Mapper(a, b, a+w, b+h, 
			                    colors, font_face);
			
			using (Graphics g = Graphics.FromImage(bitmap)) {
				mapper.RenderMapBitmap(g, bitmap_map);
			}
			
			return this.bitmap;
		}
		
		public Bitmap RenderCurrentPositionCloned(string location, Position pos)
		{
			return RenderCurrentPosition((Bitmap) this.bitmap.Clone(),
			                             location, pos);
		}
		
		public Bitmap RenderCurrentPosition(Bitmap bitmap,
		                                    string location, Position pos)
		{
			using (Graphics g = Graphics.FromImage(bitmap)) {
				mapper.PlotPosition(g, location, pos);
			}
			return bitmap;
		}
		
		public Position FindPosition(int X, int Y)
		{
			return mapper.FindPosition(X, Y);
		}
		
		public void SaveBitmap(Bitmap bitmap, string path)
		{
			bitmap.Save(path, ImageFormat.Png);	
		}
	}
}
