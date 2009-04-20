// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Imaging;

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
		private int titleheight;
		private int captionheight;
		private string font_face;
		private Colors colors;
		
		private Bitmap bitmap;
		private Grapher grapher;
		
		public GraphBitmap(bool caption, int dim, Colors colors, string font_face)
		{
			this.dimensions = dim;
			this.titleheight = caption ? (int) (.07 * (double) dim) : 0;
			this.captionheight = caption ? (int) (.34 * (double) dim) : 0;
			this.font_face = font_face;
			this.colors = colors;
		}
		
		public Bitmap RenderBaseImage(Position pos, UTCDate udt)
		{
			bitmap = new Bitmap(dimensions, titleheight+dimensions+captionheight);
			
			grapher = new Grapher(0, titleheight, dimensions, titleheight+dimensions,
			                      colors, font_face);
			
			using (Graphics g = Graphics.FromImage(bitmap)) {
				
				// paint backdrop coordinate system
				grapher.PaintBackdrop(g);
				
				// plot milestone dates
				int[] days = new int[] {
					 1, 1,    4, 2,    6, 3,    5, 4,    5, 5,     4, 6,
					21, 6,   21, 7,   20, 8,   19, 9,   19, 10,   18, 11,   21, 12,
				};
				
				for (int i=0, j=1; j<days.Length; i+=2, j+=2) {
					UTCDate udt_n = udt.SetDate(days[i], days[j]);
					
					// first half
					Color color = udt_n.IsDST ? colors.YearFstHalfDst : colors.YearFstHalfStd;
					// second half
					if (udt_n >= udt.SetDate(7, 6)) {
						color = udt_n.IsDST ? colors.YearSndHalfDst : colors.YearSndHalfStd;
					}
					
					grapher.PlotMilestoneDay(g, color, pos, udt_n);
				}

				// plot analemma curves
				for (int i = 0; i < 24; i++) {
					grapher.PlotAnalemma(g, 
					                     colors.YearFstHalfStd, colors.YearFstHalfDst, 
					                     colors.YearSndHalfStd, colors.YearSndHalfDst,
					                     pos, udt.SetHour(i));
				}

				// print milestone day labels
				grapher.PrintMilestoneDayLabels(g);
				
				// print analemma labels
				for (int i = 0; i < 24; i++) {
					grapher.PrintAnalemmaLabel(g, colors.GraphFg, 
					                           pos, udt.SetHour(i));
				}
			}
			
			return this.bitmap;
		}
		
		public Bitmap RenderCurrentDayCloned(Position pos, UTCDate dt)
		{
			return RenderCurrentDay((Bitmap) this.bitmap.Clone(), pos, dt);
		}
		
		public Bitmap RenderCurrentDay(Bitmap bitmap,
		                               Position pos, UTCDate udt)
		{
			using (Graphics g = Graphics.FromImage(bitmap)) {
				grapher.PlotDay(g, colors.CurrentDay, pos, udt);
				grapher.PlotSun(g, colors.CurrentDay, dimensions, pos, udt);
			}
			return bitmap;
		}
		
		public Bitmap RenderCaption(CaptionInfo ci)
		{
			using (Graphics g = Graphics.FromImage(bitmap)) {
				Caption caption = new Caption(0, dimensions+titleheight,
				                              dimensions, dimensions+titleheight+captionheight);
				grapher.PrintCaption(g, caption, ci);
				grapher.PrintTitle(g, 0, 0, dimensions, titleheight);
			}
			return bitmap;
		}
	
		public void SaveBitmap(Bitmap bitmap, string path)
		{
			bitmap.Save(path, ImageFormat.Png);	
		}
	}
}
