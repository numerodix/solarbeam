// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;

using LibSolar.Types;

namespace LibSolar.Mapping
{
	partial class Mapper
	{
		private Map map;
		private Colors colors;
		private string font_face;
		
		
		public Mapper(int bound_a, int bound_b, int bound_x, int bound_y,
		              Colors colors, string font_face)
		{
			this.map = new Map(bound_a, bound_b, bound_x, bound_y);
			this.colors = colors;
			this.font_face = font_face;
		}
		
		public void RenderMapBitmap(Graphics g, Bitmap bitmap_map)
		{
			g.DrawImage(bitmap_map,
			            map.A, map.B,
			            map.Dx, map.Dy);
		}
		
	}
}		
