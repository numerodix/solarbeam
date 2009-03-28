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
		private List<string> offsets;
		private Dictionary<string,List<string>> zones;
		private Dictionary<string,string> zones_rev;
		
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
			List<double> offset_list = new List<double>();
			foreach (KeyValuePair<double,List<TzTimeZone>> pair in zonedict) {
				offset_list.Add(pair.Key);
			}
			offset_list.Sort();
			
			// initialize datasource lists
			this.offsets = new List<string>();
			this.zones = new Dictionary<string,List<string>>();
			this.zones_rev = new Dictionary<string,string>();

			// build member lists by iterating sorted list
			foreach (double offset_d in offset_list) {
				string offset_s = FormatTimezone(offset_d);
				offsets.Add(offset_s);
				
				// sort timezones
				List<TzTimeZone> zlist = zonedict[offset_d];
				List<string> zname_list = new List<string>();
				foreach (TzTimeZone zone in zlist) {
					string zone_name = zone.StandardName;
					zname_list.Add(zone_name);

					if (!zones_rev.ContainsKey(zone_name)) {
						zones_rev.Add(zone_name, offset_s);
					}
				}
				zname_list.Sort();
				this.zones.Add(offset_s, zname_list);
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
			// leave this warning at the moment?
			try {
				return zones_rev[tz_name];
			} catch (Exception e) {
				Console.WriteLine("Failed to look up timezone: {0}", tz_name);
				throw e;
			}
		}
		
		public UTCDate? ApplyZone(string tz_name, DateTime dt)
		{
			try {
				TzTimeZone zone = TzTimeZone.GetTimeZone(tz_name);
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
		
		public string GetDSTStatus(string tz_name, UTCDate udt)
		{
			DateTime dt = udt.ExtractLocaltime();
			TzTimeZone zone = TzTimeZone.GetTimeZone(tz_name);
			DaylightTime dst = zone.GetDaylightChanges(dt.Year);
			string dst_s = "no DST";
			if ((dst != null) && (dst.Start.CompareTo(dst.End) != 0)) {
				if ((dt > dst.Start) && (dt < dst.End)) {
					dst_s = "DST";
				} else {
					dst_s = "Standard";
				}
			}
			return dst_s;
		}
		
		public void Ya(DateTime dt, string tz_name)
		{
			try {
			TzTimeZone zone = TzTimeZone.GetTimeZone(tz_name);
			TzDateTime tzdt = new TzDateTime(dt, zone);
			Console.WriteLine(tzdt);
			double utc_offset = tzdt.UtcOffset.TotalHours;
			Console.WriteLine("utc offset  : {0}", utc_offset);
			Console.WriteLine("local       : {0}", tzdt.DateTimeLocal);
			Console.WriteLine("utc         : {0}", tzdt.DateTimeUtc);
			Console.WriteLine("dst_name    : {0}", zone.DaylightName);
			Console.WriteLine("std_name    : {0}", zone.StandardName);
			Console.WriteLine("is dst?     : {0}", zone.IsDaylightSavingTime(dt));
			DaylightTime dst = zone.GetDaylightChanges(dt.Year);
			Console.WriteLine("dst_delta   : {0}", dst.Delta);
			Console.WriteLine("dst_start   : {0}", dst.Start);
			Console.WriteLine("dst_end     : {0}", dst.End);
			Console.WriteLine("");
			} catch {}
		}
		
		public List<string> Offsets
		{ get { return offsets; } }
		
		public List<string> GetTimezones(string offset)
		{
			return zones[offset];
		}
	}
}