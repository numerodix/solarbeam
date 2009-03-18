// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Collections.Generic;

using LibSolar.Locations;
using LibSolar.Serialization;
using LibSolar.Types;

namespace SolarbeamGui
{
	class LocationsSource
	{
		private string[] locations_array;
		private LocationList list;
		
		public LocationsSource()
		{
			list = new LocationList();
			list.Add("Baku", "Asia/Baku",
					new Position(Position.LATITUDE_POS, 40, 23, 43,
						Position.LONGITUDE_POS, 49, 52, 56));
			list.Add("Buenos Aires", "America/Buenos_Aires",
					new Position(Position.LATITUDE_NEG, 34, 36, 36,
						Position.LONGITUDE_NEG, 58, 22, 12));
			list.Add("Equator", "UTC",
					new Position(Position.LATITUDE_NEG, 0, 0, 0,
						Position.LONGITUDE_NEG, 0, 0, 0));
			list.Add("Sydney", "Australia/Sydney",
				new Position(Position.LATITUDE_NEG, 33, 51, 36,
						Position.LONGITUDE_POS, 151, 12, 40));
			list.Add("Tromsø", "Europe/Oslo",
					new Position(Position.LATITUDE_POS, 69, 40, 58,
						Position.LONGITUDE_POS, 18, 56, 34));
			list.Add("Trondheim", "Europe/Oslo",
					new Position(Position.LATITUDE_POS, 63, 25, 47,
						Position.LONGITUDE_POS, 10, 23, 36));

			locations_array = new string[list.Count];
			int i=-1;
			foreach (string name in list.Keys) {
				i++;
				locations_array[i] = list.Get(name).Name;
			}
		}
		
		public Location GetLocation(string name)
		{
			return list.Get(name);
		}
		
		public string[] Locations
		{ get { return locations_array; } }

	}	
}
