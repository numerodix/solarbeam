// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Diagnostics;

using NDesk.Options;

using LibSolar.SolarOrbit;
using LibSolar.Types;

namespace SolarbeamCli
{
	class MainCli
	{
		public static void Main(string[] args)
		{
			bool verbose = false;
			bool help = false;
			bool bench = false;
	
			// calc defaults
			string longitude = null;
			string latitude = null;
			string timezone = null;
			string date = null;
			string time = null;
	
			// bench defaults
			int step = 1;
			bool timebased = true;
	
			OptionSet p = new OptionSet ()
				.Add ("lon|longitude=", delegate (string v) { longitude = v; })
				.Add ("lat|latitude=", delegate (string v) { latitude = v; })
				.Add ("tz|timezone=", delegate (string v) { timezone = v; })
				.Add ("dt|date=", delegate (string v) { date = v; })
				.Add ("tm|time=", delegate (string v) { time = v; })
				.Add ("v|verbose", delegate (string v) { verbose = true; })
				.Add ("benchtime-5", delegate (string v) {
						bench = true;
						timebased = true;
						step = 5;
						})
				.Add ("benchloc-5", delegate (string v) {
						bench = true;
						timebased = false;
						step = 5;
						})
				.Add ("h|?|help", delegate (string v) { help = true; });
			p.Parse(args);
	
			if ((help) || (args.Length == 0)) {
				Console.Error.WriteLine("Usage:");
				Console.Error.Write("  console.exe");
				Console.Error.Write(" -lat N.63.25.47");
				Console.Error.Write(" -lon E.10.23.36");
				Console.Error.Write(" -dt 21.06.2009");
				Console.Error.Write(" -tm 12:00:00");
				Console.Error.Write(" -tz 1");
				Console.Error.WriteLine("\nOptions:");
				p.WriteOptionDescriptions(Console.Error);
				return;
			}
			if (bench) {
				Bench(step, timebased, verbose);
				return;
			}
			try
			{
				Calc(longitude, latitude, timezone, date, time);
			} catch (ArgumentException e) {
				Console.WriteLine(e.Message);
			}
		}
	
		public static void Calc(string longitude, string latitude, string timezone,
		                        string date, string time)
		{
			// Halt on missing arguments
			if ( null == date ) {
				throw new ArgumentException("Missing --date argument");
			}
			if ( null == time ) {
				throw new ArgumentException("Missing --time argument");
			}
			if ( null == longitude ) {
				throw new ArgumentException("Missing --longitude argument");
			}
			if ( null == latitude ) {
				throw new ArgumentException("Missing --latitude argument");
			}
			if ( null == timezone ) {
				throw new ArgumentException("Missing --timezone argument");
			}
	
			// All systems go
			ArrayList los = Parsing.SplitParse(longitude, '.');
			ArrayList las = Parsing.SplitParse(latitude, '.');
			ArrayList tz = Parsing.SplitParse(timezone, '.');
			ArrayList ds = Parsing.SplitParse(date, '.');
			ArrayList ts = Parsing.SplitParse(time, ':');
	
			// figure out directions
			PositionDirection lodir = Position.LONGITUDE_POS;
			try
			{
				if (String.Compare((string) los[0], "e", true) == 0)
				{
					lodir = Position.LONGITUDE_POS;
				} else if (String.Compare((string) los[0], "w", true) == 0) {
					lodir = Position.LONGITUDE_NEG;
				} else throw new Exception();
			} catch {
				throw new ArgumentException(
						string.Format("Wrong value for longitude: {0}", longitude));
			}
	
			PositionDirection ladir = Position.LATITUDE_POS;
			try
			{
				if (String.Compare((string) las[0], "n", true) == 0)
				{
					ladir = Position.LATITUDE_POS;
				} else if (String.Compare((string) las[0], "s", true) == 0) {
					ladir = Position.LATITUDE_NEG;
				} else throw new Exception();
			} catch {
				throw new ArgumentException(
						string.Format("Wrong value for latitude: {0}", latitude));
			}
	
			Position pos = new Position(lodir,
			                            (int) los[1], (int) los[2], (int) los[3],
			                            ladir,
			                            (int) las[1], (int) las[2], (int) las[3]);
	
			UTCDate dt = new UTCDate((int) tz[0],
			                   (int) ds[2], (int) ds[1], (int) ds[0],
			                   (int) ts[0], (int) ts[1], (int) ts[2]);
	
			SolarPosition sp = Orbit.CalcSolarPosition(pos, dt);
			SolarTimes sns = Orbit.CalcSolarTimes(pos, dt);
	
			Printing.Print(sp, sns);
		}
	
		public static void Bench(int step, bool timebased, bool verbose)
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();
	
			int it = 0;
			if (timebased) {
				for (int s = 0; s < 86400; s += step)
				{
					it++;
	
					int lon = 10;
					int lat = 59;
					int timezone = (int) (lon / 15);
					Position pos = new Position(Position.LonDirFromVal(lon), lon, 0, 0, 
					                            Position.LatDirFromVal(lat), lat, 0, 0);
	
					int year = 2009;
					int month = 2;
					int day = 1;
	
					UTCDate dt = new UTCDate(timezone, year, month, day, 0, 0, 0);
					dt = dt.AddSeconds(s);
	
					SolarPosition sp = Orbit.CalcSolarPosition(pos, dt);
					SolarTimes sns = Orbit.CalcSolarTimes(pos, dt);
	
					if (verbose)
					{
						Printing.Print(sp, sns);
					}
				}
			} else {
				for (int lon = -180; lon <= 180; lon += step)
				{
					for (int lat = -90; lat <= 90; lat += step)
					{
						it++;
	
						int timezone = (int) (lon / 15);
						Position pos = new Position(Position.LonDirFromVal(lon), lon, 0, 0, 
						                            Position.LatDirFromVal(lat), lat, 0, 0);
	
						int hour = 11;
						int min = 0;
						int sec = 0;
	
						int year = 2009;
						int month = 2;
						int day = 1;
	
						UTCDate dt = new UTCDate(timezone, year, month, day, hour, min, sec);
	
						SolarPosition sp = Orbit.CalcSolarPosition(pos, dt);
						SolarTimes sns = Orbit.CalcSolarTimes(pos, dt);
	
						if (verbose)
						{
							Printing.Print(sp, sns);
						}
					}
				}
			}
			
			watch.Stop();
			Console.WriteLine("Iterations: {0}", it);
			Console.WriteLine("Elapsed: {0}",watch.Elapsed);
			Console.WriteLine("In milliseconds: {0}",watch.ElapsedMilliseconds);
			Console.WriteLine("In timer ticks: {0}",watch.ElapsedTicks);
		}
	}
}