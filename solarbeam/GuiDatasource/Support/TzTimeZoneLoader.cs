// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using PublicDomain;

namespace SolarbeamGui
{	
	struct Zone
	{
		public string zone_name;
		public List<TzDatabase.TzZone> zones;
		public List<TzDatabase.TzRule> rules;
		
		public Zone(string zone_name)
		{
			this.zone_name = zone_name;
			this.zones = new List<TzDatabase.TzZone>();
			this.rules = new List<TzDatabase.TzRule>();
			}
	}
		
	class TzTimeZoneLoader
	{
		private static Dictionary<string,TzTimeZone.TzZoneInfo> yzone_dict = null;
		
		private static void InitZones()
		{
			if (yzone_dict != null) {
				return;
			}
			
			List<TzDatabase.TzRule> rule_list = new List<TzDatabase.TzRule>();
			List<TzDatabase.TzZone> zone_list = new List<TzDatabase.TzZone>();
			List<string[]> links_list = new List<string[]>();
			TzDatabase.ReadDatabase("./tz", rule_list, zone_list, links_list);

//			Console.WriteLine("------------ RULES");
//			foreach (TzDatabase.TzRule rule in rules) {
//				Console.WriteLine(rule.ToString());
//			}

//			Console.WriteLine("------------ ZONES");
//			foreach (TzDatabase.TzZone zone in zones) {
//				Console.WriteLine(zone.ZoneName);
//				Console.WriteLine(zone.RuleName);
//			}
/*
			Console.WriteLine("------------ LINKS");
			foreach (string[] link in links) {
				Console.WriteLine("===== NEXT");
				foreach (string l in link) {
					Console.WriteLine(l);
				}
			}
*/
			// zone_name -> Zone mapping
			Dictionary<string,Zone> zone_dict = new Dictionary<string,Zone>();

			// iterate over all zones to gather all entries under common key
			foreach (TzDatabase.TzZone zone in zone_list) {
				string zone_name = zone.ZoneName;
				string rule_name = zone.RuleName;
				if (!zone_dict.ContainsKey(zone_name)) {
					zone_dict.Add(zone_name, new Zone(zone_name));
				}
				zone_dict[zone_name].zones.Add(zone);

				// iterate over all rules to see if they match this zone
				foreach (TzDatabase.TzRule rule in rule_list) {
					if (rule.RuleName == rule_name) {
						zone_dict[zone_name].rules.Add(rule);
					}
				}
			}

			// instantiate all timezones
			foreach (KeyValuePair<string,Zone> pair in zone_dict) {
				string zone_name = pair.Value.zone_name;
				List<TzDatabase.TzZone> zones = pair.Value.zones;
				List<TzDatabase.TzRule> rules = pair.Value.rules;
				TzTimeZone.TzZoneInfo tzzone = 
					new TzTimeZone.TzZoneInfo(zone_name, zones, rules);
				yzone_dict.Add(zone_name, tzzone);
			}
		}
		
		public new static TzTimeZone GetTimeZone(string tzName)
		{
			InitZones();
			
			TzTimeZone result = null;
            TzTimeZone.TzZoneInfo zoneInfo = yzone_dict[tzName];
			result = new TzTimeZone(zoneInfo);
			
			return result;
		}
		
		public static string[] AllZoneNames
		{ get { 
				InitZones();
				
				string[] result = new string[yzone_dict.Count];
				int i = -1;
				foreach (KeyValuePair<string,TzTimeZone.TzZoneInfo> pair in yzone_dict) {
					i++;
					result[i] = pair.Key;
				}
				return result;
			} }
	}
}