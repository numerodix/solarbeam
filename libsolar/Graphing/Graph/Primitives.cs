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
	
	/**
	 * Primities for printing/plotting.
	 */
	partial class Diagram
	{
		private void PrintBoundedString(Graphics g, Font font, Brush txtbrush,
		                                string s, int x, int y, Placement place)
		{
			PrintBoundedString(g, font, txtbrush, s, x, y, place, 0);
		}
		
		/**
		 * @param place states the placement of the string relative to the
		 * coordinate given.
		 */
		private void PrintBoundedString(Graphics g, Font font, Brush txtbrush,
		                                string s, int x, int y, Placement place,
		                                int margin)
		{
			int height = font.Height;
			int width = (int) g.MeasureString(s, font).Width;
			using (SolidBrush br = new SolidBrush(colors.GridBg)) {
				int prepad = width/15;
				int postpad = width/5;
				
				margin = 10; // TODO
				
				// default setup : CENTER
				int a = x - width/2;
				int b = y - height/2;
				int dx = width;
				int dy = height;

				if (place == Placement.TOP_LEFT) {
					a = x;
					b = y;
				} else if (place == Placement.TOP_RIGHT) {
					a = x - width;
					b = y;
				} else if (place == Placement.BOTTOM_LEFT) {
					a = x;
					b = y - height;
				} else if (place == Placement.BOTTOM_RIGHT) {
					a = x - width;
					b = y - height;
				} else if (place == Placement.LEFT) {
					a = x;
					b = y - height/2;
				} else if (place == Placement.RIGHT) {
					a = x - width;
					b = y - height/2;
				} else if (place == Placement.TOP) {
					a = x - width/2;
					b = y;
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
