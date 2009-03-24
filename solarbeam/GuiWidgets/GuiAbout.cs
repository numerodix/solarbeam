// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiAbout : Form
	{
		public GuiAbout(string app_title)
		{
			InitializeComponent(app_title);
		}
		
		public void InitializeComponent(string app_title)
		{
			this.Text = "About " + app_title;
			this.Size = new Size(500, 300);
			this.StartPosition = FormStartPosition.CenterParent;
			
			// prevent disposal by intercepting Close() and calling Hide()
			this.Closing += delegate (object o, CancelEventArgs args) {
				args.Cancel = true;
				this.Hide();
			};
		}
	}
}