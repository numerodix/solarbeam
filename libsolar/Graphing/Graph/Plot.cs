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
		                             UTCDate udt)
		{
			PlotDayInternal(true, g, col, pos, udt);
		}
		
		public void PlotDay(Graphics g, Color col, Position pos, UTCDate udt)
		{
			PlotDayInternal(false, g, col, pos, udt);
		}
		
		private void PlotDayInternal(bool track_intersect,
		                             Graphics g, Color col, Position pos,
		                             UTCDate udt)
		{
			UTCDate udt_lower = new UTCDate(udt.Timezone, udt.DST,
			                         udt.Year, udt.Month, udt.Day,
			                         0, 0, 1);
			
			// record points to place labels
			UTCDate dt_midyear = new UTCDate(udt.Timezone, udt.DST,
			                                 udt.Year, 6, 5, 0, 0, 1);
			double el_min = 90;
			Point? pt = null; 

			double step = GetResolutionStep(dayseconds);
			for (double cursor = 0; cursor < dayseconds; cursor+=step)
			{
				UTCDate udt_new = udt_lower.AddSeconds(cursor);
 
				SolarPosition sp = Orbit.CalcSolarPosition(pos, udt_new);
				Point? point = FindPoint(sp.Azimuth, sp.Elevation);
				if (point != null) {
					using (SolidBrush br = new SolidBrush(col)) {
						PlotPoint(g, br, point.Value);
					}
					
					// collect points to place labels
					if (track_intersect) {
						double el = Math.Abs(sp.Elevation);
						
						// first half of the year
						if (udt_lower.CompareTo(dt_midyear) == -1) {
							// make sure we keep to the left of the graph
							if (point.Value.X < graph.Origin.X) {
								if (el_min > el) {
									el_min = el;
									pt = point;
								}
							}
						}
						
						// second half of the year
						if (udt_lower.CompareTo(dt_midyear) >= 0) {
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
				intersects.Add(udt_lower, new KeyValuePair<Color,Point?>(col, pt));
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
		
		public void PlotAnalemma(Graphics g, 
		                         Color col_fst_std, Color col_fst_dst, 
		                         Color col_snd_std, Color col_snd_dst,
		                         Position pos, UTCDate udt)
		{
			UTCDate dt = new UTCDate(udt.Timezone, udt.DST,
			                         udt.Year, 1, 1,
			                         udt.Hour, 0, 0);
			
			using (SolidBrush br_fst_std = new SolidBrush(col_fst_std))
			using (SolidBrush br_fst_dst = new SolidBrush(col_fst_dst))
			using (SolidBrush br_snd_std = new SolidBrush(col_snd_std))
			using (SolidBrush br_snd_dst = new SolidBrush(col_snd_dst)) {
				double step = Math.Max(1, GetResolutionStep(yeardays));
				for (double cursor = 0; cursor < yeardays; cursor+=step)
				{
					UTCDate dt_new = dt.AddDays(cursor);
					
					SolarPosition sp = Orbit.CalcSolarPosition(pos, dt_new);
					
					// first half
					Brush br = dt_new.IsDST ? br_fst_dst : br_fst_std;
					// second half
					if (cursor >= yeardays / 2) {
						br = dt_new.IsDST ? br_snd_dst : br_snd_std;
					}
					
					PlotPoint(g, br, sp.Azimuth, sp.Elevation);
				}
			}
		}
		
		public void PrintAnalemmaLabel(Graphics g, Color color,
		                               Position pos, UTCDate udt)
		{
			// northern hemisphere -> june longest, dec shortest
			int longest = 6;
			int shortest = 12;
			// southern hemisphere -> dec longest, june shortest
			if (pos.LatitudeDegree.Direction == PositionDirection.South) {
				longest = 12;
				shortest = 6;
			}
			
			UTCDate udt_inner = new UTCDate(udt.Timezone, udt.DST,
			                                udt.Year, longest, 21,
			                                udt.Hour, 0, 0);
			UTCDate udt_outer = new UTCDate(udt.Timezone, udt.DST,
			                                udt.Year, shortest, 21,
			                                udt.Hour, 0, 0);
			
			int hour_inner = udt_inner.ExtractLocal().Hour;
			int hour_outer = udt_outer.ExtractLocal().Hour;
			
			// we have DST
			if (udt_inner.IsDST || udt_outer.IsDST) {
				
			}
			
			float font_size = GetLabelFontSize();
			string hour_inner_s = FormatHour(hour_inner);
			string hour_outer_s = FormatHour(hour_outer);
			
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
//			dt = dt.AsStandard(); // confusing
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
