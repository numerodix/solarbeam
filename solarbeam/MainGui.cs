// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.
//
// <desc> Draw solar diagrams </desc>

using System;
using System.Diagnostics;
using System.Windows.Forms;

using LibSolar.Assemblies;
using LibSolar.Graphing;

namespace SolarbeamGui
{
	static class MainGui
	{
		[STAThread]
		public static void Main(string[] args)
		{
			GuiMainForm mainform = 
				new GuiMainForm(Controller.AsmInfo.GetAtt("Title"));
			if ((args.Length > 0) && (args[0] == "-nogui")) {
				Environment.Exit(0);
			}
			Application.EnableVisualStyles();
			Application.Run(mainform);
		}

		public static void TimeIt()
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();
			
			GraphBitmapDemo.GenerateBitmap(2000, "img.png");
	
			watch.Stop();
			Console.WriteLine("Elapsed: {0}",watch.Elapsed);
			Console.WriteLine("In milliseconds: {0}",watch.ElapsedMilliseconds);
			Console.WriteLine("In timer ticks: {0}",watch.ElapsedTicks);
		}
	}
}