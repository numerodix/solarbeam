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
		public void PlotPosition(Graphics g, string location, Position pos)
		{
			Point point = FindMapPoint(pos);
			float font_size = GetCursorFontSize();
			
			// draw cursor, font is readable
			if (font_size > 10) {
				PlotPositionCursor(g, location, pos, point);
				
			// draw dot
			} else {
				PlotPositionDot(g, point);
			}
		}
		
		private void PlotPositionCursor(Graphics g, 
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
				int w = 0;
				foreach (string s in stack) {
					float wx = g.MeasureString(s, font).Width;
					if (wx > w) w = (int) wx;
				}
				
				// font padding
				int margin = (int) ((double) font_size * 0.3);
				int h = (int) font_size * stack.Count;
				
				// default: upper right
				int da = margin;
				int db = -margin;
				
				// try lower left
				if (!WithinXBound(point.X, da, w) || !WithinYBound(point.Y, db, h)) {
					da = -w - margin;
					db = h + margin;
				}
				
				// try lower right
				if (!WithinXBound(point.X, da, w) || !WithinYBound(point.Y, db, h)) {
					da = margin;
					db = h + margin;
				}
				
				// try upper left
				if (!WithinXBound(point.X, da, w) || !WithinYBound(point.Y, db, h)) {
					da = -w - margin;
					db = -margin;
				}
				
				// print
				int i = 0;
				foreach (string s in stack) {
					++i;
					g.DrawString(s, font, brush,
					             point.X + da,
					             point.Y + db - font_size*i);
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
	}
}		
