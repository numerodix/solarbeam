// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Windows.Forms;

using LibSolar.Locations;
using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Handles input validation.
	 */
	partial class Controller
	{
		private static bool validating = false;
	
		private static void ValidateLocation()
		{
			Control control_loc = registry[Id.LOCATION];
			string loc_s = GetValue(control_loc);
			
			Location loc = Controller.locations_source.GetLocation(loc_s);
			SetPosition(loc.Position);
			SetTimezone(loc.Timezone);
		}
		
		private static void ValidatePosition(PositionAxis ax)
		{
			Control control_deg = registry[Id.LONGITUDE_DEGS];
			Control control_min = registry[Id.LONGITUDE_MINS];
			Control control_sec = registry[Id.LONGITUDE_SECS];
	
			int max_deg = Position.LONDEGS_MAXVALUE;
			int min_deg = Position.LONDEGS_MINVALUE;
			int max_min = Position.LONMINS_MAXVALUE;
			int min_min = Position.LONMINS_MINVALUE;
			int max_sec = Position.LONSECS_MAXVALUE;
			int min_sec = Position.LONSECS_MINVALUE;
	
			if (ax == PositionAxis.Latitude) {
				control_deg = registry[Id.LATITUDE_DEGS];
				control_min = registry[Id.LATITUDE_MINS];
				control_sec = registry[Id.LATITUDE_SECS];
	
				max_deg = Position.LATDEGS_MAXVALUE;
				min_deg = Position.LATDEGS_MINVALUE;
				max_min = Position.LATMINS_MAXVALUE;
				min_min = Position.LATMINS_MINVALUE;
				max_sec = Position.LATSECS_MAXVALUE;
				min_sec = Position.LATSECS_MINVALUE;
			}
	
			int deg = GetInt(GetValue(control_deg));
			int min = GetInt(GetValue(control_min));
			int sec = GetInt(GetValue(control_sec));
	
			if (sec > max_sec) {
				sec = min_sec;
				min += 1;
			} else if (sec < min_sec) {
				if ((min > min_min) 
						|| (deg > min_deg)) {
					sec = max_sec;
					min -= 1;
				} else {
					sec = min_sec;
				}
			}
	
			if (min > max_min) {
				min = min_min;
				deg += 1;
			} else if (min < min_min) {
				if (deg > min_deg) {
					min = max_min;
					deg -= 1;
				} else {
					min = min_min;
				}
			}
	
			if (deg >= max_deg) {
				deg = max_deg;
				min = min_min;
				sec = min_sec;
			} else if (deg < min_deg) {
				deg = min_deg;
				min = min_min;
				sec = min_sec;
			}
	
			SetValue(control_deg, deg);
			SetValue(control_min, min);
			SetValue(control_sec, sec);
		}
		
		private static void ValidateTimezoneOffset()
		{
			Control control_tzoff = registry[Id.TIMEZONE_OFFSET];
			ComboBox control_tzname = (ComboBox) registry[Id.TIMEZONE_NAME];
			
			string tzoff_val = GetValue(control_tzoff);
			string[] zones = Controller.timezone_source.GetTimezones(tzoff_val);
			
			control_tzname.Items.Clear();
			control_tzname.Items.AddRange(zones);
			if (zones.Length > 0) {
				control_tzname.Text = zones[0];
				Enable(control_tzname);
			} else {
				Disable(control_tzname);
				control_tzname.Enabled = false;
			}
		}
		
		private static void ValidateDate()
		{
			Control control_year = registry[Id.DATE_YEAR];
			Control control_month = registry[Id.DATE_MONTH];
			Control control_day = registry[Id.DATE_DAY];
	
			int year = GetInt(GetValue(control_year));
			int month = GetInt(GetValue(control_month));
			int day = GetInt(GetValue(control_day));
	
			int prev_month_year = year;
			int prev_month = month - 1;
			if (prev_month < 1) {
				prev_month = 12;
				prev_month_year -= 1;
			}
	
			int prev_day_max = GetDayMax(prev_month, prev_month_year);
			int day_max = GetDayMax(month, year);
	
			if (day > day_max) {
				day = day - day_max;
				month += 1;
			} else if (day < 1) {
				day = prev_day_max - day;
				month -= 1;
			}
			if (month > 12) {
				month = month - 12;
				year += 1;
			} else if (month < 1) {
				month = 12 - month;
				year -= 1;
			}
	    
			SetValue(control_year, year);
			SetValue(control_month, month);
			SetValue(control_day, day);
		}
	
		private static void ValidateTime()
		{
			Control control_day = registry[Id.DATE_DAY];
			Control control_hour = registry[Id.TIME_HOUR];
			Control control_min = registry[Id.TIME_MINUTE];
			Control control_sec = registry[Id.TIME_SECOND];
	
			int max_hour = UTCDate.HOUR_MAXVALUE;
			int min_hour = UTCDate.HOUR_MINVALUE;
			int max_min = UTCDate.MINUTE_MAXVALUE;
			int min_min = UTCDate.MINUTE_MINVALUE;
			int max_sec = UTCDate.SECOND_MAXVALUE;
			int min_sec = UTCDate.SECOND_MINVALUE;
	
			int day = GetInt(GetValue(control_day));
			int hour = GetInt(GetValue(control_hour));
			int min = GetInt(GetValue(control_min));
			int sec = GetInt(GetValue(control_sec));
	
			if (sec > max_sec) {
				sec = min_sec;
				min += 1;
			} else if (sec < min_sec) {
				sec = max_sec;
				min -= 1;
			}
	
			if (min > max_min) {
				min = min_min;
				hour += 1;
			} else if (min < min_min) {
				min = max_min;
				hour -= 1;
			}
	
			if (hour > max_hour) {
				hour = min_hour;
				min = min_min;
				sec = min_sec;
				day += 1;
			} else if (hour < min_hour) {
				hour = max_hour;
				day -= 1;
			}
	
			SetValue(control_day, day);
			SetValue(control_hour, hour);
			SetValue(control_min, min);
			SetValue(control_sec, sec);
	
			ValidateDate();
		}
	
		private static int GetDayMax(int month, int year)
		{
			int day_max = 31;
			if (month == 2) {
				if (DateTime.IsLeapYear(year)) {
					day_max = 29;
				} else {
					day_max = 28;
				}
			} else if ((month == 4) || (month == 6) || (month == 9) || (month == 11)) {
				day_max = 30;
			}
			return day_max;
		}
	
		private static int GetInt(object s)
		{
			return Convert.ToInt32(s);
		}
	}
}