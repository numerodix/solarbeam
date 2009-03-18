// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System.Windows.Forms;

namespace SolarbeamGui
{
	sealed class GuiMenu : MenuStrip
	{
		public GuiMenu()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			ToolStripMenuItem fileToolStripMenuItem = new ToolStripMenuItem();
			ToolStripMenuItem exitToolStripMenuItem = new ToolStripMenuItem();
			  
			// mainMenuStrip
			this.Items.AddRange(new ToolStripItem[] {fileToolStripMenuItem});
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "mainMenuStrip";
			this.Size = new System.Drawing.Size(244, 24);
			this.TabIndex = 0;

			// fileToolStripMenuItem
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {exitToolStripMenuItem});
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Text = "&File";
			fileToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
			
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            exitToolStripMenuItem.Text = "Exit";
		}
	}
}