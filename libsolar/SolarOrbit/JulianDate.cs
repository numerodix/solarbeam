// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.SolarOrbit
{
	class JulianDate
	{
		/**
		 * Finds julian day from calendar day.
		 * Number is returned for start of day.
		 */
		private static double CalcJulianDay(int year, int month, int day)
		{
			if (month <= 2) {
				year -= 1;
				month += 12;
			}

			double a = Math.Floor(year / 100.0);
			double b = 2.0 - a + Math.Floor(a / 4.0);

			return (Math.Floor( 365.25 * (year + 4716.0) )
				+ Math.Floor( 30.6001 * (month + 1.0) )
					+ day
					+ b
					- 1524.5);
		}

		/**
		 * Convert julian century to julian day.
		 */
		public static double CalcJulianDay(double julian_cent)
		{
			return (julian_cent * 36525.0 + 2451545.0);
		}

		/**
		 * Convert julian day to julian century.
		 */
		public static double CalcJulianCentury(int year, int month, int day,
											   int hour, int min, int sec)
		{
			double timenow_hours = hour + min / 60.0 + sec / 3600.0;
			double julian_day = JulianDate.CalcJulianDay(year, month, day);
			double julian_day_precise = julian_day + timenow_hours / 24.0;
			return CalcJulianCentury(julian_day_precise);
		}
		
		public static double CalcJulianCentury(double julian_day)
		{
			return (julian_day - 2451545.0) / 36525.0;
		}
	}
}
