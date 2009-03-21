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
	}	
}