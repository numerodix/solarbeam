// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

namespace LibSolar.SolarOrbit
{
	/**
	 * Primitives for handling cartesian coordinates.
	 */
	class Coordinate
	{
		public static double RadToDeg(double angle_rad)
		{
			return (180.0 * angle_rad / Math.PI);
		}

		public static double DegToRad(double angle_deg)
		{
			return (Math.PI * angle_deg / 180.0);
		}
	}
}
