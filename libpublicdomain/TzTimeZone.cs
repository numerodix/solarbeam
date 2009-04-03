using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace PublicDomain
{
    /// <summary>
    /// Represents a Time Zone from the Olson tz database.
    /// 
    /// From http://www.twinsun.com/tz/tz-link.htm
    /// "The public-domain time zone database contains code
    /// and data that represent the history of local time
    /// for many representative locations around the globe."
    /// </summary>
    [Serializable]
    [XmlType(Namespace = GlobalConstants.PublicDomainNamespace)]
    [SoapType(Namespace = GlobalConstants.PublicDomainNamespace)]
    public class TzTimeZone : TimeZone
    {
        /// <summary>
        /// 
        /// </summary>
        public static bool TreatUnspecifiedKindAsLocal = true;

        /// <summary>
        /// 
        /// </summary>
        public static readonly TzTimeZone ZoneUTC;

        /// <summary>
        /// 
        /// </summary>
        public static readonly TzTimeZone ZoneUsEastern;

        /// <summary>
        /// 
        /// </summary>
        public static readonly TzTimeZone ZoneUsCentral;

        /// <summary>
        /// 
        /// </summary>
        public static readonly TzTimeZone ZoneUsMountain;

        /// <summary>
        /// 
        /// </summary>
        public static readonly TzTimeZone ZoneUsPacific;

        private static Dictionary<double, string> s_mainTimeZones = new Dictionary<double, string>();
        private static Dictionary<string, TzZoneInfo> s_zones = new Dictionary<string, TzZoneInfo>();
        private static ReadOnlyCollection<TzZoneInfo> s_zoneList;
        private static string[] s_allZoneNames;
        private static ReaderWriterLock s_zonesLock = new ReaderWriterLock();
        private static object s_allZonesLock = new object();
        private static object s_zoneListLock = new object();

        private TzZoneInfo m_info;

        static TzTimeZone()
        {
			// init timezones from bundled zoneinfo
			InitTimeZones("zoneinfo");
        }

		/**
		 * Override TzTimeZone.GetTimeZone monstrocity that adds 5-6 seconds
		 * to static loading. Load from bundled zoneinfo distribution.
		 */
		public static void InitTimeZones(string file)
		{
			// Extract line array from bundled zoneinfo
			
			Assembly asm = Assembly.GetExecutingAssembly();
			Stream stream = asm.GetManifestResourceStream(file);
			
			int len = (int) stream.Length; // int overflow in [1,2]gb for utf-8 stream
			byte[] buffer = new byte[len];
			stream.Read(buffer, 0, len);
			
			buffer = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, buffer);
			string s = Encoding.UTF8.GetString(buffer, 0, len);
			string[] ss = s.Split(new char[] {'\n'}); // Python generated with unix linebreaks. obviously.
			
			// init datastructures for ReadDatabase call
			List<TzDatabase.TzRule> rule_list = new List<TzDatabase.TzRule>();
			List<TzDatabase.TzZone> zone_list = new List<TzDatabase.TzZone>();
			List<string[]> links_list = new List<string[]>();

			// Read database
			TzDatabase.ReadDatabaseFile(ss, rule_list, zone_list, links_list);

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
				if (!s_zones.ContainsKey(zone_name)) {
					s_zones.Add(zone_name, tzzone);
				}
				
				// add links
				foreach (string[] link in links_list) {
					string from = link[1];
					string to = link[2];
					if ((from == zone_name) && (!s_zones.ContainsKey(to))) {
						tzzone = new TzTimeZone.TzZoneInfo(to, zones, rules);
						s_zones.Add(to, tzzone);
					}
				}
			}
		}


        /// <summary>
        /// 
        /// </summary>
        public static ReadOnlyDictionary<string, TzZoneInfo> Zones
        {
            get
            {
                return new ReadOnlyDictionary<string,TzZoneInfo>(s_zones);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ReadOnlyCollection<TzZoneInfo> ZoneList
        {
            get
            {
                lock (s_zoneListLock)
                {
                    string[] allZonesNames = AllZoneNames;

                    List<TzZoneInfo> list = new List<TzZoneInfo>();
                    foreach (string zoneName in allZonesNames)
                    {
                        list.Add(s_zones[zoneName]);
                    }
                    s_zoneList = new ReadOnlyCollection<TzZoneInfo>(list);
                }
                return s_zoneList;
            }
        }

        /// <summary>
        /// Gets all zone names.
        /// </summary>
        /// <value>All zone names.</value>
        public static string[] AllZoneNames
        {
            get
            {
                lock (s_allZonesLock)
                {
                    List<string> zoneNames = new List<string>(s_zones.Keys);
                    zoneNames.Sort();
                    s_allZoneNames = zoneNames.ToArray();
                }
                return s_allZoneNames;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzTimeZone"/> class.
        /// </summary>
        public TzTimeZone()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TzTimeZone"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        public TzTimeZone(TzZoneInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            m_info = info;
        }

        /// <summary>
        /// Gets the info.
        /// </summary>
        /// <value>The info.</value>
        protected TzZoneInfo Info
        {
            get
            {
                return m_info;
            }
        }

        /// <summary>
        /// This will always return the unique zone name
        /// for this zone as specified in the Olson time zone database.
        /// This zone name does not contain spaces. To retrieve the
        /// abbreviated form which is sensitive to daylight savings time,
        /// use the GetAbbreviation method. The
        /// GetAbbreviation method will return an abbreviation such,
        /// for example, EDT or EST, in the Eastern time zone in the United
        /// States, depending on the point in time given to it.
        /// </summary>
        /// <value></value>
        /// <returns>The standard time zone name.</returns>
        /// <exception cref="T:System.ArgumentNullException">Attempted to set this property to null. </exception>
        public override string StandardName
        {
            get
            {
                return m_info.ZoneName;
            }
        }

        /// <summary>
        /// Gets the historical information, including zones and rules
        /// for this time zone.
        /// </summary>
        /// <value>The historical info.</value>
        public virtual TzZoneInfo HistoricalData
        {
            get
            {
                return m_info;
            }
        }

        /// <summary>
        /// This will always return the <see cref="StandardName"/> name
        /// for this zone as specified in the Olson time zone database.
        /// This zone name does not contain spaces. To retrieve the
        /// abbreviated form which is sensitive to daylight savings time,
        /// use the GetAbbreviation method. The
        /// GetAbbreviation method will return an abbreviation such,
        /// for example, EDT or EST, in the Eastern time zone in the United
        /// States, depending on the point in time given to it.
        /// </summary>
        /// <value></value>
        /// <returns>The daylight saving time zone name.</returns>
        public override string DaylightName
        {
            get
            {
                return StandardName;
            }
        }

        /// <summary>
        /// Returns the daylight saving time period for a particular year. By default,
        /// returns the start and end DateTimes in Local format. Use the overload
        /// of GetDaylightChanges to provide the inflection point Kind (such as Utc)
        /// to do the proper conversion.
        /// </summary>
        /// <param name="year">The year to which the daylight saving time period applies.</param>
        /// <returns>
        /// A <see cref="T:System.Globalization.DaylightTime"></see> instance containing the start and end date for daylight saving time in year.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">year is less than 1 or greater than 9999. </exception>
        public override DaylightTime GetDaylightChanges(int year)
        {
            return GetDaylightChanges(year, DateTimeKind.Local);
        }

        /// <summary>
        /// Returns the daylight saving time period for a particular year.
        /// </summary>
        /// <param name="year">The year to which the daylight saving time period applies.</param>
        /// <param name="inflectionKind">Kind of the inflection.</param>
        /// <returns>
        /// A <see cref="T:System.Globalization.DaylightTime"></see> instance containing the start and end date for daylight saving time in year.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">year is less than 1 or greater than 9999. </exception>
        public virtual DaylightTime GetDaylightChanges(int year, DateTimeKind inflectionKind)
        {
            if (year < 1 || year > 9999)
            {
                throw new ArgumentOutOfRangeException("year");
            }

            DaylightTime result = null;

            DateTime point = new DateTime(year, 1, 1);
            TzDatabase.TzZone targetZone = FindZone(point);

            PublicDomain.TzDatabase.TzRule one;
            PublicDomain.TzDatabase.TzRule two;
            GetDaylightChangeRules(ref point, targetZone, inflectionKind, out one, out two, out result);

            return result;
        }

        private void GetDaylightChangeRules(ref DateTime time, TzDatabase.TzZone targetZone, DateTimeKind inflectionKind, out TzDatabase.TzRule one, out TzDatabase.TzRule two, out DaylightTime daylightTime)
        {
            one = two = null;
            daylightTime = null;

            // First, figure out which zone we're in

            if (targetZone != null)
            {
                // The zone might not have any daylight savings time rules
                if (targetZone.RuleName != TzDatabase.NotApplicableValue)
                {
                    int index = FindRuleIndex(targetZone, time);
                    if (index != -1)
                    {
                        one = m_info.Rules[index];

                        two = GetCompanionRule(targetZone, one, index, time);

                        if (one != null && two != null)
                        {
                            DateTime from = one.GetDateTime(time.Year, targetZone.UtcOffset, two);
                            DateTime to = two.GetDateTime(time.Year, targetZone.UtcOffset, one);

                            if (from > to && Info.IsLatitudeNorth)
                            {
                                DateTime swap = to;
                                to = from;
                                from = swap;

                                TzDatabase.TzRule swapRule = one;
                                one = two;
                                two = swapRule;
                            }
                            else if (from < to && !Info.IsLatitudeNorth)
                            {
                                DateTime swap = to;
                                to = from;
                                from = swap;

                                TzDatabase.TzRule swapRule = one;
                                one = two;
                                two = swapRule;
                            }

                            TimeSpan delta = one.SaveTime;
                            if (delta.Ticks == 0)
                            {
                                delta = two.SaveTime;
                            }

                            daylightTime = new DaylightTime(from, to, delta);
                        }
                    }
                }
            }
        }

        private PublicDomain.TzDatabase.TzRule GetCompanionRule(TzDatabase.TzZone zone, PublicDomain.TzDatabase.TzRule rule, int ruleIndex, DateTime point)
        {
            TzDatabase.TzRule result = null;
            int companionIndex = FindRuleIndex(zone, point, rule.Modifier, rule.Modifier == TzDatabase.NotApplicableValue ? ruleIndex : -1, false);
            if (companionIndex != -1)
            {
                result = m_info.Rules[companionIndex];
            }
            return result;
        }

        /// <summary>
        /// Finds the zone.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public TzDatabase.TzZone FindZone(DateTime point)
        {
            // Find the zone for the span of the specified DateTime
            // We iterate backwards since it is more likely that a date
            // is closer to the present
            int length = m_info.Zones.Count;

            if (length == 0)
            {
                throw new Exception("No zones found");
            }

            // We go down to the second element
            int i;
            bool found = false;
            for (i = length - 1; i >= 1; i--)
            {
                TzDatabase.TzZone curZone = m_info.Zones[i];
                TzDatabase.TzZone prevZone = m_info.Zones[i - 1];
                DateTime untilDateTime = curZone.GetUntilDateTime();
                DateTime prevUntilDateTime = prevZone.GetUntilDateTime();

                // untilDateTime states that this zone was active from
                // the end of the previous zone until this specific instant.
                if (point < untilDateTime && point >= prevUntilDateTime)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // It is either in the first zone or the last zone
                PublicDomain.TzDatabase.TzZone firstZone = m_info.Zones[0];
                DateTime untilDateTime = firstZone.GetUntilDateTime();
                if (point < untilDateTime)
                {
                    // It is the first zone
                    i = 0;
                }
                else
                {
                    i = length - 1;
                }
            }

            return m_info.Zones[i];
        }

        /// <summary>
        /// Finds the rule.
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public TzDatabase.TzRule FindRule(TzDatabase.TzZone zone, DateTime point)
        {
            int ruleIndex = FindRuleIndex(zone, point);
            return ruleIndex == -1 ? null : m_info.Rules[ruleIndex];
        }

        /// <summary>
        /// Finds the index of the rule.
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        public int FindRuleIndex(TzDatabase.TzZone zone, DateTime point)
        {
            return FindRuleIndex(zone, point, null, -1, true);
        }

        /// <summary>
        /// Finds the rule.
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <param name="point">The point.</param>
        /// <param name="avoidModifier">The avoid modifier.</param>
        /// <param name="avoidIndex">Index of the avoid.</param>
        /// <param name="exactComparison">if set to <c>true</c> [exact comparison].</param>
        /// <returns></returns>
        public int FindRuleIndex(TzDatabase.TzZone zone, DateTime point, string avoidModifier, int avoidIndex, bool exactComparison)
        {
            // Now, we have the zone, we can start to figure out the amount
            // of time to add to UTC to get standard time

            // Get the standard time from the UTC Offset for the zone

            // Use the rules to see if we need to apply daylight savings time
            // "If this field is - then standard time
            // always applies in the time zone."
            string ruleName = zone.RuleName;

            int curRuleIndex = -1;

            if (ruleName != TzDatabase.NotApplicableValue)
            {
                // Iterate over the rules to find the
                // applicable rule
                int rulesLength = m_info.Rules.Count;
                if (rulesLength == 0 && !string.IsNullOrEmpty(ruleName))
                {
                    throw new BaseException("No rules found for rule name {0}", ruleName);
                }

                for (int j = 0; j < rulesLength; j++)
                {
                    PublicDomain.TzDatabase.TzRule r = m_info.Rules[j];

                    if (r.RuleName == ruleName && (avoidIndex == -1 || (avoidIndex != -1 && avoidIndex != j)))
                    {
                        curRuleIndex = j;
                        if ((exactComparison && point >= r.GetFromDateTime(zone.UtcOffset) && point < r.GetToDateTime(zone.UtcOffset)) ||
                            (!exactComparison && point.Year >= r.FromYear && point.Year <= r.ToYear))
                        {
                            if (avoidModifier == null || avoidModifier != r.Modifier || avoidIndex != -1)
                            {
                                break;
                            }
                        }
                    }
                }

                if (curRuleIndex == -1)
                {
                    throw new TzException("Could not find rule");
                }
            }
            return curRuleIndex;
        }

        /// <summary>
        /// Returns the coordinated universal time (UTC) offset for the specified local time.
        /// </summary>
        /// <param name="time">The local date and time.</param>
        /// <returns>
        /// The UTC offset from time, measured in ticks.
        /// </returns>
        public override TimeSpan GetUtcOffset(DateTime time)
        {
            if (time.Kind == DateTimeKind.Unspecified && TreatUnspecifiedKindAsLocal)
            {
                time = new DateTime(time.Ticks, DateTimeKind.Local);
            }

            TimeSpan result = TimeSpan.Zero;
            PublicDomain.TzDatabase.TzZone zone = FindZone(time);
            if (zone != null)
            {
                result += zone.UtcOffset;

                if (time.Kind == DateTimeKind.Utc)
                {
                    time += zone.UtcOffset;
                }

                PrepOffset(ref time, ref result, zone);
            }

            return result;
        }

        private void PrepOffset(ref DateTime time, ref TimeSpan result, PublicDomain.TzDatabase.TzZone zone)
        {
            // First, figure out which zone we're in
            DateTime point = new DateTime(time.Year, 1, 1);

            PublicDomain.TzDatabase.TzRule one;
            PublicDomain.TzDatabase.TzRule two;
            DaylightTime daylightTime;

            GetDaylightChangeRules(ref point, zone, DateTimeKind.Local, out one, out two, out daylightTime);

            if (daylightTime != null && daylightTime.Start < daylightTime.End && time >= daylightTime.Start && time < daylightTime.End)
            {
                result += one.SaveTime;
            }
            else if (daylightTime != null && daylightTime.Start > daylightTime.End && (time <= daylightTime.Start || time > daylightTime.End))
            {
                result += one.SaveTime;
            }
        }

        /// <summary>
        /// Gets the abbreviation for this time zone at
        /// the current date and time. This is sensitive
        /// to daylight savings time. For example, in the United States,
        /// in the Eastern time zone,
        /// this will return either EDT or EST, depending
        /// on whether or not the current time point is
        /// in the daylight savings time period or not.
        /// </summary>
        /// <returns></returns>
        public virtual string GetAbbreviation()
        {
            return GetAbbreviation(DateTime.Now);
        }

        /// <summary>
        /// Gets the abbreviation for this time zone at
        /// the point of <paramref name="time"/>. This is sensitive
        /// to daylight savings time. For example, in the United States,
        /// in the Eastern time zone,
        /// this will return either EDT or EST, depending
        /// on whether or not the point of <paramref name="time"/> is
        /// in the daylight savings time period or not.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public virtual string GetAbbreviation(DateTime time)
        {
            PublicDomain.TzDatabase.TzZone zone = FindZone(time);
            int rule1Index = FindRuleIndex(zone, time);
            PublicDomain.TzDatabase.TzRule rule1 = (rule1Index == -1 ? null : m_info.Rules[rule1Index]);
            if (rule1 != null)
            {
                PublicDomain.TzDatabase.TzRule modrule = rule1;
                PublicDomain.TzDatabase.TzRule rule2 = GetCompanionRule(zone, rule1, rule1Index, time);
                if (rule2 != null)
                {
                    DateTime x = rule1.GetDateTime(time.Year, zone.UtcOffset);
                    DateTime y = rule2.GetDateTime(time.Year, zone.UtcOffset);

                    if (x > y)
                    {
                        DateTime swap = y;
                        y = x;
                        x = swap;

                        PublicDomain.TzDatabase.TzRule swapRule = rule1;
                        rule1 = rule2;
                        rule2 = swapRule;
                    }

                    if (time < x)
                    {
                        modrule = rule2;
                    }
                    else if (time >= x && time < y)
                    {
                    }
                    else
                    {
                        modrule = rule2;
                    }
                }

                return zone.FormatModifier(modrule);
            }
            else if (zone != null && !string.IsNullOrEmpty(zone.Format) && zone.Format != TzDatabase.NotApplicableValue)
            {
                return zone.Format;
            }
            return StandardName;
        }

        /// <summary>
        /// Returns a value indicating whether the specified date and time is within a daylight saving time period.
        /// </summary>
        /// <param name="time">A date and time.</param>
        /// <returns>
        /// true if time is in a daylight saving time period; false otherwise, or if time is null.
        /// </returns>
        public override bool IsDaylightSavingTime(DateTime time)
        {
            DaylightTime daylightTime = GetDaylightChanges(time.Year);
            if (daylightTime != null)
            {
                if (daylightTime.Start <= daylightTime.End && time < daylightTime.Start)
                {
                    return false;
                }
                else if (time >= daylightTime.Start && time < daylightTime.End)
                {
                    return true;
                }
                else if (daylightTime.Start >= daylightTime.End && (time < daylightTime.End || time >= daylightTime.Start))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the local time that corresponds to a specified coordinated universal time (UTC).
        /// The <paramref name="time"/> parameters must have its Kind specifically set to UTC,
        /// otherwise this code will treat the time as a local DateTime and simply return the same
        /// value. It is important to note that normal usage of DateTime's, such as DateTime.Parse
        /// will create a DateTime with Kind Unspecified, which is treated as Local, therefore,
        /// if you are manually creating UTC DateTimes to pass to this function, you must explicitly
        /// create one with the UTC Kind. If you already have a DateTime (from a method like Parse)
        /// with a Local or Unspecified Kind, you can create one with UTC, simply with
        /// new DateTime(oldDateTime.Ticks, DateTimeKind.Utc)
        /// </summary>
        /// <param name="time">A UTC time. See summary of method.</param>
        /// <returns>
        /// A <see cref="T:System.DateTime"></see> instance whose value is the local time that corresponds to time.
        /// </returns>
        public override DateTime ToLocalTime(DateTime time)
        {
            switch (time.Kind)
            {
                case DateTimeKind.Local:
                    // Nothing necessary to change
                    return time;
                case DateTimeKind.Unspecified:
                    if (TreatUnspecifiedKindAsLocal)
                    {
                        // Nothing necessary to change
                        return time;
                    }
                    else
                    {
                        throw new ArgumentException("unspecified kind");
                    }
                case DateTimeKind.Utc:
                    return GetLocalTimeFromUniversalTime(time);
                default:
                    throw new NotImplementedException(time.Kind.ToString());
            }
        }

        /// <summary>
        /// Returns the coordinated universal time (UTC) that corresponds to a specified local time.
        /// </summary>
        /// <param name="time">The local date and time.</param>
        /// <returns>
        /// A <see cref="T:System.DateTime"></see> instance whose value is the UTC time that corresponds to time.
        /// </returns>
        public override DateTime ToUniversalTime(DateTime time)
        {
            switch (time.Kind)
            {
                case DateTimeKind.Local:
                    return GetUniversalTimeFromLocalTime(time);
                case DateTimeKind.Unspecified:
                    if (TreatUnspecifiedKindAsLocal)
                    {
                        return GetUniversalTimeFromLocalTime(time);
                    }
                    else
                    {
                        throw new ArgumentException("unspecified kind");
                    }
                case DateTimeKind.Utc:
                    // It is already UTC
                    return time;
                default:
                    throw new NotImplementedException(time.Kind.ToString());
            }
        }

        private DateTime GetLocalTimeFromUniversalTime(DateTime time)
        {
            time += GetUtcOffset(time);

            return new DateTime(time.Ticks, DateTimeKind.Local);
        }

        private DateTime GetUniversalTimeFromLocalTime(DateTime time)
        {
            if (time.Kind == DateTimeKind.Unspecified && TreatUnspecifiedKindAsLocal)
            {
                time = new DateTime(time.Ticks, DateTimeKind.Local);
            }

            time -= GetUtcOffset(time);

            return new DateTime(time.Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets the current time in this time zone.
        /// </summary>
        /// <value>The now.</value>
        public virtual TzDateTime Now
        {
            get
            {
                return new TzDateTime(DateTime.UtcNow, this);
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return StandardName;
        }

        /// <summary>
        /// Gets a new object representing the time zone with the
        /// name <paramref name="tzName"/> or null if none can be found.
        /// These <see cref="PublicDomain.TzTimeZone"/> instances are
        /// not cached, so constantly calling this method will continuously
        /// create new <see cref="PublicDomain.TzTimeZone"/> objects.
        /// </summary>
        /// <param name="tzName">Name of the time zone.</param>
        /// <returns>null if no time zone can by found or a TzTimeZone object</returns>
        public static TzTimeZone GetTimeZone(string tzName)
        {
            TzTimeZone result = null;
            TzZoneInfo zoneInfo;

            if (!string.IsNullOrEmpty(tzName))
            {
                try
                {
                    s_zonesLock.AcquireReaderLock(-1);
                    
                    if (Zones.TryGetValue(tzName, out zoneInfo))
                    {
                        result = new TzTimeZone(zoneInfo);
                    }
                    else
                    {
                        LockCookie writerCookie = s_zonesLock.UpgradeToWriterLock(-1);

                        try
                        {
                            if (Zones.TryGetValue(tzName, out zoneInfo))
                            {
                                result = new TzTimeZone(zoneInfo);
                            }
                            else
                            {

                                if (zoneInfo != null)
                                {
                                    // Found the time zone
                                    result = new TzTimeZone(zoneInfo);
                                    s_zones[tzName] = zoneInfo;
                                }
                            }
                        }
                        finally
                        {
                            s_zonesLock.DowngradeFromWriterLock(ref writerCookie);
                        }
                    }
                }
                finally
                {
                    s_zonesLock.ReleaseReaderLock();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the time zone by offset.
        /// </summary>
        /// <param name="utcOffsetTime">The utc offset time.</param>
        /// <returns></returns>
        public static TzTimeZone GetTimeZoneByOffset(string utcOffsetTime)
        {
            return GetTimeZoneByOffset(DateTimeUtlities.ParseTimeSpan(utcOffsetTime, DateTimeUtlities.TimeSpanAssumption.Hours));
        }

        /// <summary>
        /// Gets the time zone by offset.
        /// </summary>
        /// <param name="utcOffsetTime">The utc offset time.</param>
        /// <returns></returns>
        public static TzTimeZone GetTimeZoneByOffset(TimeSpan utcOffsetTime)
        {
            if (utcOffsetTime.Equals(TimeSpan.Zero))
            {
                return TzTimeZone.ZoneUTC;
            }
            else
            {
                // We pick the most "well-known" time zone in the set for each offset
                string zoneName;
                if (s_mainTimeZones.TryGetValue(DateTimeUtlities.ConvertTimeSpanToDouble(utcOffsetTime), out zoneName))
                {
                    return GetTimeZone(zoneName);
                }
                else
                {
                    // TODO do a manual search
                }
            }
            throw new TzDatabase.TzException("Cannot find time zone with UTC offset {0}.", utcOffsetTime);
        }

        /// <summary>
        ///     Returns a value indicating whether the specified date and time is within
        ///     the specified daylight saving time period.
        /// </summary>
        /// <param name="time">A date and time.</param>
        /// <param name="daylightTimes">A daylight saving time period.</param>
        /// <returns>
        /// 	true if time is in daylightTimes; otherwise, false.
        /// </returns>
        public static new bool IsDaylightSavingTime(DateTime time, DaylightTime daylightTimes)
        {
            return TimeZone.IsDaylightSavingTime(time, daylightTimes);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            TzTimeZone cmp = obj as TzTimeZone;
            if (cmp != null)
            {
                return StandardName.Equals(cmp.StandardName);
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return StandardName.GetHashCode();
        }
		
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
		
        /// <summary>
        /// Represents a time zone, with all of its transitions and rules.
        /// </summary>
        [Serializable]
        public class TzZoneInfo
        {
            private ReadOnlyCollection<PublicDomain.TzDatabase.TzRule> m_rules;

            private ReadOnlyCollection<PublicDomain.TzDatabase.TzZone> m_zones;

            private string m_zoneName;

            private bool m_isLatitudeNorth = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzZone"/> class.
            /// </summary>
            /// <param name="zoneName">Name of the zone.</param>
            public TzZoneInfo(string zoneName)
                : this(zoneName, null, null, true)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzZoneInfo"/> class.
            /// </summary>
            /// <param name="zoneName">Name of the zone.</param>
            /// <param name="zones">The zones.</param>
            /// <param name="rules">The rules.</param>
            [Obsolete]
            public TzZoneInfo(string zoneName, List<PublicDomain.TzDatabase.TzZone> zones, List<PublicDomain.TzDatabase.TzRule> rules)
                : this(zoneName, zones, rules, true)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PublicDomain.TzDatabase.TzZone"/> class.
            /// </summary>
            /// <param name="zoneName">Name of the zone.</param>
            /// <param name="zones">The zones.</param>
            /// <param name="rules">The rules.</param>
            /// <param name="isLatitudeNorth">if set to <c>true</c> [is latitude north].</param>
            public TzZoneInfo(string zoneName, List<PublicDomain.TzDatabase.TzZone> zones, List<PublicDomain.TzDatabase.TzRule> rules, bool isLatitudeNorth)
            {
                if (zones == null)
                {
                    zones = new List<TzDatabase.TzZone>();
                }
                if (rules == null)
                {
                    rules = new List<TzDatabase.TzRule>();
                }
                m_zoneName = zoneName;
                m_zones = zones.AsReadOnly();
                m_rules = rules.AsReadOnly();
                m_isLatitudeNorth = isLatitudeNorth;
            }

            /// <summary>
            /// Gets the rules.
            /// </summary>
            /// <value>The rules.</value>
            public ReadOnlyCollection<PublicDomain.TzDatabase.TzRule> Rules
            {
                get
                {
                    return m_rules;
                }
            }

            /// <summary>
            /// Gets the zones.
            /// </summary>
            /// <value>The zones.</value>
            public ReadOnlyCollection<PublicDomain.TzDatabase.TzZone> Zones
            {
                get
                {
                    return m_zones;
                }
            }

            /// <summary>
            /// Gets the name of the zone.
            /// </summary>
            /// <value>The name of the zone.</value>
            public string ZoneName
            {
                get
                {
                    return m_zoneName;
                }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return ZoneName;
            }

            /// <summary>
            /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
            /// </summary>
            /// <returns>
            /// A hash code for the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override int GetHashCode()
            {
                return ZoneName.GetHashCode();
            }

            /// <summary>
            /// Gets a value indicating whether this instance is latitude north.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is latitude north; otherwise, <c>false</c>.
            /// </value>
            public bool IsLatitudeNorth
            {
                get
                {
                    return m_isLatitudeNorth;
                }
            }

            /// <summary>
            /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
            /// <returns>
            /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                TzZoneInfo z = obj as TzZoneInfo;
                if (z != null && z.ZoneName.Equals(ZoneName))
                {
                    return true;
                }
                return base.Equals(obj);
            }

            /// <summary>
            /// Clones the specified zone name.
            /// </summary>
            /// <param name="zoneName">Name of the zone.</param>
            /// <returns></returns>
            public TzZoneInfo Clone(string zoneName)
            {
                TzZoneInfo result = (TzZoneInfo)MemberwiseClone();
                result.m_zoneName = zoneName;
                return result;
            }
        }

        /// <summary>
        /// Logical zone description taken from the zone tab file in the tz database.
        /// </summary>
        [Serializable]
        public class TzZoneDescription
        {
            private string m_twoLetterCode;
            private string m_zoneName;
            private string m_comments;

            /// <summary>
            /// Initializes a new instance of the <see cref="TzZoneDescription"/> class.
            /// </summary>
            public TzZoneDescription()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TzZoneDescription"/> class.
            /// </summary>
            /// <param name="twoLetterCode">The two letter code.</param>
            /// <param name="location">The location.</param>
            /// <param name="zoneName">Name of the zone.</param>
            /// <param name="comments">The comments.</param>
            public TzZoneDescription(string twoLetterCode, string zoneName, string comments)
            {
                m_twoLetterCode = twoLetterCode;
                m_zoneName = zoneName;
                m_comments = comments;
            }

            /// <summary>
            /// 
            /// </summary>
            public string TwoLetterCode
            {
                get
                {
                    return m_twoLetterCode;
                }
            }


            /// <summary>
            /// 
            /// </summary>
            public string ZoneName
            {
                get
                {
                    return m_zoneName;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Comments
            {
                get
                {
                    return m_comments;
                }
            }

            /// <summary>
            /// Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> containing a fully qualified type name.
            /// </returns>
            public override string ToString()
            {
                return string.Format("{0}\t{2}\t{3}", TwoLetterCode, ZoneName, Comments);
            }
        }
    }
}
