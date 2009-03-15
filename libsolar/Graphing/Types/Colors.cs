// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

namespace LibSolar.Graphing
{
	/**
	 * Package all colors used in graphing.
	 */
	public struct Colors
	{
		private Color grid_bg;
		private Color grid_fg;
		private Color year_fst_half;
		private Color year_snd_half;
		private Color current_day;
		private Color graph_fg;

		public Colors(Color grid_bg, Color grid_fg, Color graph_fg,
				Color year_fst_half, Color year_snd_half,
				Color current_day)
		{
			this.grid_bg = grid_bg;
			this.grid_fg = grid_fg;
			this.graph_fg = graph_fg;
			this.year_fst_half = year_fst_half;
			this.year_snd_half = year_snd_half;
			this.current_day = current_day;
		}
		
		public Color GridBg
		{ get { return this.grid_bg; } }
		
		public Color GridFg
		{ get { return this.grid_fg; } }
		
		public Color GraphFg
		{ get { return this.graph_fg; } }
		
		public Color YearFstHalf
		{ get { return this.year_fst_half; } }
		
		public Color YearSndHalf
		{ get { return this.year_snd_half; } }
		
		public Color CurrentDay
		{ get { return this.current_day; } }
	}
}
