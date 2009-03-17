// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Globalization;

using LibSolar.Types;
using PublicDomain;

namespace SolarbeamGui
{
	class TimezoneSource
	{
		private string[] offsets;
		private Dictionary<string,string[]> zones;
		private Dictionary<string,string> zones_rev;
		
		public TimezoneSource()
		{
			// use dummy date as reference point to extract utc offsets
			DateTime dummy = DateTime.Now;
			
			// build mapping offsets -> timezones
			Dictionary<double,List<TzTimeZone>> zonedict = 
				new Dictionary<double,List<TzTimeZone>>();
			foreach (string zone_name in TzTimeZoneLoader.AllZoneNames) {
				TzTimeZone zone = TzTimeZoneLoader.GetTimeZone(zone_name);
				
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
			
			// initialize datasource arrays
			this.offsets = new string[zonelist.Count];
			this.zones = new Dictionary<string,string[]>();
			this.zones_rev = new Dictionary<string,string>();

			// build member arrays by iterating sorted list
			for (int i=0; i < zonelist.Count; i++) {
				double offset_d = zonelist[i];
				string offset_s = FormatTimezone(offset_d);
				offsets[i] = offset_s;
				
				List<TzTimeZone> zlist = zonedict[offset_d];
				string[] zarray = new string[zlist.Count];
				for (int j=0; j < zlist.Count; j++) {
					string zone_name = zlist[j].ToString();
					zarray[j] = zone_name;
					
					zones_rev.Add(zone_name, offset_s);
				}
				this.zones.Add(offset_s, zarray);
			}
			
//			foreach (string offset in this.offsets) {
//				Console.WriteLine("\n{0}", offset);
//				foreach (string zone in this.zones[offset]) {
//					Console.WriteLine("{0}", zone);
//				}
//			}
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
		
		public string GetOffsetName(string tz_name)
		{
			return zones_rev[tz_name];
		}
		
		public UTCDate? ApplyZone(string tz_name, DateTime dt)
		{
			try {
				TzTimeZone zone = TzTimeZoneLoader.GetTimeZone(tz_name);
				TzDateTime tzdt = new TzDateTime(dt, zone);
				double offset = tzdt.UtcOffset.TotalHours;
				DateTime dt_new = tzdt.DateTimeUtc;
				return new UTCDate(offset,
				                   dt_new.Year, dt_new.Month, dt_new.Day,
				                   dt_new.Hour, dt_new.Minute, dt_new.Second);
			} catch (TypeInitializationException) {
				return null;
			}
		}
		
		public string[] Offsets
		{ get { return offsets; } }
		
		public string[] GetTimezones(string offset)
		{
			return zones[offset];
		}
	}
}
