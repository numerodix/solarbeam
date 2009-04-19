// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Globalization;

using PublicDomain;

using LibSolar.Types;

namespace SolarbeamGui
{
	enum DSTStatus {
		NoDST,
		Standard,
		Daylight,
	}
	
	sealed class TimezoneSource
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
		
		public UTCDate? GetUTCDate(string tz_name, DateTime dt)
		{
			try {
				TzTimeZone zone = TzTimeZone.GetTimeZone(tz_name);
				double offset = zone.FindZone(dt).UtcOffset.TotalHours;
				DaylightTime dst = zone.GetDaylightChanges(dt.Year);
				return new UTCDate(offset, dst,
				                   dt.Year, dt.Month, dt.Day,
				                   dt.Hour, dt.Minute, dt.Second);
			} catch (TypeInitializationException) {
				return null;
			}
		}
		
		public DSTStatus GetDSTStatus(UTCDate udt)
		{
			DSTStatus dst_s = DSTStatus.NoDST;
			if (udt.HasDST) {
				switch (udt.IsDST) {
				case true:
					dst_s = DSTStatus.Daylight; break;
				case false:
					dst_s = DSTStatus.Standard; break;
				}
			}
			return dst_s;
		}
		
		public List<string> Offsets
		{ get { return offsets; } }
		
		public List<string> GetTimezones(string offset)
		{
			return zones[offset];
		}

		private void PrintDate(DateTime dt, string tz_name)
		{
			TzTimeZone zone = TzTimeZone.GetTimeZone(tz_name);
			TzDateTime tzdt = new TzDateTime(dt, zone);
			TzDatabase.TzZone dbzone = zone.FindZone(dt);
			Console.WriteLine(tzdt);
			double utc_offset = tzdt.UtcOffset.TotalHours;
			double utc_std_offset = dbzone.UtcOffset.TotalHours;
			Console.WriteLine("utc offset      : {0}", utc_offset);
			Console.WriteLine("utc_std offset  : {0}", utc_std_offset);
			Console.WriteLine("local           : {0}", tzdt.DateTimeLocal);
			Console.WriteLine("utc             : {0}", tzdt.DateTimeUtc);
			Console.WriteLine("dst_name        : {0}", zone.DaylightName);
			Console.WriteLine("std_name        : {0}", zone.StandardName);
			Console.WriteLine("is dst?         : {0}", zone.IsDaylightSavingTime(dt));
			try {
				DaylightTime dst = zone.GetDaylightChanges(dt.Year);
				Console.WriteLine("dst_delta       : {0}", dst.Delta);
				Console.WriteLine("dst_start       : {0}", dst.Start);
				Console.WriteLine("dst_end         : {0}", dst.End);
			} catch (NullReferenceException) {}
			Console.WriteLine(string.Empty);
		}
		
		private void PrintZonelist()
		{
			foreach (string offset in this.offsets) {
				Console.WriteLine("\n{0}", offset);
				foreach (string zone in this.zones[offset]) {
					Console.WriteLine("{0}", zone);
				}
			}
		}
	}
}
