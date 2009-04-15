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
		volatile public bool expired = false; // atomic reads/writes
		public Label label;
		
		public GuiSplash(string form_title, string icon)
		{
			InitializeComponent(form_title, icon);
		}
		
		private void InitializeComponent(string form_title, string icon)
		{
			this.DoubleBuffered = true; // prevent flicker on updates
			this.Text = form_title;
			this.Icon = new Icon(Controller.AsmInfo.GetResource(icon));
			
			Bitmap logo = new Bitmap(Controller.AsmInfo.GetResource("logo.png"));
			this.BackgroundImage = logo;
			
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.None;
			this.Size = new Size(logo.Size.Width+2, logo.Size.Height+2);
			this.BackColor = Color.Black;
			this.BackgroundImageLayout = ImageLayout.Center;
			
			label = new Label();
			label.Text = "Starting...";
			label.Size = new Size(200, 18);
			label.Location = new Point(30, 170);
			label.BackColor = Color.White;
			label.ForeColor = Color.OrangeRed;
			this.Controls.Add(label);
		}
		
		public void Launch()
		{
			this.Show();
			this.Activate(); // bring to front
			Application.DoEvents();
			
			while (!expired) {
				Thread.Sleep(100);
				if (Controller.SplashQueue.Count > 0) {
					string msg = Controller.SplashQueue.Dequeue();
					label.Text = msg + "...";
					Application.DoEvents();
				}
			}
			
			this.Close();
		}
	}
}