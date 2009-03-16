// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Windows.Forms;

using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Set widget values.
	 */
	partial class Controller
	{
		public static void InitForm()
		{
			// init current time and date
			DateTime dt = DateTime.Now;
	
			SetDate(dt);
			SetLocation("Trondheim");
		}
		
		private static void SetLocation(string location)
		{
			SetValue(registry[Id.LOCATION], location);
		}
		
		private static void SetPosition(Position pos)
		{
			SetValue(registry[Id.LATITUDE_DEGS], pos.LatitudeDegree.Deg);
			SetValue(registry[Id.LATITUDE_MINS], pos.LatitudeDegree.Min);
			SetValue(registry[Id.LATITUDE_SECS], pos.LatitudeDegree.Sec);
			string londir = pos.LatitudeDegree.Direction.ToString();
			SetValue(registry[Id.LATITUDE_DIRECTION], londir);
	
			SetValue(registry[Id.LONGITUDE_DEGS], pos.LongitudeDegree.Deg);
			SetValue(registry[Id.LONGITUDE_MINS], pos.LongitudeDegree.Min);
			SetValue(registry[Id.LONGITUDE_SECS], pos.LongitudeDegree.Sec);
			string latdir = pos.LongitudeDegree.Direction.ToString();
			SetValue(registry[Id.LONGITUDE_DIRECTION], latdir);
		}
	  
		private static void SetTimezone(string tz_name)
		{
			string tz_off = Controller.timezone_source.GetOffsetName(tz_name);
			SetValue(registry[Id.TIMEZONE_OFFSET], tz_off);
			ValidateTimezoneOffset(); // update timezone name list
			SetValue(registry[Id.TIMEZONE_NAME], tz_name);
		}
	
		private static void SetDate(DateTime dt_loc)
		{
			SetValue(registry[Id.TIME_SECOND], dt_loc.Second);
			SetValue(registry[Id.TIME_MINUTE], dt_loc.Minute);
			SetValue(registry[Id.TIME_HOUR], dt_loc.Hour);
			SetValue(registry[Id.DATE_DAY], dt_loc.Day);
			SetValue(registry[Id.DATE_MONTH], dt_loc.Month);
			SetValue(registry[Id.DATE_YEAR], dt_loc.Year);
		}
			
		private static void SetOutputs(Position pos, UTCDate dt)
		{
			SolarPosition sp = Orbit.CalcSolarPosition(pos, dt);
			SolarTimes st = Orbit.CalcSolarTimes(pos, dt);
			
			SetValue(registry[Id.ELEVATION], FormatAngle(sp.Elevation));
			SetValue(registry[Id.AZIMUTH], FormatAngle(sp.Azimuth));
			
			if (st.Sunrise.HasValue) {
				SetValue(registry[Id.SUNRISE], FormatTime(st.Sunrise.Value));
			} else {
				SetValue(registry[Id.SUNRISE], "none");
			}
			if (st.Sunset.HasValue) {
				SetValue(registry[Id.SUNSET], FormatTime(st.Sunset.Value));
			} else {
				SetValue(registry[Id.SUNSET], "none");
			}
			SetValue(registry[Id.SOLAR_NOON], FormatTime(st.Noon));
			SetValue(registry[Id.DAY_LENGTH], FormatDayLength(st, sp));
		}
	
		public static void SetValue(Control control, object val)
		{
			//Console.WriteLine("Set {0} : {1}", reg_rev[control], val);
			if (control is ComboBox) {
				((ComboBox) control).Text = (string) val;
			} else if (control is NumericUpDown) {
				((NumericUpDown) control).Value = GetInt(val);
			} else if (control is TextBox) {
				((TextBox) control).Text = (string) val;
			} else {
				throw new ArgumentException(string.Format(
					"Cannot set value to unknown control: {0}", 
					control.GetType().ToString()));
			}
		}
		
		public static void InitControl(Control control)
		{
			validating = true;
			if (control is ComboBox) {
				((ComboBox) control).SelectedIndex = 0;
				cache[reg_rev[control]] = String.Format("{0}", 0);
			} else if (control is NumericUpDown) {
				NumericUpDown num = ((NumericUpDown) control);
				int val = (int) (num.Maximum - num.Minimum) / 2;
				num.Value = val;
				cache[reg_rev[control]] = String.Format("{0}", val);
			} else if (control is TextBox) {
				((TextBox) control).Text = String.Empty;
				cache[reg_rev[control]] = String.Empty;
			}
			validating = false;
			Console.WriteLine("Set {0} : {1}", reg_rev[control], GetValue(control));
		}
	}
}
