// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Windows.Forms;

using LibSolar.Types;

namespace SolarbeamGui
{
	/**
	 * Read widget values.
	 */
	partial class Controller
	{
		private static Position ReadPosition()
		{
			string ilodir = GetValue(registry[Id.LONGITUDE_DIRECTION]);
			PositionDirection lodir =
				(PositionDirection) Enum.Parse(typeof(PositionDirection), ilodir);
			string iladir = GetValue(registry[Id.LATITUDE_DIRECTION]);
			PositionDirection ladir =
				(PositionDirection) Enum.Parse(typeof(PositionDirection), iladir);
	
			int lodeg = GetInt(GetValue(registry[Id.LONGITUDE_DEGS]));
			int lomin = GetInt(GetValue(registry[Id.LONGITUDE_MINS]));
			int losec = GetInt(GetValue(registry[Id.LONGITUDE_SECS]));
			
			int ladeg = GetInt(GetValue(registry[Id.LATITUDE_DEGS]));
			int lamin = GetInt(GetValue(registry[Id.LATITUDE_MINS]));
			int lasec = GetInt(GetValue(registry[Id.LATITUDE_SECS]));
			
			Position pos = null;
			// try to instantiate type, otherwise mark inputs as erroneous
			try {
				pos = new Position(ladir,
				                   ladeg, lamin, lasec,
				                   lodir,
				                   lodeg, lomin, losec);
			} catch (ArgumentException) {
				MarkError(ins_position);
			}
			return pos;
		}
		
		private static UTCDate? ReadDate()
		{
			string tz_name = GetValue(registry[Id.TIMEZONE_NAME]);
			
			int year = GetInt(GetValue(registry[Id.DATE_YEAR]));
			int month = GetInt(GetValue(registry[Id.DATE_MONTH]));
			int day = GetInt(GetValue(registry[Id.DATE_DAY]));
			
			int hour = GetInt(GetValue(registry[Id.TIME_HOUR]));
			int min = GetInt(GetValue(registry[Id.TIME_MINUTE]));
			int sec = GetInt(GetValue(registry[Id.TIME_SECOND]));
			
			DateTime? date = null;
			UTCDate? udt = null;
			// try to instantiate type, otherwise mark inputs as erroneous
			try {
				DateTime dt = new DateTime(year, month, day, hour, min, sec,
				                           DateTimeKind.Local);
				udt = TimezoneSource.GetUTCDate(tz_name, dt);
				TimezoneSource.Ya(dt, tz_name);
			} catch (ArgumentException) {
				MarkError(ins_timedate);
			}
			
			return udt;
		}
		
		private static string GetValue(Component control)
		{
			string val = null;
			if (control is ComboBox) {
				val = ((ComboBox) control).Text;
			} else if (control is NumericUpDown) {
				val = ((NumericUpDown) control).Value.ToString();
			} else if (control is TextBox) {
				val = ((TextBox) control).Text;
			}
			return val;
		}
	}
}
