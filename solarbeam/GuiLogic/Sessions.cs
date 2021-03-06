// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.ComponentModel;

using LibSolar;
using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Handle sessions.
	 */
	static partial class Controller
	{
		public static void SaveAutoSession()
		{
			// get a new dict, apparently cannot write to cache while 
			// enumerating (wtf?)
			Dictionary<Id,string> dict = new Dictionary<Id,string>();
			
			// flush all widget values to dict
			foreach (KeyValuePair<Id,Component> pair in registry) {
				Id id = pair.Key;
				string val = GetValue(registry[id]);
				dict.Add(id, val);
			}
			Serializer.Serialize(Controller.AsmInfo, Constants.AutoSessionFilename, dict);
		}
		
		public static void LoadAutoSession(string file)
		{
			// get a new dict, apparently cannot write to cache while 
			// enumerating (wtf?)
			Dictionary<Id,string> dict = 
				(Dictionary<Id,string>) Serializer.Deserialize(Controller.AsmInfo,
				                                               file);
			
			// if key found in dict is in cache, set it and set the widget
			foreach (KeyValuePair<Id,string> pair in dict) {
				Id id = pair.Key;
				string val = dict[id];
				if ((registry.ContainsKey(id)) && (val != null)) {
					cache[id] = val;
					SetValue(registry[id], val);
				}
			}
		}
		
		private static void WriteSession(string filename)
		{
			Dictionary<Id,string> dict = new Dictionary<Id,string>();
			
			// save location, position and date
			foreach (KeyValuePair<Id,Component> pair in registry) {
				Id id = pair.Key;
				Component control = pair.Value;
				if (ins_position.Contains(id) || ins_timedate.Contains(id)) {
					dict.Add(id, GetValue(control));
				}
			}
			Serializer.Serialize(Controller.AsmInfo, filename, dict);
		}
		
		private static void ReadSession(string filename)
		{
			Dictionary<Id,string> dict = 
				(Dictionary<Id,string>) Serializer.Deserialize(Controller.AsmInfo,
				                                               filename);
			
			// if key found	in dict is in registry, set it and set the widget
			foreach (KeyValuePair<Id,string> pair in dict) {
				Id id = pair.Key;
				string val = pair.Value;
				if ((registry.ContainsKey(id)) && (val != null)) {
					cache[id] = val;
					SetValue(registry[id], val);
				}
			}
		}
	}
}