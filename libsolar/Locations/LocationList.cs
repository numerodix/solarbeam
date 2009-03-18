// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Collections.Generic;

using LibSolar.Serialization;
using LibSolar.Types;

namespace LibSolar.Locations
{
	[Serializable]
	public class LocationList
	{
		private Dictionary<string,Location> locations;
		
		public LocationList()
		{
			locations = new Dictionary<string,Location>();
		}
		
		public void Add(string name, string timezone, Position pos)
		{
			Location loc = new Location(name, pos, timezone);
			locations.Add(name, loc);
		}
		
		public Location Get(string name)
		{
			return locations[name];
		}
		
		public Dictionary<string,Location>.KeyCollection Keys
		{ get { return locations.Keys; } }
		
		public int Count
		{ get { return locations.Count; } }
	}
}