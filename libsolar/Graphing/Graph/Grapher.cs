// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

namespace LibSolar.Graphing
{	
	partial class Grapher
	{
		private Grid grid;
		private Graph graph;
		private Colors colors;
		private string font_face;
		
		
		public Grapher(int bound_a, int bound_b, int bound_x, int bound_y,
		               Colors colors, string font_face)
		{
			this.grid = new Grid(bound_a, bound_b, bound_x, bound_y);
			this.graph = new Graph(grid.Origin, grid.Deci);
			this.colors = colors;
			this.font_face = font_face;
		}
	}
}