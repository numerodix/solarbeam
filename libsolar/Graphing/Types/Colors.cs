// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

namespace LibSolar.Graphing
{
	/**
	 * Package all colors used in graphing.
	 */
	public class Colors
	{
		private Color grid_bg = Color.White;
		private Color grid_fg = Color.Gray;
		private Color graph_fg = Color.Black;
		private Color year_fst_half = Color.Blue;
		private Color year_snd_half = Color.Green;
		private Color year_fst_half_std = Color.DarkBlue;
		private Color year_fst_half_dst = Color.SteelBlue;
		private Color year_snd_half_std = Color.DarkGreen;
		private Color year_snd_half_dst = Color.LimeGreen;
		private Color current_day = Color.FromArgb(255,127,0);
		
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
		
		public Color YearFstHalfStd
		{ get { return this.year_fst_half_std; } }
		
		public Color YearFstHalfDst
		{ get { return this.year_fst_half_dst; } }
		
		public Color YearSndHalfStd
		{ get { return this.year_snd_half_std; } }
		
		public Color YearSndHalfDst
		{ get { return this.year_snd_half_dst; } }
		
		public Color CurrentDay
		{ get { return this.current_day; } }
	}
}
