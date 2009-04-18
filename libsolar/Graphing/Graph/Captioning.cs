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
				
				int x1 = caption.Dx / 20;
				
				List<string> stack = new List<string>();
				stack.Add(ci.Location);
				stack.Add(FormatPosition(ci.Position));
				stack.Add(FormatTimezone(ci.Timezone, ci.DST));
				PrintLocation(g, brush, caption, caption.A+x1, caption.B, stack);
			}
		}
		
		private void PaintCaptionBackground(Graphics g, Caption caption)
		{
			using (SolidBrush br = new SolidBrush(colors.GridBg)) {
				g.FillRectangle(br, caption.A, caption.B, caption.Dx, caption.Dy);
			}
		}
		
		private void PrintLocation(Graphics g, SolidBrush br, Caption caption,
		                           int a, int b, List<string> stack)
		{
			float font_size = GetLabelFontSize();
			using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel)) {
				int height = font.Height;
				for (int i=0; i<stack.Count; i++) {
					PrintBoundedString(g, font, br, stack[i],
					                   a, b + (height*i),
					                   Placement.TOP_LEFT);
				}
			}
		}
	}	
}
