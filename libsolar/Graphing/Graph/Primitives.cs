// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;

using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace LibSolar.Graphing
{
	enum Placement
	{
		TOP_LEFT,
		TOP_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_RIGHT,
		CENTER,
		LEFT,
		RIGHT,
		BOTTOM,
		TOP
	}
	
	partial class Diagram
	{
		private void PrintBoundedString(Graphics g, Font font, Color color,
		                                string s, int x, int y, Placement place)
		{
			using (SolidBrush br_txt = new SolidBrush(color)) {
				PrintBoundedString(g, font, br_txt, s, x, y, place);
			}
		}
		
		/**
		 * @param place states the placement of the string relative to the
		 * coordinate given.
		 */
		private void PrintBoundedString(Graphics g, Font font, Brush txtbrush,
		                               string s, int x, int y, Placement place)
		{
			int height = font.Height;
			int width = (int) g.MeasureString(s, font).Width;
			using (SolidBrush br = new SolidBrush(colors.GridBg)) {
				int prepad = width/15;
				int postpad = width/5;
				
				// default setup : TOP_LEFT
				int a = x;
				int b = y;
				int dx = width;
				int dy = height;

				if (place == Placement.TOP_RIGHT) {
					a = x - width;
				} else if (place == Placement.BOTTOM_LEFT) {
					b = y - height;
				} else if (place == Placement.BOTTOM_RIGHT) {
					a = x - width;
					b = y - height;
				} else if (place == Placement.CENTER) {
					a = x - width/2;
					b = y - height/2;
				} else if (place == Placement.LEFT) {
					b = y - height/2;
				} else if (place == Placement.RIGHT) {
					a = x - width;
					b = y - height/2;
				} else if (place == Placement.TOP) {
					a = x - width/2;
				} else if (place == Placement.BOTTOM) {
					a = x - width/2;
					b = y - height;
				}

				g.FillRectangle(br, a - prepad, b, dx + postpad, dy);
				g.DrawString(s, font, txtbrush, a, b);
			}
		}
		
		private Point? FindPoint(double azimuth, double elevation)
		{
			Point? point = null;
			if (elevation >= 0) {
				elevation = Math.Abs(elevation);
				double z = graph.Radius * ((90.0 - elevation) / 90.0);

				double az_rad = Coordinate.DegToRad(azimuth);
				int x = (int) (z * Math.Sin(az_rad));
				int y = (int) (z * Math.Cos(az_rad));

				int xx = graph.Origin.X + x;
				int yy = graph.Origin.Y - y;

				point = new Nullable<Point>(new Point(xx, yy));
			}
			return point;
		}
		
		private double GetResolutionStep(double space)
		{
			return ( space / (double) grid.Diameter ) / 2;
		}
		
	}
}
