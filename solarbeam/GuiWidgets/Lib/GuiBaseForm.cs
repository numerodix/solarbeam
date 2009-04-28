// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * A main form with title and icon.
	 */
	abstract class GuiBaseForm : Form
	{
		public GuiBaseForm(string form_title, string icon)
		{
			this.DoubleBuffered = true;
			this.Text = form_title;
			this.Icon = Controller.AsmInfo.GetIcon(icon);

			InitializeComponent();
			
			Widgets.DoLayout(this);
		}
		
		protected abstract void InitializeComponent();
	}
}
