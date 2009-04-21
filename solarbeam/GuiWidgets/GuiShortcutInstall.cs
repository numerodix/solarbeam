// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using LibSolar.Util;

namespace SolarbeamGui
{
	sealed class GuiShortcutInstall : Form
	{
		public GuiShortcutInstall(string app_title, string icon)
		{
			InitializeComponent(app_title, icon);
		}
		
		public void InitializeComponent(string app_title, string icon)
		{	
			this.DoubleBuffered = true;
			this.Text = "About " + app_title;
			this.Icon = new Icon(Controller.AsmInfo.GetResource(icon));
			
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ClientSize = new Size(300, 200);
			
			// prevent disposal by intercepting Close() and calling Hide()
			this.Closing += delegate (object o, CancelEventArgs args) {
				args.Cancel = true;
				this.Hide();
			};
		}
	}
}
