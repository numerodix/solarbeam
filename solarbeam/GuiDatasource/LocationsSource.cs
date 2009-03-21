// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Collections.Generic;
using System.IO;

using LibSolar.Locations;
using LibSolar.Serialization;
using LibSolar.Types;

namespace SolarbeamGui
{
	class LocationsSource
	{
		private List<string> locations_list;
		private LocationList list;
		private string file = "locations.bin";
		
		public LocationsSource()
		{
			try {
				list = GetStoredList();
			} catch (FileNotFoundException) {
				list = LocationListData.GetStandardList();
			}

			// sort keys
			List<string> keylist = new List<string>();
			foreach (string name in list.Keys) {
				keylist.Add(name);
			}
			keylist.Sort();
			
			locations_list = keylist;
		}
		
		// serialize list in finalizer
		~LocationsSource()
		{
			StoreList();
		}
		
		private LocationList GetStoredList()
		{
			return ((LocationList) Serializer.Deserialize(file));
		}
		
		private void StoreList()
		{
			Serializer.Serialize(file, list);
		}
		
		public Location GetLocation(string name)
		{
			return list.Get(name);
		}
		
		public void RemoveLocation(string name)
		{
			list.Remove(name);
		}
		
		public List<string> Locations
		{ get { return locations_list; } }
		
		public string Tip
		{ get { return "Select an existing location or enter a new one"; } }
		
		public static string TipTitle
		{ get { return "Location"; } }
	}	
}
