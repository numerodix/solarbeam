// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide layout.
	 */
	partial class Widgets
	{
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