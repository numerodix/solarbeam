// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Types
{
	public enum PositionAxis
	{
		Longitude,
		Latitude,
	}

	public enum PositionDirection
	{
		North,
		South,
		East,
		West,
	}

	/**
	 * Class to package position primitives, and to intercept invalid values.
	 */
	[Serializable]
	public class Position
	{
		[NonSerialized]
		public static PositionDirection LATITUDE_POS = PositionDirection.North;
		[NonSerialized]
		public static PositionDirection LATITUDE_NEG = PositionDirection.South;
		[NonSerialized]
		public static PositionDirection LONGITUDE_POS = PositionDirection.East;
		[NonSerialized]
		public static PositionDirection LONGITUDE_NEG = PositionDirection.West;
		
		[NonSerialized]
		public const int LATDEGS_MIN = 0;
		[NonSerialized]
		public const int LATDEGS_MAX = 90;
		[NonSerialized]
		public const int LATMINS_MIN = 0;
		[NonSerialized]
		public const int LATMINS_MAX = 59;
		[NonSerialized]
		public const int LATSECS_MIN = 0;
		[NonSerialized]
		public const int LATSECS_MAX = 59;
		
		[NonSerialized]
		public const int LONDEGS_MIN = 0;
		[NonSerialized]
		public const int LONDEGS_MAX = 180;
		[NonSerialized]
		public const int LONMINS_MIN = 0;
		[NonSerialized]
		public const int LONMINS_MAX = 59;
		[NonSerialized]
		public const int LONSECS_MIN = 0;
		[NonSerialized]
		public const int LONSECS_MAX = 59;

		private double latitude;
		private double longitude;

		private int latitude_int;
		private int longitude_int;

		public Position(PositionDirection ladir,
		                int ladeg, int lamin, int lasec,
		                PositionDirection lodir,
		                int lodeg, int lomin, int losec)
		{
			latitude_int = CollapsePositionUnits(ladir, ladeg, lamin, lasec);
			longitude_int = CollapsePositionUnits(lodir, lodeg, lomin, losec);

			CheckIsLatitude(ladir);
			CheckIsLongitude(lodir);

			CheckLatitudeVal(latitude_int);
			CheckLongitudeVal(longitude_int);

			longitude_int = InvertLongitude(longitude_int);

			latitude = latitude_int / 3600.0;
			longitude = longitude_int / 3600.0;
			
			latitude = AdjustLatitude(latitude);
		}

		public static PositionDirection LatDirFromVal(int val)
		{
			if (val >= 0) {
				return LATITUDE_POS;
			} else {
				return LATITUDE_NEG;
			}
		}

		public static PositionDirection LonDirFromVal(int val)
		{
			if (val >= 0) {
				return LONGITUDE_POS;
			} else {
				return LONGITUDE_NEG;
			}
		}

		/**
		 * Ensure direction corresponds to latitude direction.
		 */
		private void CheckIsLatitude(PositionDirection dir)
		{
			if ((dir != PositionDirection.North) && (dir != PositionDirection.South))
			{
				throw new ArgumentException(
						string.Format("Bad direction for latitude: {0}", dir));
			}
		}

		/**
		 * Ensure direction corresponds to longitude direction.
		 */
		private void CheckIsLongitude(PositionDirection dir)
		{
			if ((dir != PositionDirection.East) && (dir != PositionDirection.West))
			{
				throw new ArgumentException(
						string.Format("Bad direction for longitude: {0}", dir));
			}
		}

		/**
		 * Latitude [0,90] represents north of equator.
		 * Latitude [0,-90] represents south of equator.
		 */
		private void CheckLatitudeVal(int lat)
		{
			if ( (lat < -90*3600) || (lat > 90*3600) ) {
				throw new ArgumentException(
						string.Format("Bad value for latitude: {0}", lat));
			}
		}

		/**
		 * Longitude [0,180] represents west of 0 meridian.
		 * Longitude [0,-180] represents east of 0 meridian.
		 */
		private void CheckLongitudeVal(int lon)
		{
			if ( (lon < -180*3600) || (lon > 180*3600) ) {
				throw new ArgumentException(
						string.Format("Bad value for longitude: {0}", lon));
			}
		}

		/**
		 * The original SRRB code has this adjustment, presumably to avoid bumping
		 * up against the treacherous 90 degrees value in trigonometric functions.
		 */
		private static double AdjustLatitude(double lat)
		{
			if ( (lat >= -90*3600.0) && (lat < -89.8*3600.0) ) {
				return -89.8*3600.0;
			}
			if ( (lat <= 90*3600.0) && (lat > 89.8*3600.0) ) {
				return 89.8*3600.0;
			}
			return lat;
		}

		/**
		 * Most common is to represent east longitude as positive. Solarbeam does
		 * the opposite, so we accept input in the standard form and switch the
		 * sign.
		 */
		private static int InvertLongitude(int lon)
		{
			return -lon;
		}

		/**
		 * Normalize integer position to decimal value.
		 */
		public static int CollapsePositionUnits(PositionDirection dir,
		                                        int deg, int min, int sec)
		{
			int multiplier =
				( (dir == LATITUDE_NEG) || (dir == LONGITUDE_NEG) ? -1 : 1 );
			return 
				( multiplier
				 * ( Math.Abs(deg) * 3600
					 + ( Math.Abs(min) * 60)
					 + ( Math.Abs(sec) ) ));
		}
		
		public static void Expand(double degs,
		                          out int deg, out int min, out int sec)
		{
			degs = Math.Abs(degs);

			deg = (int) (degs / 3600.0);
			degs = degs - deg * 3600.0;
			min = (int) (degs / 60.0);
			degs = degs - min * 60.0;
			sec = (int) degs;
		}

		/**
		 * Extract integer representation of position.
		 * This method is for returning results to client, therefore remember to
		 * invert longitude.
		 */
		private static Degree ExpandPositionUnits(PositionAxis ax, int degs)
		{
			PositionDirection dir;

			if (ax == PositionAxis.Latitude) {
				dir = LATITUDE_POS;

				if (degs < 0) {
					dir = LATITUDE_NEG;
				}
			} else {
				dir = LONGITUDE_POS;

				degs = InvertLongitude(degs);
				if (degs < 0) {
					dir = LONGITUDE_NEG;
				}
			}

			int deg = 0;
			int min = 0;
			int sec = 0;
			Expand(degs, out deg, out min, out sec);

			return new Degree(dir, deg, min, sec);
		}
		
		public static int GetGeographicTimezoneOffset(Position pos)
		{
			Degree v = pos.LongitudeDegree;
			double lon = (double) CollapsePositionUnits(v.Direction,
			                                            v.Deg,
			                                            v.Min,
			                                            v.Sec) / 3600.0;
			
			// center around 0
			if (lon >= 0) lon = lon + 7.5;
			else lon = lon - 7.5;
			
			return (int) (lon / 15.0);
		}

		public double Latitude
		{ get { return latitude; } }

		public Degree LatitudeDegree
		{ get { return ExpandPositionUnits(PositionAxis.Latitude, latitude_int); } }

		public double Longitude
		{ get { return longitude; } }

		public Degree LongitudeDegree
		{ get { return ExpandPositionUnits(PositionAxis.Longitude, longitude_int); } }

		// Helpers
		public string PrintLatitude()
		{
			Degree d = LatitudeDegree;
			return d.Print();
		}

		public string PrintLongitude()
		{
			Degree d = LongitudeDegree;
			return d.Print();
		}
	}

	/**
	 * Struct to represent position coordinate in long form.
	 */
	public struct Degree
	{
		private PositionDirection dir;
		private int deg;
		private int min;
		private int sec;

		public Degree(PositionDirection dir,
		              int deg, int min, int sec)
		{
			this.dir = dir;
			this.deg = deg;
			this.min = min;
			this.sec = sec;
		}

		public PositionDirection Direction
		{ get { return dir; } }

		public int Deg
		{ get { return deg; } }

		public int Min
		{ get { return min; } }

		public int Sec
		{ get { return sec; } }

		// Helpers
		public string Print()
		{
			string dir_s = "E";
			switch (dir)
			{
				case PositionDirection.West: dir_s = "W"; break;
				case PositionDirection.North: dir_s = "N"; break;
				case PositionDirection.South: dir_s = "S"; break;
			}
			return string.Format("{1,3:G}° {2,2:G}' {3,2:G}\" {0}",
			                     dir_s, deg, min, sec);
		}
	}
}
