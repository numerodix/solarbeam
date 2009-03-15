// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using LibSolar.Types;

namespace SolarbeamGui
{
	class PositionSource
	{
		private string[] dirs_latitude;
		private string[] dirs_longitude;
		
		public PositionSource()
		{
			dirs_latitude = new string[] {
				PositionDirection.North.ToString(),
				PositionDirection.South.ToString()
			};
			dirs_longitude = new string[] {
				PositionDirection.East.ToString(),
				PositionDirection.West.ToString()
			};
		}
		
		public string[] LatitudeDirections
		{ get { return dirs_latitude; } }
		
		public string[] LongitudeDirections
		{ get { return dirs_longitude; } }
	}	
}