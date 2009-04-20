// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

namespace LibSolar.Mapping
{
	/**
	 * Package all colors used in mapping.
	 */
	public class Colors
	{
		private Color map_bg = Color.White;
		private Color map_fg = Color.Black;
		
		public Color MapBg
		{ get { return this.map_bg; } }

		public Color MapFg
		{ get { return this.map_fg; } }
	}
}
