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
		private Point FindMapPoint(Position pos)
		{
			Degree latd = pos.LatitudeDegree;
			Degree lond = pos.LongitudeDegree;
			
			double lon_val = Position.CollapsePositionUnits(lond.Direction,
			                                                lond.Deg, lond.Min, lond.Sec);
			double lat_val = Position.CollapsePositionUnits(latd.Direction,
			                                                latd.Deg, latd.Min, latd.Sec);
			
			double lon = (lon_val / 3600); 
			double lat = -1 * lat_val / 3600;
			
			double lon_pivot = 10.9;
			
			if (lon < (-180 + lon_pivot)) {
				lon = (180.0 - lon_pivot) + (180.0 - Math.Abs(lon));
			} else {
				lon -= lon_pivot;
			}
			
			int px = (int) (map.Origin.X + ((lon / 360.0) * map.Dx));
			int py = (int) (map.Origin.Y + ((lat / 180.0) * map.Dy));
			
			return new Point(px, py);
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
