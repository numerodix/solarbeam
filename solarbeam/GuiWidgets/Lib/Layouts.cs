// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide layout.
	 */
	partial class Widgets
	{
		public static TableLayoutPanel GetStacked(Control[] controls, string[] widths)
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = 1;
			layout.RowCount = controls.Length;
			
			foreach (string width in widths) {
				float val = 0F;
				SizeType tp = SizeType.Absolute;
				if (width.EndsWith("%")) {
					val = (float) Convert.ToDouble(width.Remove(width.Length - 1));
					tp = SizeType.Percent;
				} else {
					val = (float) Convert.ToDouble(width);
				}
				layout.RowStyles.Add(new RowStyle(tp, val));
			}
	
			foreach (Control c in controls) {
				c.Dock = DockStyle.Fill;
//				c.Anchor = AnchorStyles.Bottom | AnchorStyles.Top;
				layout.Controls.Add(c);
			}
			return layout;
		}
		
		public static TableLayoutPanel GetLaidOut(Control[] controls, string[] widths)
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = controls.Length;
			layout.RowCount = 1;
			
			foreach (string width in widths) {
				float val = 0F;
				SizeType tp = SizeType.Absolute;
				if (width.EndsWith("%")) {
					val = (float) Convert.ToDouble(width.Remove(width.Length - 1));
					tp = SizeType.Percent;
				} else {
					val = (float) Convert.ToDouble(width);
				}
				layout.ColumnStyles.Add(new ColumnStyle(tp, val));
			}
	
			foreach (Control c in controls) {
				c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				layout.Controls.Add(c);
			}
			return layout;
		}
		
		public static TableLayoutPanel GetLaidOut(Control[] controls, float[] widths)
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = controls.Length;
			layout.RowCount = 1;
			
			foreach (float width in widths)
			{
				layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));
			}
	
			foreach (Control c in controls)
			{
				c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				layout.Controls.Add(c);
			}
			return layout;
		}
				
		public static TableLayoutPanel GetTableLayoutPanel(int rows, int cols, 
		                                                   int margin, int padding)
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = cols;
			layout.RowCount = rows;
			layout.Padding = new Padding(padding);
			layout.Margin = new Padding(margin);
			return layout;
		}
	}
}
