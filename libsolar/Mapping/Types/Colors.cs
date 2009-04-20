// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

namespace LibSolar.Mapping
{
	/**
	 * Package all colors used in graphing.
	 */
	public class Colors
	{
		private Color map_bg = Color.White;
		private Color grid_fg = Color.Gray;
		private Color graph_fg = Color.Black;
		private Color year_fst_half_std = Color.DarkBlue;
		private Color year_fst_half_dst = Color.SteelBlue;
		private Color year_snd_half_std = Color.Green;
		private Color year_snd_half_dst = Color.LimeGreen;
		private Color current_day = Color.Red;
		
		public Color MapBg
		{ get { return this.map_bg; } }
		
		public Color GridFg
		{ get { return this.grid_fg; } }
		
		public Color GraphFg
		{ get { return this.graph_fg; } }
		
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
