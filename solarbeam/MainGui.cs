// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.
//
// <desc> Draw solar diagrams </desc>

using System;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

using LibSolar.Assemblies;
using LibSolar.Graphing;

namespace SolarbeamGui
{
	static class MainGui
	{		
		[STAThread]
		public static void Main()
		{
			//TimeIt();
			
			AsmInfo asminfo = new AsmInfo(Assembly.GetExecutingAssembly());
			Application.EnableVisualStyles();
			Application.Run(new GuiMainForm(asminfo.GetAtt("Title")));
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