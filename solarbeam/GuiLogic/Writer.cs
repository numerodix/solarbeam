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
			// init Trondheim
			Position pos = new Position(Position.LONGITUDE_POS,
			                            10, 23, 36,
			                            Position.LATITUDE_POS,
			                            63, 25, 47);
			// init current time and date
			UTCDate dt = UTCDate.Now(1); // timezone +1
	
			SetDate(dt);
			SetPosition(pos);
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
	
		private static void SetDate(UTCDate dt)
		{
			SetValue(registry[Id.TIME_SECOND], dt.Second);
			SetValue(registry[Id.TIME_MINUTE], dt.Minute);
			SetValue(registry[Id.TIME_HOUR], dt.Hour);
			SetValue(registry[Id.DATE_DAY], dt.Day);
			SetValue(registry[Id.DATE_MONTH], dt.Month);
			SetValue(registry[Id.DATE_YEAR], dt.Year);
			//SetValue(registry[Id.TIMEZONE_OFFSET], dt.Timezone);
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
	  
		public static void SetValue(Control control, object val)
		{
			if (control is ComboBox) {
				((ComboBox) control).Text = (string) val;
			} else if (control is HScrollBar) {
				((HScrollBar) control).Value = GetInt(val);
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
	}
}