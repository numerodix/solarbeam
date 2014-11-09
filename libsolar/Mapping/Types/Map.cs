// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

namespace LibSolar.Mapping
{
	/**
	 * Represents a map drawn on a bitmap with set bounds within the bitmap.
	 */
	public struct Map
	{
		private int bound_a;
		private int bound_b;
		private int bound_x;
		private int bound_y;
		
		private int dx;
		private int dy;
		
		private Point origin;
		
		public Map(int bound_a, int bound_b, int bound_x, int bound_y)
		{
			this.bound_a = bound_a;
			this.bound_b = bound_b;
			this.bound_x = bound_x;
			this.bound_y = bound_y;
			
			this.dx = this.bound_x - this.bound_a;
			this.dy = this.bound_y - this.bound_b;
			
			if (dx < 1) {
				throw new ArgumentException(string.Format(
					"Not met: bound_x>bound_a, bound_a: {0}, bound_x: {1}",
					this.bound_a, this.bound_x));
			}
			if (dy < 1) {
				throw new ArgumentException(string.Format(
					"Not met: bound_y>bound_b, bound_b: {0}, bound_y: {1}",
					this.bound_b, this.bound_y));
			}
			
			int center_x = this.bound_a + (this.dx / 2);
			int center_y = this.bound_b + (this.dy / 2);
			
			this.origin = new Point(center_x, center_y);
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
	}
}
