// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

using LibSolar.Types;

namespace LibSolar.Graphing
{
	/**
	 * The class encapsulates the graphing primitives into chunks of popular
	 * use.
	 */
	public class GraphBitmap
	{
		private int dimensions;
		private string font_face;
		private Colors colors;
		
		private Bitmap bitmap;
		private Diagram diagram;
		
		public GraphBitmap(int dim, Colors colors, string font_face)
		{
			this.dimensions = dim;
			this.font_face = font_face;
			this.colors = colors;
		}
		
		public Bitmap RenderBaseImage(Position pos, UTCDate udt)
		{
			bitmap = new Bitmap(dimensions, dimensions);
			
			diagram = new Diagram(0, 0, dimensions, dimensions,
			                      colors, font_face);
			
			using (Graphics g = Graphics.FromImage(bitmap)) {
				
				// paint backdrop coordinate system
				diagram.PaintBackdrop(g);
				
				// plot milestone dates first half of the year
				diagram.PlotMilestoneDay(g, colors.YearFstHalf, pos, udt.SetDate(1, 1));
				diagram.PlotMilestoneDay(g, colors.YearFstHalf, pos, udt.SetDate(2, 4));
				diagram.PlotMilestoneDay(g, colors.YearFstHalf, pos, udt.SetDate(3, 6));
				diagram.PlotMilestoneDay(g, colors.YearFstHalf, pos, udt.SetDate(4, 5));
				diagram.PlotMilestoneDay(g, colors.YearFstHalf, pos, udt.SetDate(5, 5));
				diagram.PlotMilestoneDay(g, colors.YearFstHalf, pos, udt.SetDate(6, 4));
		
				// plot milestone dates second half of the year
				diagram.PlotMilestoneDay(g, colors.YearSndHalf, pos, udt.SetDate(6, 21));
				diagram.PlotMilestoneDay(g, colors.YearSndHalf, pos, udt.SetDate(7, 21));
				diagram.PlotMilestoneDay(g, colors.YearSndHalf, pos, udt.SetDate(8, 20));
				diagram.PlotMilestoneDay(g, colors.YearSndHalf, pos, udt.SetDate(9, 19));
				diagram.PlotMilestoneDay(g, colors.YearSndHalf, pos, udt.SetDate(10, 19));
				diagram.PlotMilestoneDay(g, colors.YearSndHalf, pos, udt.SetDate(11, 18));
				diagram.PlotMilestoneDay(g, colors.YearSndHalf, pos, udt.SetDate(12, 21));
		
				// plot analemma curves
				for (int i = 0; i < 24; i++) {
					diagram.PlotAnalemma(g, colors.YearFstHalf, colors.YearSndHalf,
					                     pos, udt.Timezone, udt.Year, i);
				}

				// print milestone day labels
				diagram.PrintMilestoneDayLabels(g);
				
				// print analemma labels
				for (int i = 0; i < 24; i++) {
					diagram.PrintAnalemmaLabel(g, colors.GraphFg,
					                           pos, udt.Timezone, udt.Year, i);
				}
			}
			
			return this.bitmap;
		}
		
		public Bitmap RenderCurrentDayCloned(int dim, 
		                                     Position pos, UTCDate dt)
		{
			return RenderCurrentDay((Bitmap) this.bitmap.Clone(), dim, pos, dt);
		}
		
		public Bitmap RenderCurrentDay(Bitmap bitmap, int dim, 
		                               Position pos, UTCDate udt)
		{
			using (Graphics g = Graphics.FromImage(bitmap)) {
				diagram.PlotDay(g, colors.CurrentDay, pos, udt);
				diagram.PlotSun(g, colors.CurrentDay, dimensions, pos, udt);
			}
			return bitmap;
		}
	
		public void SaveBitmap(Bitmap bitmap, string path)
		{
			bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);	
		}
	}
}
