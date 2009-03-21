// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using LibSolar.Types;

namespace SolarbeamGui
{
	class PositionSource
	{
		private List<string> dirs_latitude;
		private List<string> dirs_longitude;
		
		public PositionSource()
		{
			dirs_latitude = new List<string>();
			dirs_latitude.Add(PositionDirection.North.ToString());
			dirs_latitude.Add(PositionDirection.South.ToString());
			
			dirs_longitude = new List<string>();
			dirs_longitude.Add(PositionDirection.East.ToString());
			dirs_longitude.Add(PositionDirection.West.ToString());
		}
		
		public List<string> LatitudeDirections
		{ get { return dirs_latitude; } }
		
		public List<string> LongitudeDirections
		{ get { return dirs_longitude; } }
		
		public static string LatTipTitle
		{ get { return "Latitude"; } }
		
		public static string TipLatDegree
		{ get { return String.Format(
					"Enter the number of degrees latitude ({0}-{1})",
					Position.LATDEGS_MINVALUE,
					Position.LATDEGS_MAXVALUE);
		} }

		public static string TipLatMinute
		{ get { return String.Format(
					"Enter the number of minutes latitude ({0}-{1})",
					Position.LATMINS_MINVALUE,
					Position.LATMINS_MAXVALUE);
		} }

		public static string TipLatSecond
		{ get { return String.Format(
					"Enter the number of seconds latitude ({0}-{1})",
					Position.LATSECS_MINVALUE,
					Position.LATSECS_MAXVALUE);
		} }

		public static string LonTipTitle
		{ get { return "Longitude"; } }

		public static string TipLonDegree
		{ get { return String.Format(
					"Enter the number of degrees longitude ({0}-{1})",
					Position.LONDEGS_MINVALUE,
					Position.LONDEGS_MAXVALUE);
		} }

		public static string TipLonMinute
		{ get { return String.Format(
					"Enter the number of minutes longitude ({0}-{1})",
					Position.LONMINS_MINVALUE,
					Position.LONMINS_MAXVALUE);
		} }

		public static string TipLonSecond
		{ get { return String.Format(
					"Enter the number of seconds longitude ({0}-{1})",
					Position.LONSECS_MINVALUE,
					Position.LONSECS_MAXVALUE);
		} }
	}	
}
