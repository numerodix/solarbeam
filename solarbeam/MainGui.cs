// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.
//
// <desc> Draw solar diagrams </desc>

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

using LibSolar.Graphing;
using LibSolar.Util;

namespace SolarbeamGui
{
	static class MainGui
	{
		private static GuiSplash splash;
		private static GuiMainForm mainform;
		private static Thread splashthread;
		private static string[] args;
		
		[STAThread]
		public static void Main(string[] args)
		{
			MainGui.args = args;
			
			if (args.Length > 0) {
				if (args[0] == "-nogui") {
					TimeGuiCreate();
					Environment.Exit(0);
				} else if (args[0] == "-timeit") {
					TimeBitmapCreate();
					Environment.Exit(0);
				}
			}

			RunSplash();
			RunMainForm();
		}
		
		private static void RunSplash()
		{
			// start splash in separate thread
			splash = new GuiSplash(Controller.AsmInfo.GetAtt("Title"));
			splashthread = new Thread(splash.Launch);
			splashthread.IsBackground = true;
			splashthread.Name = "splash";
			splashthread.Start();
		}
		
		private static void RunMainForm()
		{
			Thread.CurrentThread.IsBackground = true; // prevent hung mainthread?
			mainform = new GuiMainForm(Controller.AsmInfo.GetAtt("Title"));
			Application.EnableVisualStyles();
			
			// all done initializing, kill splash
			splash.expired = true;
			splashthread.Join();
			
			if ((args.Length > 0) && (args[0] == "-checkhang")) {
				mainform.Shown += delegate (object o,EventArgs a) { 
					mainform.Refresh(); 
					mainform.Update(); 
					Thread.Sleep(1000); 
					Environment.Exit(0); };
			}

			// run message loop
			Application.Run(mainform);
		}

		private static void TimeGuiCreate()
		{
			GuiMainForm mainform =
				new GuiMainForm(Controller.AsmInfo.GetAtt("Title"));
		}
		
		private static void TimeBitmapCreate()
		{
			Stopwatch watch = new Stopwatch();
			watch.Start();
			
			GraphBitmapDemo.GenerateBitmap(5000, "img.png");
	
			watch.Stop();
			Console.WriteLine("Elapsed: {0}",watch.Elapsed);
			Console.WriteLine("In milliseconds: {0}",watch.ElapsedMilliseconds);
			Console.WriteLine("In timer ticks: {0}",watch.ElapsedTicks);
		}
		
		public static void Quit()
		{
			mainform.Close();
		}
	}
}
