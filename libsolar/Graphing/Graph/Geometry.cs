// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;

using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace LibSolar.Graphing
{
	/**
	 * Geometry related primitives to aid in plotting.
	 */
	partial class Grapher
	{
		private KeyValuePair<Point?,double?> FindPointSlopeAtHour(Position pos,
		                                                          UTCDate dt)
		{
			UTCDate[] dts = new UTCDate[] {dt.AddHours(-1), dt, dt.AddHours(1)};
			List<Point?> pts = FindPoints(pos, dts);
			
			Point? prev = pts[0];
			Point? point = pts[1];
			Point? next = pts[2];
			double? slope = null;
			
			Point? a = null;
			Point? b = null;
			if (point != null) {
				if ((prev != null) && (next != null)) {
					a = new Nullable<Point>(prev.Value);
					b = new Nullable<Point>(next.Value);
				} else if ((prev == null) && (next != null)) {
					a = new Nullable<Point>(point.Value);
					b = new Nullable<Point>(next.Value);
				} else if ((prev != null) && (next == null)) {
					a = new Nullable<Point>(prev.Value);
					b = new Nullable<Point>(point.Value);
				}
			}
			
			if ((a != null) && (b != null)) {
				slope = ComputeSlope(a.Value, b.Value);
			}
			return new KeyValuePair<Point?,double?>(point, slope);
		}
		
		private double ComputeSlope(Point a, Point b)
		{
			double dx = Math.Abs(b.X - a.X); // find pos angle
			double dy = b.Y - a.Y;
			double hypotenuse = Math.Sqrt(dx*dx + dy*dy);
			
			double ang_rad = Math.Asin(dx / hypotenuse);
			double ang = Coordinate.RadToDeg(ang_rad);
			
			if ((a.X <= b.X) && (a.Y <= b.Y)) {
				ang = 180 - ang;
			} else if ((a.X >= b.X) && (a.Y <= b.Y)) {
				ang += 180;
			} else if ((a.X >= b.X) && (a.Y >= b.Y)) {
				ang = 360 - ang;
			}
			
			return ang;
		}

		private Placement SlopeToPlacement(double slope, Position pos)
		{
			if (pos.LatitudeDegree.Direction == PositionDirection.South) {
				slope = (slope + 180.0) % 360.0;
			}
			
			Placement place = Placement.LEFT;
			if ((slope > 22.5) && (slope < 67.5)) {
				place = Placement.TOP_LEFT;
			} else if ((slope > 67.5) && (slope < 112.5)) {
				place = Placement.TOP;
			} else if ((slope > 112.5) && (slope < 157.5)) {
				place = Placement.TOP_RIGHT;
			} else if ((slope > 157.5) && (slope < 202.5)) {
				place = Placement.RIGHT;
			} else if ((slope > 202.5) && (slope < 247.5)) {
				place = Placement.BOTTOM_RIGHT;
			} else if ((slope > 247.5) && (slope < 292.5)) {
				place = Placement.BOTTOM;
			} else if ((slope > 292.5) && (slope < 337.5)) {
				place = Placement.BOTTOM_LEFT;
			} else if ((slope > 337.5) || (slope < 22.5)) {
				place = Placement.LEFT;
			}
			return place;
		}
		
		private List<Point?> FindPoints(Position pos, UTCDate[] dts)
		{
			List<Point?> pts = new List<Nullable<Point>>();
			foreach (UTCDate dt in dts)
			{
				SolarPosition sp = Orbit.CalcSolarPosition(pos, dt);
				Point? pt = FindPoint(sp.Azimuth, sp.Elevation);
				pts.Add(pt);
			}
			return pts;
		}
	}	
}
