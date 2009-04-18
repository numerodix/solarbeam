// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace LibSolar.Graphing
{
	/**
	 * The class demonstrates the typical use of the graphing library.
	 */
	public class GraphBitmapDemo
	{
		public static void GenerateBitmap(int dim, string path)
		{
			string loc = "South Pole";
			
			// loc: Equator
			Position pos = new Position(Position.LATITUDE_NEG,
			                            89, 59, 59,
			                            Position.LONGITUDE_POS,
			                            179, 59, 59);
			// time: Now
			DateTime dt = DateTime.Now;
			UTCDate udt = new UTCDate(0, null,
			                         dt.Year, dt.Month, dt.Day,
			                         dt.Hour, dt.Minute, dt.Second);

			// set up constants
			Colors colors = new Colors();
			string font_face = "Arial";
			
			// generate base image
			GraphBitmap grbit = new GraphBitmap(true, dim, colors, font_face);
			Bitmap bitmap_plain = grbit.RenderBaseImage(pos, udt);
			
			// render current day
			Bitmap bitmap = grbit.RenderCurrentDay(bitmap_plain, dim, pos, udt);
			
			// render caption
			CaptionInfo ci = new CaptionInfo(loc, pos, udt);
			bitmap = grbit.RenderCaption(ci);
			
			// save
			grbit.SaveBitmap(bitmap, path);
		}
	}
}
