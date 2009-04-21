// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;

using LibSolar.Types;

namespace LibSolar.Mapping
{	
	partial class Mapper
	{
		private const double lon_pivot = 10.9;
		
		private Point FindMapPoint(Position pos)
		{
			Degree latd = pos.LatitudeDegree;
			Degree lond = pos.LongitudeDegree;
			
			double lon_val = Position.CollapsePositionUnits(lond.Direction,
			                                                lond.Deg, lond.Min, lond.Sec);
			double lat_val = Position.CollapsePositionUnits(latd.Direction,
			                                                latd.Deg, latd.Min, latd.Sec);
			
			// handle pivot
			double lon = (lon_val / 3600); 
			double lat = -1 * lat_val / 3600;
						
			if (lon < (-180 + lon_pivot)) {
				lon = (180.0 - lon_pivot) + (180.0 - Math.Abs(lon));
			} else {
				lon -= lon_pivot;
			}
			
			int px = (int) (map.Origin.X + ((lon / 360.0) * map.Dx));
			int py = (int) (map.Origin.Y + ((lat / 180.0) * map.Dy));
			
			return new Point(px, py);
		}
		
		public Position FindPosition(int X_in, int Y_in)
		{
			// displace cursor to lower right
			double X = X_in - 13;
			double Y = Y_in - 7;
			
			// the canvas
			double a = map.A;
			double b = map.B;
			double x = map.X;
			double y = map.Y;
			
			// bound point within canvas
			X = Math.Max(a, Math.Min(x, X));
			Y = Math.Max(b, Math.Min(y, Y));
			
			double w = ((X - a) / (x - a)) * 360;
			double h = ((Y - b) / (y - b)) * 180;
			
			// handle pivot
			w += lon_pivot;
			if (w > 360) {
				w = w - 360;
			}
			
			if (h <= 90) {
				h = 90 - h;
			} else {
				h = -(h - 90);
			}
			
			if (w <= 180) {
				w = -(180 - w);
			} else {
				w = w - 180;
			}
			
			int ladeg = 0;
			int lamin = 0;
			int lasec = 0;
			Expand(h * 3600.0, out ladeg, out lamin, out lasec);
			PositionDirection ladir = h > 0 ? Position.LATITUDE_POS : Position.LATITUDE_NEG;
			
			int lodeg = 0;
			int lomin = 0;
			int losec = 0;
			Expand(w * 3600.0, out lodeg, out lomin, out losec);
			PositionDirection lodir = w > 0 ? Position.LONGITUDE_POS : Position.LONGITUDE_NEG;
			
			return new Position(ladir, ladeg, lamin, lasec,
			                    lodir, lodeg, lomin, losec);
		}
		
		private void Expand(double degs, out int deg, out int min, out int sec)
		{
			degs = Math.Abs(degs);

			deg = (int) (degs / 3600.0);
			degs = degs - deg * 3600.0;
			min = (int) (degs / 60.0);
			degs = degs - min * 60.0;
			sec = (int) degs;
		}
		
		private bool WithinYBound(int b, int db, int delta)
		{
			return WithinBound(b, db, delta, map.B, map.Y);
		}
		
		private bool WithinXBound(int a, int da, int delta)
		{
			return WithinBound(a, da, delta, map.A, map.X);
		}
		
		private bool WithinBound(int z, int dz, int delta, int lower, int upper)
		{
			bool within = false;
			if (dz >= 0) {
				within = (z + dz + delta < upper);
			} else {
				within = (z + dz - delta > lower);
			}
			return within;
		}
	}
}		
