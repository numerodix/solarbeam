// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

using LibSolar.SolarOrbit;

namespace LibSolar.Graphing
{	
	/**
	 * Print caption on a bitmap.
	 */
	partial class Diagram
	{
		public void PrintCaption(Graphics g, Caption caption)
		{
			using (SolidBrush brush = new SolidBrush(colors.GraphFg)) {
				PaintCaptionBackground(g, caption);
				PrintLocation(g, brush, caption, "Oslo (NOR)");
			}
		}
		
		private void PaintCaptionBackground(Graphics g, Caption caption)
		{
			using (SolidBrush br = new SolidBrush(colors.GridBg)) {
				g.FillRectangle(br, caption.A, caption.B, caption.Dx, caption.Dy);
			}
		}
		
		private void PrintLocation(Graphics g, SolidBrush br, Caption caption,
		                           string location)
		{
			using (Font font = new Font(font_face, 12, GraphicsUnit.Pixel)) {
				PrintBoundedString(g, font, br, location,
				                   caption.A, caption.B, Placement.TOP_LEFT);
			}
		}
	}	
}
