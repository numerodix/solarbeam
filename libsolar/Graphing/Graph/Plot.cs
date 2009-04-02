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
	 * Plot a graph on a Graph.
	 */
	partial class Diagram
	{
		private int dayseconds = 24*3600;
		private int yeardays = 365;
		
		private Dictionary<UTCDate,KeyValuePair<Color,Point?>> intersects = 
			new Dictionary<UTCDate,KeyValuePair<Color,Point?>>();
		
		
		public void PlotMilestoneDay(Graphics g, Color col, Position pos, 
		                             double tz,
		                             int year, int month, int day)
		{
			PlotDayInternal(true, g, col, pos, tz, year, month, day);
		}
		
		public void PlotDay(Graphics g, Color col, Position pos, double tz,
		                             int year, int month, int day)
		{
			PlotDayInternal(false, g, col, pos, tz, year, month, day);
		}
		
		private void PlotDayInternal(bool track_intersect,
		                             Graphics g, Color col, Position pos, double tz,
		                             int year, int month, int day)
		{
			UTCDate dt = new UTCDate(tz,
					year, month, day,
					0, 0, 1);
			
			// record points to place labels
			UTCDate dt_midyear = new UTCDate(tz, year, 6, 5, 0, 0, 1);
			double el_min = 90;
			Point? pt = null; 

			double step = GetResolutionStep(dayseconds);
			for (double cursor = 0; cursor < dayseconds; cursor+=step)
			{
				UTCDate dt_new = dt.AddSeconds(cursor);
 
				SolarPosition sp = Orbit.CalcSolarPosition(pos, dt_new);
				Point? point = FindPoint(sp.Azimuth, sp.Elevation);
				if (point != null) {
					using (SolidBrush br = new SolidBrush(col)) {
						PlotPoint(g, br, point.Value);
					}
					
					// collect points to place labels
					if (track_intersect) {
						double el = Math.Abs(sp.Elevation);
						
						// first half of the year
						if (dt.CompareTo(dt_midyear) == -1) {
							// make sure we keep to the left of the graph
							if (point.Value.X < graph.Origin.X) {
								if (el_min > el) {
									el_min = el;
									pt = point;
								}
							}
						}
						
						// second half of the year
						if (dt.CompareTo(dt_midyear) >= 0) {
							// make sure we keep to the right of the graph
							if (point.Value.X >= graph.Origin.X) {
								if (el_min > el) {
									el_min = el;
									pt = point;
								}
							}
						}
					}
				}
			}
			
			if (track_intersect) {
				intersects.Add(dt, new KeyValuePair<Color,Point?>(col, pt));
			}
		}
		
		public void PrintMilestoneDayLabels(Graphics g)
		{
			float font_size = GetLabelFontSize();
			using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel)) {
				foreach (KeyValuePair<UTCDate,KeyValuePair<Color,Point?>> pair 
				         in intersects) {
					KeyValuePair<Color,Point?> inner = pair.Value;
					Color color = inner.Key;
					Point? point = inner.Value;
					
					if (point != null) {
						string dt_s = FormatDate(pair.Key);

						Placement place = Placement.RIGHT;
						if (point.Value.X > graph.Origin.X) {
							place = Placement.LEFT;
						}
						
						PrintBoundedString(g, font, color, dt_s,
						                   point.Value.X, point.Value.Y,
						                   place);
					}
				}
			}
		}
		
		public void PlotAnalemma(Graphics g, Color col_fst, Color col_snd,
		                         Position pos, double tz, int year, int hour)
		{
			UTCDate dt = new UTCDate(tz,
					year, 1, 1,
					hour, 0, 0);
			
			using (SolidBrush br_fst = new SolidBrush(col_fst))
			using (SolidBrush br_snd = new SolidBrush(col_snd)) {
				double step = Math.Max(1, GetResolutionStep(yeardays));
				for (double cursor = 0; cursor < yeardays; cursor+=step)
				{
					UTCDate dt_new = dt.AddDays(cursor);
					
					SolarPosition sp = Orbit.CalcSolarPosition(pos, dt_new);
					Brush br = (cursor < yeardays / 2) ? br_fst : br_snd;
					PlotPoint(g, br, sp.Azimuth, sp.Elevation);
				}
			}
		}
		
		public void PrintAnalemmaLabel(Graphics g, Color color,
		                               Position pos, double tz, int year, int hour)
		{
			// northern hemisphere -> june longest, dec shortest
			int longest = 6;
			int shortest = 12;
			// southern hemisphere -> dec longest, june shortest
			if (pos.LatitudeDegree.Direction == PositionDirection.South) {
				longest = 12;
				shortest = 6;
			}
			
			UTCDate udt_inner = new UTCDate(tz,
			                                year, longest, 21,
			                                hour, 0, 0);
			UTCDate udt_outer = new UTCDate(tz,
			                                year, shortest, 21,
			                                hour, 0, 0);
			
			int hour_inner = udt_inner.ExtractLocal().Hour;
			int hour_outer = udt_outer.ExtractUTC().Hour;
			
			float font_size = GetLabelFontSize();
			string hour_inner_s = hour_inner == 0 ? "24" : hour_inner.ToString();
			string hour_outer_s = hour_outer == 0 ? "24" : hour_outer.ToString();
			
			KeyValuePair<Point?,double?> pair_max =
				FindPointSlopeAtHour(pos, udt_inner);
			if (pair_max.Key != null) {
				Placement place = SlopeToPlacement(pair_max.Value.Value, pos);
				using (SolidBrush br_txt = new SolidBrush(color))
				using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel)) {
					PrintBoundedString(g, font, br_txt, hour_inner_s,
					                   pair_max.Key.Value.X, pair_max.Key.Value.Y,
					                   place);
				}
			}
			
			KeyValuePair<Point?,double?> pair_min = 
				FindPointSlopeAtHour(pos, udt_outer);
			if (pair_min.Key != null) {
				Placement place = 
					SlopeToPlacement( (pair_min.Value.Value + 180.0) % 360.0, pos);
				using (SolidBrush br_txt = new SolidBrush(color))
				using (Font font = new Font(font_face, font_size, GraphicsUnit.Pixel)) {
					PrintBoundedString(g, font, br_txt, hour_outer_s,
					                   pair_min.Key.Value.X, pair_min.Key.Value.Y,
					                   place);
				}
			}		
		}
		
		public void PlotSun(Graphics g, Color col, int dim, Position pos, UTCDate dt)
		{
			dt = dt.AsStandard();
			SolarPosition sp = Orbit.CalcSolarPosition(pos, dt);
			Point? point = FindPoint(sp.Azimuth, sp.Elevation);
			if (point != null)
			{
				int radius = Math.Max(3, dim / 50 / 2);
				int a = point.Value.X - radius;
				int b = point.Value.Y - radius;
				using (SolidBrush br = new SolidBrush(col)) {
					g.FillEllipse(br, a, b, 2*radius, 2*radius);
				}
			}			
		}
		
		public void PlotPoint(Graphics g, Brush br, double azimuth, double elevation)
		{
			Point? point = FindPoint(azimuth, elevation);
			if (point != null)
			{
				PlotPoint(g, br, point.Value);
			}
		}
		
		public void PlotPoint(Graphics g, Brush br, Point point)
		{
			int thickness = GetLineThickness();
			int x = point.X - thickness / 2;
			int y = point.Y - thickness / 2;
			g.FillRectangle(br, x, y, thickness, thickness);
		}
	}
}		
