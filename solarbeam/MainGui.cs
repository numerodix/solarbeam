// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.
//
// <desc> Draw solar diagrams </desc>

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

using LibSolar.Assemblies;
using LibSolar.Graphing;

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

			splashthread = new Thread(RunSplash);
			splashthread.IsBackground = true;
			Thread.CurrentThread.IsBackground = true; // cure for cancer?
			splashthread.Name = "splash";
			splashthread.Start();
			Console.WriteLine("main :: After splash invoke");
			RunMainForm();
		}
		
		private static void RunSplash()
		{
			splash = new GuiSplash();
			splash.Launch();
		}
		
		private static void RunMainForm()
		{
//			Thread.Sleep(3000);
			mainform = new GuiMainForm(Controller.AsmInfo.GetAtt("Title"));
			Console.WriteLine("main :: after gui create");
			Application.EnableVisualStyles();
			
//			Thread.Sleep(3000);
			Console.WriteLine("main :: pre call expire");
			splash.expired = true;
			splashthread.Join();
//			splashthread.Abort();
			Console.WriteLine("main :: post call expire");
			
			if ((args.Length > 0) && (args[0] == "-checkhang")) {
				mainform.Shown += delegate (object o,EventArgs a) { 
					mainform.Refresh(); 
					mainform.Update(); 
					Thread.Sleep(1000); 
					Environment.Exit(0); };
			}
				
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
	}
}