// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace LibSolar.Types
{
	/**
	 * Stores the info used in caption printing.
	 */
	public struct CaptionInfo
	{
		private string loc;
		private Position pos;
		private double dst;
		private UTCDate udt;
		private SolarPosition sp;
		private SolarTimes st;
		private UTCDate? dawn;
		private UTCDate? dusk;
		
		public CaptionInfo(string loc, Position pos, UTCDate udt)
		{
			this.loc = loc;
			this.pos = pos;
			this.dst = udt.GetDST();
			this.udt = udt;
			this.sp = Orbit.CalcSolarPosition(pos, udt);
			this.st = Orbit.CalcSolarTimes(pos, udt);
			
			SolarTimes st_ss = PointFinder.FindDawnDusk(pos, udt);
			this.dawn = st_ss.Sunrise;
			this.dusk = st_ss.Sunset;
		}
		
		public string Location
		{ get { return this.loc; } }
		
		public Position Position
		{ get { return this.pos; } }
		
		public double Timezone
		{ get { return this.udt.Timezone; } }
				
		public double DST
		{ get { return this.dst; } }
		
		public UTCDate Date
		{ get { return this.udt; } }
		
		public double Elevation
		{ get { return this.sp.Elevation; } }
		
		public double Azimuth
		{ get { return this.sp.Azimuth; } }
		
		public UTCDate Noon
		{ get { return this.st.Noon; } }
		
		public UTCDate? Sunrise
		{ get { return this.st.Sunrise; } }
		
		public UTCDate? Sunset
		{ get { return this.st.Sunset; } }
		
		public UTCDate? Dawn
		{ get { return this.dawn; } }
		
		public UTCDate? Dusk
		{ get { return this.dusk; } }
	}	
}
