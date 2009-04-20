// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;

using LibSolar.Formatting;
using LibSolar.Mapping;
using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace LibSolar.Graphing
{	
	/**
	 * Print caption on a bitmap.
	 */
	partial class Diagram
	{
		public void PrintCaption(Graphics g, Caption caption, CaptionInfo ci)
		{
			using (SolidBrush brush = new SolidBrush(colors.GraphFg)) {
				PaintCaptionBackground(g, caption);
				
				int dxx = GetInc(caption);
				
				float font_size = GetLabelFontSize()+1;
				using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel)) {
					int height = font.Height;
					
					List<string> stack = new List<string>();
					stack.Add(ci.Location);
					stack.Add(Formatter.FormatCaptionPosition(ci.Position));
					stack.Add(Formatter.FormatCaptionTimezone(ci.Timezone, ci.Timezone+ci.DST));
					PrintVertically(g, brush, font, caption, caption.A+dxx, caption.B, stack);
					
					stack = new List<string>();
					stack.Add(string.Empty);
					stack.Add(Formatter.FormatCaptionDate(ci.Date));
					stack.Add(Formatter.FormatCaptionTime(ci.Date, ci));
					PrintVertically(g, brush, font, caption, caption.A+dxx, caption.B+3*height, stack);
					
					int h_ang = (int) (7.5 * (double) height);
					
					stack = new List<string>();
					stack.Add(string.Format("sun elevation: {0}",
					                        Formatter.FormatAngle(ci.Elevation)));
					stack.Add(string.Format("sun azimuth: {0}",
					                        Formatter.FormatAngle(ci.Azimuth)));
					PrintHorizontally(g, brush, font, caption, caption.B+h_ang, stack);
					
					int h_tm = (int) (10 * (double) height);
					
					stack = new List<string>();
					stack.Add(Formatter.FormatTime(ci.Dawn));
					stack.Add(Formatter.FormatTime(ci.Sunrise));
					stack.Add(Formatter.FormatTime(ci.Noon));
					stack.Add(Formatter.FormatTime(ci.Sunset));
					stack.Add(Formatter.FormatTime(ci.Dusk));
					PrintHorizontally(g, brush, font, caption, caption.B+h_tm, stack);
					
					stack = new List<string>();
					stack.Add("dawn");
					stack.Add("sunrise");
					stack.Add("solar noon");
					stack.Add("sunset");
					stack.Add("dusk");
					PrintHorizontally(g, brush, font, caption, caption.B+h_tm+1*height, stack);
					
					RenderMap(g, ci.Position, caption);
				}
			}
		}
		
		private void PaintCaptionBackground(Graphics g, Caption caption)
		{
			using (SolidBrush br = new SolidBrush(colors.GridBg)) {
				g.FillRectangle(br, caption.A, caption.B, caption.Dx, caption.Dy);
			}
		}
		
		private void PrintVertically(Graphics g, SolidBrush br, Font font, 
		                             Caption caption,
		                             int a, int b, List<string> stack)
		{
			int height = font.Height;
			for (int i=0; i<stack.Count; i++) {
				PrintBoundedString(g, font, br, stack[i],
				                   a, b + (height*i),
				                   Placement.TOP_LEFT);
			}
		}
		
		private void PrintHorizontally(Graphics g, SolidBrush br, Font font,
		                               Caption caption,
		                               int b, List<string> stack)
		{
			int dxx = GetInc(caption);
			int height = font.Height;
			int space = (caption.Dx-dxx) - (caption.A+dxx);
			int space_1 = space / stack.Count;
			for (int i=0; i<stack.Count; i++) {
				int inset = (int) (space_1 * ((double) i + 0.5));
				PrintBoundedString(g, font, br, stack[i],
				                   caption.A + dxx + inset,
				                   b+height/2,
				                   Placement.CENTER);
			}
		}
		
		private void RenderMap(Graphics g, Position pos, Caption caption)
		{
			int w = caption.Dx / 3;
			int h = w / 2;
			MapBitmap mapbitmap = new MapBitmap(w, h,
			                                    new LibSolar.Mapping.Colors(),
			                                    font_face);
			Bitmap bitmap = mapbitmap.RenderBaseImage();
			bitmap = mapbitmap.RenderCurrentPosition(bitmap, pos);
			
			int dxx = GetInc(caption);
			int a = (caption.A + caption.Dx) - w - dxx;
			int b = caption.B;
			
			g.DrawImage(bitmap, a, b, w, h);
			
			using (SolidBrush br = new SolidBrush(colors.GraphFg))
			using (Pen pen = new Pen(br)) {
				g.DrawRectangle(pen, a, b, w ,h);
			}
		}
		
		private int GetInc(Caption caption)
		{
			return caption.Dx / 20;
		}
	}	
}
