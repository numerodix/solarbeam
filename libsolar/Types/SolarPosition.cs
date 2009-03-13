// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.Types
{
	public struct SolarPosition
	{
		private Position pos;
		private UTCDate dt;
		private double jc;
		private double eqtime;
		private double decl;
		private double az;
		private double el;

		public SolarPosition(Position pos, UTCDate dt, 
							 double jc, double eqtime, double decl, 
							 double az, double el)
		{
			this.pos = pos;
			this.dt = dt;
			this.jc = jc;
			this.eqtime = eqtime;
			this.decl = decl;
			this.az = az;
			this.el= el;
		}

		public Position Position
		{ get { return pos; } }

		public UTCDate Date
		{ get { return dt; } }

		public double JulianCentury
		{ get { return jc; } }

		public double EqTime
		{ get { return eqtime; } }

		public double Declination
		{ get { return decl; } }

		public double Azimuth
		{ get { return az; } }

		public double Elevation
		{ get { return el; } }

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
			s += string.Format(template, "azimuth", az + " °");
			s += string.Format(template, "elevation", el + " °");
			return s;
		}
	}
}
