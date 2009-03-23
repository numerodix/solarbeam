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
		
		[STAThread]
		public static void Main(string[] args)
		{
			if (args.Length > 0) {
				if (args[0] == "-nogui") {
					GuiMainForm mainform =
						new GuiMainForm(Controller.AsmInfo.GetAtt("Title"));
				} else if (args[0] == "-timeit") {
					TimeIt();
				}
				Environment.Exit(0);
			}
/*			
			Thread thread_splash = new Thread(new ThreadStart(RunSplash));
			thread_splash.Start();
			
			Thread.Sleep(1000);
			thread_splash.Abort();
			Thread.Sleep(1000);
*/			
			Thread splashthread = new Thread(RunSplash);
			splashthread.Start();
//			RunSplash();
			Console.WriteLine("main :: After splash invoke");
			Controller.SplashQueue.Enqueue("Item");
			RunMainForm();
		}
		
		private static void RunSplash()
		{
			splash = new GuiSplash();
//			Application.Run(splash);
			splash.Launch();
		}
		
		private static void RunMainForm()
		{
			Thread.Sleep(3000);
			mainform = new GuiMainForm(Controller.AsmInfo.GetAtt("Title"));
			Console.WriteLine("main :: after gui create");
			Application.EnableVisualStyles();
			
			Thread.Sleep(3000);
			Console.WriteLine("main :: pre call expire");
			splash.expired = true;
			//splashthread.Join();
			Console.WriteLine("main :: post call expire");
			Application.Run(mainform);
		}

		private static void TimeIt()
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