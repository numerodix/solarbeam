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
		
		public void PlotPosition(Graphics g, string location, Position pos)
		{
			Point point = FindMapPoint(pos);
			if (map.Dx > 300) {
				PlotPositionCursor(g, location, pos, point);
			} else {
				PlotPositionDot(g, point);
			}
		}
		
		public void PlotPositionCursor(Graphics g, 
		                               string location, Position pos, Point point)
		{
			int line = Math.Max(1, GetLineThickness());
			float font_size = GetCursorFontSize();
			int len = (int) (font_size * 3.5);
			using (SolidBrush brush = new SolidBrush(colors.Cursor))
			using (Pen pen = new Pen(brush)) {
				g.DrawLine(pen,
				           new Point(point.X-len, point.Y),
				           new Point(point.X+len, point.Y));
				g.DrawLine(pen,
				           new Point(point.X, point.Y-len),
				           new Point(point.X, point.Y+len));
			}
			
			using (SolidBrush brush = new SolidBrush(colors.Text))
			using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel)) {
				
				List<string> stack = new List<string>();
				stack.Add(pos.LongitudeDegree.Print());
				stack.Add(pos.LatitudeDegree.Print());
				stack.Add(location);
				
				int d = (int) ((double) font_size * 0.3);
				int i = 0;
				foreach (string s in stack) {
					if (s != null) {
						++i;
						g.DrawString(s, font, brush,
						             point.X + d,
						             point.Y - d - font_size*i);
					}
				}
			}
		}
		
		private void PlotPositionDot(Graphics g, Point point)
		{
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
				
		private float GetCursorFontSize()
		{
			return (float) Math.Max(1.0, (double) map.Dx / 60.0);
		}
				
		private int GetLineThickness()
		{
			return Math.Max(1, map.Dx / 200);
		}
	}
}		
