// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiSplash : Form
	{
//		private Timer timer;
		public bool expired = false;
		public Label label;
		
		public GuiSplash()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			Bitmap logo = new Bitmap(Controller.AsmInfo.GetResource("logo.png"));
			this.BackgroundImage = logo;
			
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.None;
			this.Size = logo.Size;
			
			label = new Label();
			label.Text = "Starting...";
			label.Size = new Size(200, 20);
			label.Location = new Point(30, 170);
			label.BackColor = Color.White;
			label.ForeColor = Color.OrangeRed;
			this.Controls.Add(label);
			
			this.DoubleBuffered = true; // prevent flicker on updates
/*			
			timer = new Timer();
			timer.Interval = 1000;
			timer.Tick += new EventHandler(ProcessTick);
			timer.Enabled = true;
*/		}
		
		public void Launch()
		{
			Console.WriteLine("splash :: init");
			this.Show();
			Application.DoEvents();
			
			while (!expired) {
				Console.WriteLine("splash :: >> TICK");
				Thread.Sleep(100); //Thread.Sleep(0); // give up cpu slice only
				if (Controller.SplashQueue.Count > 0) {
					string msg = Controller.SplashQueue.Dequeue();
					Console.WriteLine("splash dequeue :: {0}", msg);
					label.Text = msg + "...";
					Application.DoEvents();
				}
				//Thread.Sleep(100);
				Console.WriteLine("splash :: << TICK");
			}
			
			Console.WriteLine("splash :: start close");
			this.Close();
			this.Dispose();
			Console.WriteLine("splash :: end close");
		}
/*		
		private void ProcessTick(object o, EventArgs a)
		{
			Console.WriteLine("splash :: ProcessTick()");
			if (expired) {
				timer.Dispose();
				Close();
			}
			
			Console.WriteLine("splash :: tick");
			if (Controller.SplashQueue.Count > 0) {
				string msg = Controller.SplashQueue.Dequeue();
				Console.WriteLine(msg);
			}
		}
*/		
/*		public void Expire(object o, EventArgs a)
		{
			Console.WriteLine("splash :: Expire()");
			expired = true;
		}
*/	}
}