// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Types
{
	public struct SolarTimes
	{
		private Position pos;
		private UTCDate dt;
		private double jc;
		private double eqtime;
		private double decl;
		private UTCDate? sunrise;
		private UTCDate noon;
		private UTCDate? sunset;

		public SolarTimes(Position pos, UTCDate dt, 
								 double jc, double eqtime, double decl, 
								 UTCDate? sunrise, UTCDate noon, UTCDate? sunset)
		{
			this.pos = pos;
			this.dt = dt;
			this.jc = jc;
			this.eqtime = eqtime;
			this.decl = decl;
			this.sunrise = sunrise;
			this.noon = noon;
			this.sunset = sunset;
		}

		public Position Position
		{ get { return pos; } }

		public UTCDate DateTime
		{ get { return dt; } }

		public double JulianCentury
		{ get { return jc; } }

		public double EqTime
		{ get { return eqtime; } }

		public double Declination
		{ get { return decl; } }

		public UTCDate? Sunrise
		{ get { return sunrise; } }

		public UTCDate Noon
		{ get { return noon; } }

		public UTCDate? Sunset
		{ get { return sunset; } }

		// Helpers
		public string Print(string template, bool print_params)
		{
			string s = "";
			if (print_params) {
				s += string.Format(template, "longitude", pos.PrintLongitude());
				s += string.Format(template, "latitude", pos.PrintLatitude());
				s += string.Format(template, "timezone", dt.PrintTimezone());
				s += string.Format(template, "date", dt.Print());
				s += "\n";
			}
//			s += string.Format(template, "jc", jc);
//			s += string.Format(template, "eqtime", eqtime + " min");
//			s += string.Format(template, "decl", decl + " °");
			if (sunrise.HasValue) {
				s += string.Format(template, "sunrise", sunrise.Value.Print());
			} else {
				s += string.Format(template, "sunrise", "none");
			}
			s += string.Format(template, "noon", noon.Print());
			if (sunset.HasValue) {
				s += string.Format(template, "sunset", sunset.Value.Print());
			} else {
				s += string.Format(template, "sunset", "none");
			}
			return s;
		}
	}
}
