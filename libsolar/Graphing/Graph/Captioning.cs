// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;

using LibSolar.SolarOrbit;

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
					stack.Add(FormatPosition(ci.Position));
					stack.Add(FormatTimezone(ci.Timezone, ci.Timezone+ci.DST));
					PrintVertically(g, brush, font, caption, caption.A+dxx, caption.B, stack);
					
					stack = new List<string>();
					stack.Add(string.Empty);
					stack.Add(FormatCaptionDate(ci.Date));
					stack.Add(FormatCaptionTime(ci.Date, ci));
					PrintVertically(g, brush, font, caption, (caption.Dx/2)+dxx*3, caption.B, stack);
					
					int h_ang = 3*height;
					
					stack = new List<string>();
					stack.Add(FormatTime(ci.Dawn));
					stack.Add(FormatTime(ci.Sunrise));
					stack.Add(FormatTime(ci.Noon));
					stack.Add(FormatTime(ci.Sunset));
					stack.Add(FormatTime(ci.Dusk));
					PrintHorizontally(g, brush, font, caption, caption.B+h_ang, stack);
					
					int h_tm = 5*height;
					
					stack = new List<string>();
					stack.Add(FormatTime(ci.Dawn));
					stack.Add(FormatTime(ci.Sunrise));
					stack.Add(FormatTime(ci.Noon));
					stack.Add(FormatTime(ci.Sunset));
					stack.Add(FormatTime(ci.Dusk));
					PrintHorizontally(g, brush, font, caption, caption.B+h_tm, stack);
					
					stack = new List<string>();
					stack.Add("dawn");
					stack.Add("sunrise");
					stack.Add("solar noon");
					stack.Add("sunset");
					stack.Add("dusk");
					PrintHorizontally(g, brush, font, caption, caption.B+h_tm+1*height, stack);
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
		
		private int GetInc(Caption caption)
		{
			return caption.Dx / 20;
		}
	}	
}
