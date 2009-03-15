// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
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
				pos = new Position(lodir,
				                   lodeg, lomin, losec,
				                   ladir,
				                   ladeg, lamin, lasec);
			} catch (ArgumentException) {
				foreach (Id id in ins_render)
				{
					MarkError(registry[id]);
				}
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
			// try to instantiate type, otherwise mark inputs as erroneous
			try {
				date = new DateTime(year, month, day,
				                    hour, min, sec, DateTimeKind.Local);
			} catch (ArgumentException) {
				foreach (Id id in ins_update)
				{
					MarkError(registry[id]);
				}
			}
			
			return Controller.timezone_source.ApplyZone(tz_name, date.Value);
		}
		
		private static string GetValue(Control control)
		{
			string val = null;
			if (control is ComboBox) {
				val = ((ComboBox) control).Text;
			} else if (control is HScrollBar) {
				val = ((HScrollBar) control).Value.ToString();
			} else if (control is NumericUpDown) {
				val = ((NumericUpDown) control).Value.ToString();
			} else if (control is TextBox) {
				val = ((TextBox) control).Text;
			} else {
				throw new ArgumentException(string.Format(
					"Cannot get value of unknown control: {0}", 
					control.GetType().ToString()));
			}
			return val;
		}
	}
}