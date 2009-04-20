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
	}
}		
