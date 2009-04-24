// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.IO;

namespace SolarbeamGui
{
	class Details
	{
		private string location;
		private string position;
		private string timezone;
		private string date;
		private string time;
		private string elevation;
		private string azimuth;
		private string sunrise;
		private string solarnoon;
		private string sunset;
		private string solardaylength;
		private string dawn;
		private string dusk;
		private string daylength;
		
		public Details(string location, string position,
		               string timezone, string date,
		               string time,
		               string elevation, string azimuth,
		               string sunrise, string solarnoon,
		               string sunset, string solardaylength,
		               string dawn, string dusk,
		               string daylength)
		{
			this.location = location;
			this.position = position;
			this.timezone = timezone;
			this.date = date;
			this.time = time;
			this.elevation = elevation;
			this.azimuth = azimuth;
			this.sunrise = sunrise;
			this.solarnoon = solarnoon;
			this.sunset = sunset;
			this.solardaylength = solardaylength;
			this.dawn = dawn;
			this.dusk = dusk;
			this.daylength = daylength;
		}
		
		public string Codegen()
		{
			int w = 17;
			string s = string.Empty;
			
			s += FormatTitle("Parameters");
			if ((location != null) && (location != string.Empty)) {
				s += FormatPair(w, "Location", location);
			}
			s += FormatPair(w, "Coordinates", position);
			s += FormatPair(w, "Timezone", timezone);
			s += FormatPair(w, "Date", date);
			s += FormatPair(w, "Time", time);
			s += "\n";
			s += FormatTitle("Solar position");
			s += FormatPair(w, "Solar elevation", elevation);
			s += FormatPair(w, "Solar azimuth", azimuth);
			s += "\n";
			s += FormatTitle("Solar times");
			s += FormatPair(w, "Sunrise", sunrise);
			s += FormatPair(w, "Solar noon", solarnoon);
			s += FormatPair(w, "Sunset", sunset);
			s += FormatPair(w, "Solar day length", solardaylength);
			s += "\n";
			s += FormatTitle("Civil twilight (elevation -6°)");
			s += FormatPair(w, "Dawn", dawn);
			s += FormatPair(w, "Dusk", dusk);
			s += FormatPair(w, "Day length", daylength);
			
			return s;
		}
		
		private static string FormatTitle(string s)
		{
			return string.Format("== {0} ==\n", s);
		}
		
		private static string FormatPair(int w, string key, string val)
		{
			return string.Format("{0,-" + w + "} : {1}\n", key, val);
		}
		
		public void Write(string path)
		{
			string s = Codegen();
			using (StreamWriter writer = new StreamWriter(path)) {
				writer.WriteLine(s);
				writer.Close();
			}
		}
	}
}
