// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiAbout : Form
	{
		public GuiAbout()
		{
			InitializeComponent();
		}
		
		public void InitializeComponent()
		{
			this.Size = new Size(500, 300);
		}
	}
}