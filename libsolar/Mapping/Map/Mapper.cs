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
		
		public void PlotPosition(Graphics g, Position pos)
		{
			Point point = FindMapPoint(pos);
			int rad = Math.Max(2, GetLineThickness());
			using (SolidBrush brush = new SolidBrush(Color.Red)) {
				g.FillEllipse(brush, point.X - rad, point.Y - rad, rad*2, rad*2);
			}
		}
		
		private Point FindMapPoint(Position pos)
		{
			Degree latd = pos.LatitudeDegree;
			Degree lond = pos.LongitudeDegree;
			
			double lon_val = Position.CollapsePositionUnits(lond.Direction,
			                                                lond.Deg, lond.Min, lond.Sec);
			double lat_val = Position.CollapsePositionUnits(latd.Direction,
			                                                latd.Deg, latd.Min, latd.Sec);
			
			double lon = (lon_val / 3600); 
			double lat = -1 * lat_val / 3600;
			
			double lon_pivot = 10.9;
			
			if (lon < (-180 + lon_pivot)) {
				lon = (180.0 - lon_pivot) + (180.0 - Math.Abs(lon));
			} else {
				lon -= lon_pivot;
			}
			
			int px = (int) (map.Origin.X + ((lon / 360.0) * map.Dx));
			int py = (int) (map.Origin.Y + ((lat / 180.0) * map.Dy));
			return new Point(px, py);
		}
				
		private int GetLineThickness()
		{
			return Math.Max(1, map.Dx / 200);
		}
	}
}		
