// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;

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
			// loc: Trondheim
			Position pos = new Position(Position.LATITUDE_POS,
			                            63, 25, 47,
			                            Position.LONGITUDE_POS,
			                            10, 23, 36);
			// time: Now
			DateTime now = DateTime.Now;
			UTCDate dt = new UTCDate(1,
			                         now.Year, now.Month, now.Day, 
			                         now.Hour, now.Minute, now.Second);

			// set up constants
			Colors colors = new Colors(Color.White, Color.Gray, Color.Black,
			                           Color.Blue, Color.Green,
			                           Color.Red);
			string font_face = "Arial";
			
			// generate base image
			GraphBitmap grbit = new GraphBitmap(dim, colors, font_face);
			Bitmap bitmap_plain = grbit.RenderBaseImage(pos, dt);
			
			// render current day
			Bitmap bitmap_fst = grbit.RenderCurrentDay(bitmap_plain, dim, pos, dt);
			
			// save
			grbit.SaveBitmap(bitmap_fst, path);
		}
	}
}
