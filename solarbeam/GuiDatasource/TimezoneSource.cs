// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;

using PublicDomain;

namespace SolarbeamGui
{
	class TimezoneSource
	{
		private string[] offsets;
		private Dictionary<string,string[]> zones;
		
		public TimezoneSource()
		{
			// use dummy date as reference point to extract utc offsets
			DateTime dummy = DateTime.Now;
			
			// build mapping offsets -> timezones
			Dictionary<double,List<TzTimeZone>> zonedict = 
				new Dictionary<double,List<TzTimeZone>>();
			foreach (string zone_name in TzTimeZone.AllZoneNames) {
				TzTimeZone zone = TzTimeZone.GetTimeZone(zone_name);
				
				TimeSpan span = zone.FindZone(dummy).UtcOffset;
				double span_d = span.TotalMinutes;

				if (!zonedict.ContainsKey(span_d)) {
					zonedict.Add(span_d, new List<TzTimeZone>());
				}
				zonedict[span_d].Add(zone);
			}
			
			// extract offsets to order numerically
			List<double> zonelist = new List<double>();
			foreach (KeyValuePair<double,List<TzTimeZone>> pair in zonedict) {
				zonelist.Add(pair.Key);
			}
			zonelist.Sort();
			
			// build member arrays by iterating sorted list
			this.offsets = new string[zonelist.Count+1]; // +1 for UTC
			this.zones = new Dictionary<string,string[]>();
			
			// set up UTC as index 0
			string utc_s = "UTC";
			this.offsets[0] = utc_s;
			this.zones[utc_s] = new string[] {};
			
			for (int i=0; i < zonelist.Count; i++) {
				double offset_d = zonelist[i];
				string offset_s = FormatTimezone(offset_d);
				offsets[i+1] = offset_s;
				
				List<TzTimeZone> zlist = zonedict[offset_d];
				string[] zarray = new string[zlist.Count];
				for (int j=0; j < zlist.Count; j++) {
					zarray[j] = zlist[j].ToString();
				}
				this.zones.Add(offset_s, zarray);
			}
			
			foreach (string offset in this.offsets) {
				Console.WriteLine("\n{0}", offset);
				foreach (string zone in this.zones[offset]) {
					Console.WriteLine("{0}", zone);
				}
			}

		}
		
		private string FormatTimezone(double tz)
		{
			int hour = (int) (Math.Abs(tz) / 60.0);
			int min = (int) (Math.Abs(tz) - (hour * 60.0));
			string sign = (tz < 0) ? "-" : "+";
			
			string min_s = String.Empty;
			if (min > 0) {
				min_s = String.Format(":{0:00}", min);	
			}
			return String.Format("{0}{1:00}{2}", sign, hour, min_s);
		}
		
		public string[] Offsets
		{ get { return offsets; } }
	}
}