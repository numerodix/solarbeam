// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using PublicDomain;

namespace SolarbeamGui
{	
	/**
	 * Helper struct used during init.
	 */
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

	/**
	 * Provide an implementation of relevant methods from TzTimeZone class.
	 */
	class TzTimeZoneLoader
	{
		private static Dictionary<string,TzTimeZone.TzZoneInfo> zone_dict = null;

		/**
		 * Override TzTimeZone.GetTimeZone monstrocity that adds 5-6 seconds
		 * to static loading. Load from bundled zoneinfo distribution.
		 */
		private static void InitZones()
		{
			if (zone_dict != null) {
				return;
			} else {
				zone_dict = new Dictionary<string,TzTimeZone.TzZoneInfo>();
			}

			// init datastructures for ReadDatabase call
			List<TzDatabase.TzRule> rule_list = new List<TzDatabase.TzRule>();
			List<TzDatabase.TzZone> zone_list = new List<TzDatabase.TzZone>();
			List<string[]> links_list = new List<string[]>();

			// read database files
			TzDatabase.ReadDatabase("./tz", rule_list, zone_list, links_list);

			// zone_name -> Zone mapping
			Dictionary<string,Zone> name_zone_dict = new Dictionary<string,Zone>();

			// iterate over all zones to gather all entries under common key
			foreach (TzDatabase.TzZone zone in zone_list) {
				string zone_name = zone.ZoneName;
				string rule_name = zone.RuleName;
				if (!name_zone_dict.ContainsKey(zone_name)) {
					name_zone_dict.Add(zone_name, new Zone(zone_name));
				}
				name_zone_dict[zone_name].zones.Add(zone);

				// iterate over all rules to see if they match this zone
				foreach (TzDatabase.TzRule rule in rule_list) {
					if (rule.RuleName == rule_name) {
						name_zone_dict[zone_name].rules.Add(rule);
					}
				}
			}

			// instantiate all timezones
			foreach (KeyValuePair<string,Zone> pair in name_zone_dict) {
				string zone_name = pair.Value.zone_name;
				List<TzDatabase.TzZone> zones = pair.Value.zones;
				List<TzDatabase.TzRule> rules = pair.Value.rules;
				TzTimeZone.TzZoneInfo tzzone = 
					new TzTimeZone.TzZoneInfo(zone_name, zones, rules);
				zone_dict.Add(zone_name, tzzone);
				
				// add links
				foreach (string[] link in links_list) {
					string from = link[1];
					string to = link[2];
					if (from == zone_name) {
						tzzone = new TzTimeZone.TzZoneInfo(to, zones, rules);
						zone_dict.Add(to, tzzone);
					}
				}
			}
		}

		/**
		 * Override TzTimeZone.GetTimeZone
		 */
		public static TzTimeZone GetTimeZone(string tzName)
		{
			InitZones();

			TzTimeZone.TzZoneInfo zoneInfo = zone_dict[tzName];
			TzTimeZone result = new TzTimeZone(zoneInfo);

			return result;
		}

		/**
		 * Override TzTimeZone.AllZoneNames
		 */
		public static string[] AllZoneNames
		{ get { 
				  InitZones();

				  string[] result = new string[zone_dict.Count];
				  int i = -1;
				  foreach (KeyValuePair<string,TzTimeZone.TzZoneInfo> pair in zone_dict) {
					  i++;
					  result[i] = pair.Key;
				  }
				  return result;
			  } }
	}
}
