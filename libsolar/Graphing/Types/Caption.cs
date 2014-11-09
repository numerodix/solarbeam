// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Graphing
{
	/**
	 * Represents the caption drawn below a graph.
	 */
	struct Caption
	{
		private int bound_a;
		private int bound_b;
		private int bound_x;
		private int bound_y;
		
		private int dx;
		private int dy;
	
		public Caption(int bound_a, int bound_b, int bound_x, int bound_y)
		{
			this.bound_a = bound_a;
			this.bound_b = bound_b;
			this.bound_x = bound_x;
			this.bound_y = bound_y;
			
			this.dx = this.bound_x - this.bound_a;
			this.dy = this.bound_y - this.bound_b;
		}
		
		public int A
		{ get { return this.bound_a; } }
		
		public int B
		{ get { return this.bound_b; } }
		
		public int X
		{ get { return this.bound_x; } }
		
		public int Y
		{ get { return this.bound_y; } }
		
		public int Dx
		{ get { return this.dx; } }
		
		public int Dy
		{ get { return this.dy; } }
	}	
}
