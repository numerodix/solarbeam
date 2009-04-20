// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

using LibSolar.SolarOrbit;

namespace LibSolar.Graphing
{	
	/**
	 * Paint backdrop on a grid.
	 */
	partial class Grapher
	{		
		public void PaintBackdrop(Graphics g)
		{
			using (Pen pen = new Pen(colors.GridFg, GetLineThickness()))
			using (SolidBrush brush = new SolidBrush(colors.GridFg)) {
				PaintBackground(g);
				DrawCircles(g, pen);
				DrawRadials(g, pen);
				PrintElevationLabels(g, brush);
				PrintDirectionLabels(g, brush);
			}
		}
		
		private void PaintBackground(Graphics g)
		{
			using (SolidBrush br = new SolidBrush(colors.GridBg)) {
				g.FillRectangle(br, grid.A, grid.B, grid.Dx, grid.Dy);
			}
		}
		
		private void DrawCircles(Graphics g, Pen pen)
		{
			for (int r = 0; r < graph.Radius; r += graph.Delta)
			{
				g.DrawEllipse(pen, 
				              graph.A + r, graph.B + r, 
				              graph.Diameter - r*2, graph.Diameter - r*2);
			}
		}
		
		private void DrawRadials(Graphics g, Pen pen)
		{
			for (int ang = 0; ang <= 360; ang += 10)
			{
				double start_pt = graph.Radius / 9;
				// shorten 10, 20 radii
				if (ang % 30 != 0)
				{
					start_pt = graph.Radius * 2 / 9;
				}
				// lengthen 90 radii
				if (ang % 90 == 0)
				{
					start_pt = 0;
				}

				// lengthen 90 radii
				double end_pt = graph.Radius;
				if (ang % 90 == 0)
				{
					end_pt = graph.Radius + grid.Qubit;
				}

				double ang_rad = Coordinate.DegToRad(ang);
				int a = (int) Math.Ceiling( start_pt * Math.Cos(ang_rad) );
				int b = (int) Math.Ceiling( start_pt * Math.Sin(ang_rad) );
				int x = (int) ( end_pt * Math.Cos(ang_rad) );
				int y = (int) ( end_pt * Math.Sin(ang_rad) );

				g.DrawLine(pen, 
				           graph.Origin.X + a, graph.Origin.Y + b,
				           graph.Origin.X + x, graph.Origin.Y + y);
			}
		}
		
		private void PrintElevationLabels(Graphics g, SolidBrush br)
		{
			float font_size = (float) Math.Max(1.0, (double) graph.Delta / 3.0);
			using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel))
			{
				// Print elevation degree labels
				int width = (int) g.MeasureString("90°", font).Width;
				int x = graph.Origin.X - width;
				for (int el = 10; el <= 80; el += 10)
				{
					int y = (graph.Origin.Y - graph.Radius - grid.Qubit*2/3)
						+ (7/4*graph.Delta * el/10);
					PrintBoundedString(g, font, br,
					                   el.ToString()+"°",
					                   x, y, Placement.TOP_LEFT);
				}

				// Print elevation legend
				string s = "Elevation";
				int leg_width = (int) g.MeasureString(s, font).Width;
				int leg_x = graph.Origin.X - leg_width / 2;
				PrintBoundedString(g, font, br, s, leg_x, 
				                   graph.Origin.Y - graph.Radius + (graph.Delta/5),
				                   Placement.TOP_LEFT);
			}
		}
		
		private void PrintDirectionLabels(Graphics g, SolidBrush br)
		{
			int space = (int) ((double) (graph.Delta - grid.Qubit) * (4.0/5.0));
			float font_size = (float) Math.Max(1.0, space);
			using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel))
			{
				int height = font.Height;

				string n_str = "N";
				int n_width = (int) g.MeasureString(n_str, font).Width;
				PrintBoundedString(g, font, br, n_str,
				                   graph.Origin.X - n_width/2,
				                   graph.Origin.Y - graph.Radius - graph.Delta,
				                   Placement.TOP_LEFT);

				string s_str = "S";
				int s_width = (int) g.MeasureString(s_str, font).Width;
				PrintBoundedString(g, font, br, s_str,
				                   graph.Origin.X - s_width/2,
				                   graph.Origin.Y + graph.Radius + grid.Qubit,
				                   Placement.TOP_LEFT);
				
				string w_str = "W";
				PrintBoundedString(g, font, br, w_str,
				                   graph.Origin.X - graph.Radius - graph.Delta,
				                   graph.Origin.Y - height/2,
				                   Placement.TOP_LEFT);

				string e_str = "E";
				PrintBoundedString(g, font, br, e_str,
				                   graph.Origin.X + graph.Radius + grid.Qubit,
				                   graph.Origin.Y - height/2,
				                   Placement.TOP_LEFT);
			}
		}
	}	
}
