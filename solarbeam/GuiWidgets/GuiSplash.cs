// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiSplash : Form
	{
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
			
			this.Closed += new EventHandler(Quit);
		}
		
		private void Quit(object o, EventArgs a)
		{
			Application.Exit();
		}
	}
}