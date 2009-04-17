// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

using NDesk.Options;

using LibSolar.Graphing;
using LibSolar.SolarOrbit;
using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamCli
{
	static class MainCli
	{
		public static void Main(string[] args)
		{
			bool verbose = false;
			bool help = false;
			bool bench = false;
	
			// calc defaults
			string latitude = null;
			string longitude = null;
			string timezone = null;
			string date = null;
			string time = null;
			
			// generate image, set to negative dimensions
			int imgsz = -1;
	
			// bench defaults
			int step = 1;
			bool timebased = true;
	
			OptionSet p = new OptionSet ()
				.Add ("lat=", "Latitude position: (N/S).00.00.00", 
				      delegate (string v) { latitude = v; })
				.Add ("lon=", "Longitude position: (E/W).000.00.00", 
					  delegate (string v) { longitude = v; })
				.Add ("tz=", "Timezone offset from UTC: [-12,14]",
					  delegate (string v) { timezone = v; })
				.Add ("dt=", "Date: dd.mm.yyyy",
					  delegate (string v) { date = v; })
				.Add ("tm=", "Time: hh:mm:ss",
					  delegate (string v) { time = v; })
				.Add ("img=", "Generate diagram image: 500",
					  delegate (int v) { imgsz = v; })
				.Add ("benchtime-5", "Benchmark all times with 5sec increments",
					  delegate (string v) {
						bench = true;
						timebased = true;
						step = 5;
						})
				.Add ("benchpos-5", "Benchmark all positions with 5° increments",
					  delegate (string v) {
						bench = true;
						timebased = false;
						step = 5;
						})
				.Add ("v|verbose", "Display benchmark results",
					  delegate (string v) { verbose = true; })
				.Add ("h|help", "Display this message",
					  delegate (string v) { help = true; });
			p.Parse(args);
	
			if ((help) || (args.Length == 0)) {
				AsmInfo asminfo = new AsmInfo(Assembly.GetExecutingAssembly());
				Console.Error.WriteLine("{0} {1}",
				                        asminfo.GetAtt("Title"),
				                        asminfo.GetAtt("Version"));
				
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
				Calc(imgsz, longitude, latitude, timezone, date, time);
			} catch (ArgumentException e) {
				Console.WriteLine(e.Message);
			}
		}
	
		public static void Calc(int imgsz, string longitude, string latitude, 
		                        string timezone, string date, string time)
		{
			// Halt on missing arguments
			if ( null == date ) {
				throw new ArgumentException("Missing --date argument");
			}
			if ( null == time ) {
				throw new ArgumentException("Missing --time argument");
			}
			if ( null == latitude ) {
				throw new ArgumentException("Missing --latitude argument");
			}
			if ( null == longitude ) {
				throw new ArgumentException("Missing --longitude argument");
			}
			if ( null == timezone ) {
				throw new ArgumentException("Missing --timezone argument");
			}
	
			// All systems go
			ArrayList las = Parsing.SplitParse(latitude, '.');
			ArrayList los = Parsing.SplitParse(longitude, '.');
			ArrayList tz = Parsing.SplitParse(timezone, '.');
			ArrayList ds = Parsing.SplitParse(date, '.');
			ArrayList ts = Parsing.SplitParse(time, ':');
	
			// figure out directions
			PositionDirection ladir = Position.LATITUDE_POS;
			if (String.Compare((string) las[0], "n", true) == 0) {
				ladir = Position.LATITUDE_POS;
			} else if (String.Compare((string) las[0], "s", true) == 0) {
				ladir = Position.LATITUDE_NEG;
			} else {
				throw new ArgumentException(
						string.Format("Wrong value for latitude: {0}", latitude));
			}
	
			PositionDirection lodir = Position.LONGITUDE_POS;
			if (String.Compare((string) los[0], "e", true) == 0) {
				lodir = Position.LONGITUDE_POS;
			} else if (String.Compare((string) los[0], "w", true) == 0) {
				lodir = Position.LONGITUDE_NEG;
			} else {
				throw new ArgumentException(
						string.Format("Wrong value for longitude: {0}", longitude));
			}
	
			Position pos = new Position(ladir,
			                            (int) las[1], (int) las[2], (int) las[3],
			                            lodir,
			                            (int) los[1], (int) los[2], (int) los[3]);
	
			UTCDate dt = new UTCDate((int) tz[0], null,
			                   (int) ds[2], (int) ds[1], (int) ds[0],
			                   (int) ts[0], (int) ts[1], (int) ts[2]);
	
			SolarPosition sp = Orbit.CalcSolarPosition(pos, dt);
			SolarTimes sns = Orbit.CalcSolarTimes(pos, dt);
	
			Printing.Print(sp, sns);
			
			if (imgsz > 0) {
				GenImg(imgsz, pos, dt);
			}
		}
		
		private static void GenImg(int dim, Position pos, UTCDate udt)
		{
			string path = Formatter.FormatImgFilename(string.Empty, pos, udt);

			// set up constants
			Colors colors = new Colors();
			string font_face = "Arial";
			
			// generate base image
			GraphBitmap grbit = new GraphBitmap(false, dim, colors, font_face);
			Bitmap bitmap_plain = grbit.RenderBaseImage(pos, udt);
			
			// render current day
			Bitmap bitmap_fst = grbit.RenderCurrentDay(bitmap_plain, dim, pos, udt);
			
			// save
			grbit.SaveBitmap(bitmap_fst, path);
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
					Position pos = new Position(Position.LatDirFromVal(lat), lat, 0, 0,
					                            Position.LonDirFromVal(lon), lon, 0, 0);

					int year = 2009;
					int month = 2;
					int day = 1;
	
					UTCDate dt = new UTCDate(timezone, null, year, month, day, 0, 0, 0);
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
						Position pos = new Position(Position.LatDirFromVal(lat), lat, 0, 0,
						                            Position.LonDirFromVal(lon), lon, 0, 0);
	
						int hour = 11;
						int min = 0;
						int sec = 0;
	
						int year = 2009;
						int month = 2;
						int day = 1;
	
						UTCDate dt = new UTCDate(timezone, null, 
						                         year, month, day, hour, min, sec);
	
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
