// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.Util
{
	public class Formatter
	{
		public const string SessionFileExtension = ".solarbeam";
		public const string SessionFileFilter = "*" + SessionFileExtension;
		public const string SessionFileDesc = "SolarBeam sessions";
		
		public const string ImageFileExtension = ".png";
		public const string ImageFileFilter = "*" + ImageFileExtension;
		public const string ImageFileDesc = "Png images";
		
		public const string BinaryFileExtension = ".bin";
		public const string AutoSessionFilename = "autosave" + BinaryFileExtension;
		public const string LocationListFilename = "locations" + BinaryFileExtension;
		
		public static string FormatImgFilename(string loc, Position pos, UTCDate dt)
		{
			return FormatFilename(loc, pos, dt) + ImageFileExtension;
		}
		
		public static string FormatSessionFilename(string loc, Position pos, UTCDate dt)
		{
			return FormatFilename(loc, pos, dt) + SessionFileExtension;
		}
		
		private static string FormatFilename(string loc, Position pos, UTCDate dt)
		{
			string dt_s = String.Format("{0:0000}-{1:00}-{2:00}",
			                            dt.Year, dt.Month, dt.Day);
			string tm_s = String.Format("{0:00}-{1:00}-{2:00}",
			                            dt.Hour, dt.Minute, dt.Second);
			string loc_s = loc;
			if (loc == String.Empty) {
				string latdir_s = (pos.LatitudeDegree.Direction
						== PositionDirection.North) ? "N" : "S";
				string lat_s = String.Format("{0}{1:00}-{2:00}-{3:00}",
				                             latdir_s,
				                             pos.LatitudeDegree.Deg,
				                             pos.LatitudeDegree.Min,
				                             pos.LatitudeDegree.Sec);
				string londir_s = (pos.LongitudeDegree.Direction
						== PositionDirection.East) ? "E" : "W";
				string lon_s = String.Format("{0}{1:00}-{2:00}-{3:00}",
				                             londir_s,
				                             pos.LongitudeDegree.Deg,
				                             pos.LongitudeDegree.Min,
				                             pos.LongitudeDegree.Sec);
				loc_s = String.Format("{0} {1}", lat_s, lon_s);
			}
			string s = String.Format("{0} - {1} {2}", loc_s, dt_s, tm_s);
			return s;
		}	
	}
}