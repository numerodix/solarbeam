// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Collections.Generic;

using LibSolar.Locations;
using LibSolar.Types;

namespace SolarbeamGui
{
	class LocationsSource
	{
		private string[] locations_array;
		private Dictionary<string,Location> locations_dict;
		
		public LocationsSource()
		{
			locations_dict = new Dictionary<string,Location>();
			locations_dict.Add("Trondheim", 
			                   new Location("Trondheim",
			                                new Position(Position.LATITUDE_POS,
			                                             63, 25, 47,
			                                             Position.LONGITUDE_POS,
			                                             10, 23, 36),
			                                "Europe/Oslo"));
			locations_dict.Add("Baku", 
			                   new Location("Baku",
			                                new Position(Position.LATITUDE_POS,
			                                             40, 23, 43,
			                                             Position.LONGITUDE_POS,
			                                             49, 52, 56),
			                                "Asia/Baku"));
			locations_dict.Add("Sydney", 
			                   new Location("Sydney",
			                                new Position(Position.LATITUDE_NEG,
			                                             33, 51, 36,
			                                             Position.LONGITUDE_POS,
			                                             151, 12, 40),
			                                "Australia/Sydney"));

			locations_array = new string[locations_dict.Count];
			int i=-1;
			foreach (KeyValuePair<string,Location> pair in locations_dict) {
				i++;
				locations_array[i] = pair.Key;
			}
		}
		
		public Location GetLocation(string name)
		{
			return locations_dict[name];
		}
		
		public string[] Locations
		{ get { return locations_array; } }
	}	
}
