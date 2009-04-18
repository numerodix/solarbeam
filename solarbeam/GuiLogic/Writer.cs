// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
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
	
			SetDateTime(dt);
			SetLocation("Equator");
			
			SetValue(registry[Id.IMAGE_SIZE], 500);
		}
		
		private static void SetLocation(string location)
		{
			ComboBox control = (ComboBox) registry[Id.LOCATION];
			// only set name if it's on the list
			if (control.Items.Contains(location)) {
				SetValue(control, location);
			} else {
				// don't select index 0, will not trigger value change
				control.SelectedIndex = control.Items.Count / 2;
			}
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
			string tz_off = Controller.TimezoneSource.GetOffsetName(tz_name);
			SetValue(registry[Id.TIMEZONE_OFFSET], tz_off);
			ValidateTimezoneOffset(); // update timezone name list
			SetValue(registry[Id.TIMEZONE_NAME], tz_name);
		}
	
		private static void SetDateTime(DateTime dt_loc)
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
			SolarTimes st_ss = Orbit.CalcSolarTimes(pos, dt);
			
			SetValue(registry[Id.ELEVATION], FormatAngle(sp.Elevation));
			SetValue(registry[Id.AZIMUTH], FormatAngle(sp.Azimuth));
			
			SetValue(registry[Id.SOLAR_NOON], FormatTime(st_ss.Noon));
			
			string sunrise = "##:##";
			string sunset = "##:##";
			string daylength = FormatDayLength(st_ss, sp);
			if (st_ss.Sunrise.HasValue) {
				sunrise = FormatTime(st_ss.Sunrise.Value);
			}
			if (st_ss.Sunset.HasValue) {
				sunset = FormatTime(st_ss.Sunset.Value);
			}
			string riseset = string.Format("{0} - {1}  ({2})",
			                               sunrise, sunset, daylength);
			SetValue(registry[Id.SUNRISESUNSET], riseset);
			
			SolarTimes st_dd = PointFinder.FindDawnDusk(pos, dt);
			string dawn = "##:##";
			string dusk = "##:##";
			string daylength2 = FormatDayLength(st_dd, sp);
			if (st_dd.Sunrise.HasValue) {
				dawn = FormatTime(st_dd.Sunrise.Value);
			}
			if (st_dd.Sunset.HasValue) {
				dusk = FormatTime(st_dd.Sunset.Value);
			}
			string dawndusk = string.Format("{0} - {1}  ({2})",
			                               dawn, dusk, daylength2);
			SetValue(registry[Id.DAWNDUSK], dawndusk);
		}
		
		public static void SetImage(Component control, string img)
		{
			if (control is Label) {
				((Label) control).Image = new Bitmap(Controller.AsmInfo.GetResource(img));
			}
		}
		
		public static void UpdateTooltip(Component control, string tip)
		{
			Id id = reg_rev[control];
			tooltips[id].SetToolTip((Control) control, tip);
		}
	
		public static void SetValue(Component control, object val)
		{
			if (control is CheckBox) {
				((CheckBox) control).Checked = GetBool(val);
			} else if (control is ComboBox) {
				((ComboBox) control).Text = (string) val;
			} else if (control is NumericUpDown) {
				((NumericUpDown) control).Value = GetInt(val);
			} else if (control is TextBox) {
				((TextBox) control).Text = (string) val;
			}
		}
		
		public static void InitControl(Component control)
		{
			// form has no values yet, don't validate this input
			validate_lock = true;
			
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
			
			// reactivate validation
			validate_lock = false;
		}
	}
}
