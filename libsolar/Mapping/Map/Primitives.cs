// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;

using LibSolar.Types;

namespace LibSolar.Mapping
{	
	partial class Mapper
	{
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
		
		private float GetCursorFontSize()
		{
			return (float) Math.Max(1.0, (double) map.Dx / 50.0);
		}
				
		private int GetLineThickness()
		{
			return Math.Max(1, map.Dx / 200);
		}
	}
}		
