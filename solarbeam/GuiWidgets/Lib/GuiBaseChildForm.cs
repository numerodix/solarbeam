// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SolarbeamGui
{
	abstract class GuiBaseChildForm : Form
	{
		public GuiBaseChildForm(string form_title, string icon)
		{
			this.DoubleBuffered = true;
			this.Text = form_title;
			this.Icon = Controller.AsmInfo.GetIcon(icon);
						
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			
			// prevent disposal by intercepting Close() and calling Hide()
			this.Closing += delegate (object o, CancelEventArgs args) {
				args.Cancel = true;
				this.Hide();
			};
			
			InitializeComponent();
		}
		
		public abstract void InitializeComponent();
	}
}