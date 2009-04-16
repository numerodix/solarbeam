// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;

using LibSolar.Types;

namespace LibSolar.SolarOrbit
{
	/**
	 * This class is a port of SRRB javascript code from
	 * http://www.srrb.noaa.gov/highlights/sunrise/sunrise.html
	 *
	 * Note from original source:
	 * The calculations in the NOAA Sunrise/Sunset and Solar Position
	 * Calculators are based on equations from Astronomical Algorithms,
	 * by Jean Meeus. The sunrise and sunset results have been verified
	 * to be accurate to within a minute for locations between +/- 72° 
	 * latitude, and within 10 minutes outside of those latitudes.
	 */
	public class Orbit
	{
		public const double ASTRONOMICAL_TWIGHLIGHT = -18.0; /**< elevation angle (deg) */
		public const double NAUTICAL_TWIGHLIGHT = -12.0; /**< elevation angle (deg) */
		public const double CIVIL_TWIGHLIGHT = -6.0; /**< elevation angle (deg) */

		/**
		 * Calculate the geometric mean longitude of the Sun.
		 * @param jc julian century
		 * @return in degrees
		 */
		private static double CalcGeometricMeanLongtitudeSun(double jc)
		{
			double l0 = 280.46646 + jc
				* (36000.76983 + 0.0003032 * jc);

			while (l0 > 360.0) {
				l0 -= 360.0;
			}
			while (l0 < 0.0) {
				l0 += 360.0;
			}

			return l0;
		}

		/**
		 * Calculate the geometric mean anomaly of the Sun.
		 * @param jc julian century
		 * @return in degrees
		 */
		private static double CalcGeometricMeanAnomalySun(double jc)
		{
			return ( 357.52911
					+ jc * (35999.05029 - 0.0001537 * jc) );
		}

		/**
		 * Calculate the eccentricity of the Earth's orbit.
		 * @param jc julian century
		 * @return unitless
		 */
		private static double CalcEccentricityEarthOrbit(double jc)
		{
			return ( 0.016708634
					- jc * (0.000042037 + 0.0000001267 * jc) );
		}

		/**
		 * Calculate the equation of the center of the Sun.
		 * @param jc julian century
		 * @return in degrees
		 */
		private static double CalcSunEqOfCenter(double jc)
		{
			double m = CalcGeometricMeanAnomalySun(jc);

			double mrad = Coordinate.DegToRad(m);
			double sinm = Math.Sin(mrad);
			double sin2m = Math.Sin(mrad + mrad);
			double sin3m = Math.Sin(mrad + mrad + mrad);

			double c =
				sinm * (1.914602 - jc * (0.004817 + 0.000014 * jc))
					+ sin2m * (0.019993 - 0.000101 * jc)
					+ sin3m * 0.000289;
			return c;
		}

		/**
		 * Calculate the true longitude of the Sun.
		 * @param jc julian century
		 * @return in degees
		 */
		private static double CalcSunTrueLongtitude(double jc)
		{
			double l0 = CalcGeometricMeanLongtitudeSun(jc);
			double c = CalcSunEqOfCenter(jc);

			return (l0 + c);
		}

		/**
		 * Calculate the apparent longitude of the Sun.
		 * @param jc julian century
		 * @return in degrees
		 */
		private static double CalcSunApparentLongtitude(double jc)
		{
			double o = CalcSunTrueLongtitude(jc);

			double omega = 125.04 - 1934.136 * jc;
			double lambda =
				o - 0.00569 - 0.00478 * Math.Sin( Coordinate.DegToRad(omega) );

			return lambda;
		}

		/**
		 * Calculate the mean obliquity of the ecliptic.
		 * @param jc julian century
		 * @return in degrees
		 */
		private static double CalcMeanObliquityOfEcliptic(double jc)
		{
			double seconds = 21.448
				- jc * (46.8150 + jc
				   * (0.00059 - jc * 0.001813));
			double e0 = 23.0 + ( 26.0 + (seconds / 60.0) ) / 60.0;

			return e0;
		}

		/**
		 * Calculate the corrected obliquity of the ecliptic.
		 * @param jc julian century
		 * @return in degrees
		 */
		private static double CalcObliquityCorrection(double jc)
		{
			double e0 = CalcMeanObliquityOfEcliptic(jc);

			double omega = 125.04 - 1934.136 * jc;
			double e = e0 + 0.00256 * Math.Cos( Coordinate.DegToRad(omega) );
			return e;
		}

		/**
		 * Calculate the declination of the Sun.
		 * @param jc julian century
		 * @return in degrees
		 */
		private static double CalcSunDeclination(double jc)
		{
			double e = CalcObliquityCorrection(jc);
			double lambda = CalcSunApparentLongtitude(jc);

			double sint = Math.Sin( Coordinate.DegToRad(e) )
				* Math.Sin( Coordinate.DegToRad(lambda) );
			double theta = Coordinate.RadToDeg( Math.Asin(sint) );
			return theta;
		}

		/**
		 * Calculate the difference between true solar time and mean solar time
		 * @param jc julian century
		 * @return in minutes of time
		 */
		private static double CalcEquationOfTime(double jc)
		{
			double epsilon = CalcObliquityCorrection(jc);
			double l0 = CalcGeometricMeanLongtitudeSun(jc);
			double e = CalcEccentricityEarthOrbit(jc);
			double m = CalcGeometricMeanAnomalySun(jc);

			double y = Math.Tan( Coordinate.DegToRad(epsilon) / 2.0 );
			y *= y;

			double sin2l0 = Math.Sin( 2.0 * Coordinate.DegToRad(l0) );
			double sinm   = Math.Sin( Coordinate.DegToRad(m) );
			double cos2l0 = Math.Cos( 2.0 * Coordinate.DegToRad(l0) );
			double sin4l0 = Math.Sin( 4.0 * Coordinate.DegToRad(l0) );
			double sin2m  = Math.Sin( 2.0 * Coordinate.DegToRad(m) );

			double etime = y * sin2l0
				- 2.0 * e * sinm
					+ 4.0 * e * y * sinm * cos2l0
					- 0.5 * y * y * sin4l0
					- 1.25 * e * e * sin2m;

			return Coordinate.RadToDeg(etime) * 4.0;
		}

		/**
		 * Calculate the true solar time.
		 * @param eqtime equation of time in minutes of time
		 * @param lon longitude in degrees
		 * @return in minutes of time
		 */
		private static double CalcTrueSolarTime(double eqtime, double lon,
		                                        int hour, int min, int sec)
		{
			// first find the time offset
			double solarTimeFix = eqtime - 4.0 * lon;
			double trueSolarTime = hour * 60.0 + min + sec/60.0 + solarTimeFix;

			while (trueSolarTime > 1440) {
				trueSolarTime -= 1440;
			}
			return trueSolarTime;
		}

		/**
		 * Calculate the solar hour angle.
		 * @param eqtime the equation of time in minutes of time
		 * @param lon longitude in degrees
		 * @return in degrees
		 */
		private static double CalcHourAngle(double eqtime, double lon,
		                                    int hour, int min, int sec)
		{
			double tst = CalcTrueSolarTime(eqtime, lon, hour, min, sec);
			double hourAngle = tst / 4.0 - 180.0;
			if (hourAngle < -180) {
				hourAngle += 360;
			}
			return hourAngle;
		}

		/**
		 * Calculate the hour angle of the Sun at sunrise.
		 * @param rise true == sunrise, false == sunset
		 * @param lat latitude in degrees
		 * @param decl solar declination angle in degrees
		 * @return in radians
		 */
		private static double CalcHourAngleAtSunriseSunset(bool rise, double lat, 
		                                                   double decl)
		{
			double lat_rad = Coordinate.DegToRad(lat);
			double decl_rad  = Coordinate.DegToRad(decl);

			double hour_ang = ( Math.Acos( Math.Cos( Coordinate.DegToRad(90.833) )
									/ ( Math.Cos(lat_rad) * Math.Cos(decl_rad) )
									- Math.Tan(lat_rad) * Math.Tan(decl_rad) ) );

			if ( Double.IsNaN(hour_ang) ) {
				throw new ArgumentException(
					string.Format(
						"Latitude `{0}` and declination `{1}` produces hour angle `{2}`",
						lat, decl, hour_ang));
			}

			if (rise) {
				return hour_ang;
			} else {
				return -hour_ang;
			}
		}

		/**
		 * Calculate the solar zenith angle.
		 * @param lat latitude in degrees
		 * @param decl solar declination in degrees
		 * @param ha solar hour angle in degrees
		 * @return in degrees
		 */
		private static double CalcSolarZenithAngle(double lat, double decl,
		                                           double ha)
		{
			double csz = Math.Sin( Coordinate.DegToRad(lat) )
				* Math.Sin( Coordinate.DegToRad(decl) )
				+ Math.Cos( Coordinate.DegToRad(lat) )
				* Math.Cos( Coordinate.DegToRad(decl) )
				* Math.Cos( Coordinate.DegToRad(ha) );
			if (csz > 1.0) {
				csz = 1.0;
			} else if (csz < -1.0) {
				csz = -1.0;
			}
			double zenith = Coordinate.RadToDeg( Math.Acos(csz) );
			return zenith;
		}

		/**
		 * Calculate azimuth angle.
		 * @param lat latitude in degrees
		 * @param zenith solar zenith angle in degrees
		 * @param decl solar declination in degrees
		 * @param hour_ang solar hour angle in degrees
		 * @return in degrees
		 */
		private static double CalcAzimuth(double lat, double zenith,
		                                  double decl, double hour_ang)
		{
			double azDenom = ( Math.Cos( Coordinate.DegToRad(lat) ) *
							  Math.Sin( Coordinate.DegToRad(zenith) ) );
			double azimuth, azRad;
			if ( Math.Abs(azDenom) > 0.001 ) {
				azRad = ( Math.Sin( Coordinate.DegToRad(lat) ) *
						  Math.Cos( Coordinate.DegToRad(zenith) ) -
						  Math.Sin( Coordinate.DegToRad(decl) ) )
					/ azDenom;
				if ( Math.Abs(azRad) > 1.0 ) {
					if (azRad < 0) {
						azRad = -1.0;
					} else {
						azRad = 1.0;
					}
				}

				azimuth = 180.0 - Coordinate.RadToDeg( Math.Acos(azRad) );
				
				if (hour_ang > 0.0) {
					azimuth = -azimuth;
				}
			} else {
				if (lat > 0.0) {
					azimuth = 180.0;
				} else { 
					azimuth = 0.0;
				}
			}
			if (azimuth < 0.0) {
				azimuth += 360.0;
			}
			return azimuth;
		}

		/**
		 * Calculate the solar refraction correction.
		 * @param zenith solar zenith angle in degrees
		 * @return in degrees
		 */
		private static double CalcSolarRefractionCorrection(double zenith)
		{
			double exoatmElevation = 90.0 - zenith;
			double refcorr;
			if (exoatmElevation > 85.0) {
				refcorr = 0.0;
			} else {
				double te = Math.Tan( Coordinate.DegToRad(exoatmElevation) );
				if (exoatmElevation > 5.0) {
					refcorr = 58.1 / te
						- 0.07 / (te*te*te)
						+ 0.000086 / (te*te*te*te*te);
				} else if (exoatmElevation > -0.575) {
					refcorr = 1735.0
						+ exoatmElevation
						* (-518.2 + exoatmElevation
							* (103.4 + exoatmElevation
								* (-12.79 + exoatmElevation * 0.711) ) );
				} else {
					refcorr = -20.774 / te;
				}
				refcorr = refcorr / 3600.0;
			}
			return refcorr;
		}

		/**
		 * Calculate solar elevation angle.
		 * @param zenith solar zenith angle in degrees
		 * @return in degrees
		 */
		private static double CalcSolarElevation(double zenith)
		{
			double refcorr = CalcSolarRefractionCorrection(zenith);
			double solar_zen = zenith - refcorr;
			double elevation = 90.0 - solar_zen;
			return elevation;
		}

		/**
		 * Calculate the time of solar noon.
		 * @param jc julian century
		 * @param lon longitude in degrees
		 * @return in minutes of time
		 */
		private static double CalcSolarNoon(double jc, double lon)
		{
			// First pass uses approximate solar noon to calculate eqtime
			double jd = JulianDate.CalcJulianDay(jc) + lon / 360;
			double jc_noon = JulianDate.CalcJulianCentury(jd);
			double eqtime = CalcEquationOfTime(jc_noon);
			double sol_noon = 720 + (lon * 4) - eqtime; // min

			jd = JulianDate.CalcJulianDay(jc)
				- 0.5 + sol_noon / 1440.0;
			jc_noon = JulianDate.CalcJulianCentury(jd);
			eqtime = CalcEquationOfTime(jc_noon);
			sol_noon = 720 + (lon * 4) - eqtime; // min
			
			return sol_noon;
		}

		/**
		 * Calculate the time of sunrise.
		 * @param rise true == sunrise, false == sunset
		 * @param jc julian century
		 * @param solar_noon solar noone time in minutes
		 * @param lon longitude in degrees
		 * @param lat latitude in degrees
		 * @return in minutes of time
		 */
		private static double CalcSunriseSunset(bool rise,
		                                        double solar_noon, double jc,
		                                        double lon, double lat)
		{
			double jd = JulianDate.CalcJulianDay(jc);
			double jc_noon = JulianDate.CalcJulianCentury(jd + solar_noon / 1440.0);

			double eqtime = CalcEquationOfTime(jc_noon);
			double decl = CalcSunDeclination(jc_noon);

			double hour_ang;
			if (rise) {
				hour_ang = CalcHourAngleAtSunriseSunset(true, lat, decl);
			} else {
				hour_ang = CalcHourAngleAtSunriseSunset(false, lat, decl);
			}

			double delta = lon - Coordinate.RadToDeg(hour_ang);
			double time_diff = 4 * delta;	// in minutes of time
			double time = 720 + time_diff - eqtime;	// in minutes


			double new_jd = JulianDate.CalcJulianDay(jc) + time / 1440.0;
			double new_jc_noon = JulianDate.CalcJulianCentury(new_jd);
			double new_eqtime = CalcEquationOfTime(new_jc_noon);
			double new_decl = CalcSunDeclination(new_jc_noon);

			double new_hour_ang;
			if (rise) {
				new_hour_ang = CalcHourAngleAtSunriseSunset(true, lat, new_decl);
			} else {
				new_hour_ang = CalcHourAngleAtSunriseSunset(false, lat, new_decl);
			}

			double new_delta = lon - Coordinate.RadToDeg(new_hour_ang);
			double new_time_diff = 4 * new_delta;
			double new_time = 720 + new_time_diff - new_eqtime; // in minutes

			return new_time;
		}

		/**
		 * Calculate solar position for location and date.
		 * @param dt datetime in UTC
		 * @return solar positions in degrees
		 */
		public static SolarPosition CalcSolarPosition(Position pos, UTCDate dt)
		{
			double jc = JulianDate.CalcJulianCentury(dt.Year, dt.Month, dt.Day,
													 dt.Hour, dt.Minute, dt.Second);
			double eqtime = CalcEquationOfTime(jc);
			double decl = CalcSunDeclination(jc);
			double hour_ang = CalcHourAngle(eqtime, pos.Longitude,
											dt.Hour, dt.Minute, dt.Second);
			double zenith = CalcSolarZenithAngle(pos.Latitude, decl, hour_ang);

			// calculate end products
			double azimuth = CalcAzimuth(pos.Latitude, zenith, decl, hour_ang);
			double elevation = CalcSolarElevation(zenith);

			return new SolarPosition(pos, dt, jc, eqtime, decl, azimuth, elevation);
		}

		/**
		 * Calculate solar sunrise, noon and sunset.
		 * @param dt datetime in UTC
		 * @return solar times in UTC
		 */
		public static SolarTimes CalcSolarTimes(Position pos, UTCDate dt)
		{
			// reset time to start of day as solar times are given in increments
			// from the beginning of the day
			dt = dt.AtStartOfUTCDay();

			double jc = JulianDate.CalcJulianCentury(dt.Year, dt.Month, dt.Day, 
													 dt.Hour, dt.Minute, dt.Second);
			double solar_noon = CalcSolarNoon(jc, pos.Longitude);
			UTCDate d_noon = dt.AddMinutes(solar_noon);

			UTCDate? d_rise;
			try {
				double sunrise = CalcSunriseSunset(true, solar_noon, jc,
												   pos.Longitude, pos.Latitude);
				d_rise = dt.AddMinutes(sunrise);
			} catch (ArgumentException) {
				d_rise = null;
			}

			UTCDate? d_set;
			try {
				double sunset = CalcSunriseSunset(false, solar_noon, jc,
												  pos.Longitude, pos.Latitude);
				d_set = dt.AddMinutes(sunset);
			} catch (ArgumentException) {
				d_set = null;
			}

			// <diag>
			double jd = JulianDate.CalcJulianDay(jc)
				- 0.5 + solar_noon / 1440.0;
			double jc_noon = JulianDate.CalcJulianCentury(jd);

			double eqtime = CalcEquationOfTime(jc_noon);
			double decl = CalcSunDeclination(jc_noon);
			// </diag>

			return new SolarTimes(pos, dt, jc, eqtime, decl, d_rise, d_noon, d_set);
		}
	}
}
