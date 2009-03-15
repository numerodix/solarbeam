// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

namespace LibSolar.Graphing
{
	/**
	 * Represents the graph drawn on top of a grid.
	 */
	struct Graph
	{
		private Point origin;
		private int delta;
		
		private int radius;
		private int diameter;
		private int bound_a;
		private int bound_b;
		private int bound_x;
		private int bound_y;
		private int dx;
		private int dy;
	
		public Graph(Point origin, int deci)
		{
			this.origin = origin;
			this.delta = deci;
			
			this.radius = this.delta * 9;
			this.diameter = this.radius * 2;
			
			this.bound_a = this.origin.X - this.radius;
			this.bound_b = this.origin.Y - this.radius;
			this.bound_x = this.origin.X + this.radius;
			this.bound_y = this.origin.Y + this.radius;
			
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
				
		public Point Origin
		{ get { return this.origin; } }

		public int Delta
		{ get { return this.delta; } }

		public int Radius
		{ get { return this.radius; } }

		public int Diameter
		{ get { return this.diameter; } }
	}	
}
