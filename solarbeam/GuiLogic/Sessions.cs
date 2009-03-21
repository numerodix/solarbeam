// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using LibSolar.Serialization;

namespace SolarbeamGui
{
	/**
	 * Handle sessions.
	 */
	static partial class Controller
	{
		public static void SaveSession()
		{
			// get a new dict, apparently cannot write to cache while 
			// enumerating (wtf?)
			Dictionary<Id,string> dict = new Dictionary<Id,string>();
			
			// flush all widget values to dict
			foreach (KeyValuePair<Id,string> pair in cache) {
				Id id = pair.Key;
				string val = GetValue(registry[id]);
				dict.Add(id, val);
			}
			Serializer.Serialize("last_session.bin", dict);
		}
		
		public static void LoadSession()
		{
			// get a new dict, apparently cannot write to cache while 
			// enumerating (wtf?)
			Dictionary<Id,string> dict = 
				(Dictionary<Id,string>) Serializer.Deserialize("last_session.bin");
			
			// if key found in dict is in cache, set it and set the widget
			foreach (KeyValuePair<Id,string> pair in dict) {
				Id id = pair.Key;
				string val = dict[id];
				if ((cache.ContainsKey(id)) && (val != null)) {
					cache[id] = val;
					SetValue(registry[id], val);
				}
			}
		}
	}
}