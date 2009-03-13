// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide initialization of common gui components.
	 */
	sealed class GuiCommon
	{
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