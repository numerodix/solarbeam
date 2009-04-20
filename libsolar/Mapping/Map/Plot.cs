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
		public void RenderMapBitmap(Graphics g, Bitmap bitmap_map)
		{
			g.DrawImage(bitmap_map,
			            map.A, map.B,
			            map.Dx, map.Dy);
		}
		
		public void PlotPosition(Graphics g, string location, Position pos)
		{
			Point point = FindMapPoint(pos);
			float font_size = GetCursorFontSize();
			if (font_size > 10) {
				PlotPositionCursor(g, location, pos, point);
			} else {
				PlotPositionDot(g, point);
			}
		}
		
		public void PlotPositionCursor(Graphics g, 
		                               string location, Position pos, Point point)
		{
			DrawCursor(g, point);
			
			// print strings
			float font_size = GetCursorFontSize();
			using (SolidBrush brush = new SolidBrush(colors.Text))
			using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel)) {
				
				List<string> stack = new List<string>();
				stack.Add(pos.LongitudeDegree.Print());
				stack.Add(pos.LatitudeDegree.Print());
				if ((location != null) && (location != string.Empty)) stack.Add(location);
				
				// find longest string
				int longest = 0;
				foreach (string s in stack) {
					float w = g.MeasureString(s, font).Width;
					if (w > longest) longest = (int) w;
				}
				
				// font padding
				int d = (int) ((double) font_size * 0.3);
				
				int a = point.X;
				int b = point.Y;
				int db = (int) font_size * stack.Count;
				// string won't fit, switch orientation
				if (((point.X + longest > map.X) || (point.X - longest < map.A))
				    || ((point.Y + db > map.Y) || (point.Y - db < map.B))) {
					d *= -1;
					a -= longest;
					b += db;
				}
				
				// print
				int i = 0;
				foreach (string s in stack) {
					++i;
					g.DrawString(s, font, brush,
					             a + d,
					             b - d - font_size*i);
				}
			}
		}
		
		private void DrawCursor(Graphics g, Point point)
		{
			int len = (int) (GetCursorFontSize() * 3.5);
			using (SolidBrush brush = new SolidBrush(colors.Cursor))
			using (Pen pen = new Pen(brush)) {
				g.DrawLine(pen,
				           new Point(point.X-len, point.Y),
				           new Point(point.X+len, point.Y));
				g.DrawLine(pen,
				           new Point(point.X, point.Y-len),
				           new Point(point.X, point.Y+len));
			}
		}
		
		private void PlotPositionDot(Graphics g, Point point)
		{
			int rad = Math.Max(2, GetLineThickness());
			using (SolidBrush brush = new SolidBrush(Color.Red)) {
				g.FillEllipse(brush, point.X - rad, point.Y - rad, rad*2, rad*2);
			}
		}
	}
}		
