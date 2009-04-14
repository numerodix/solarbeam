// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using LibSolar.Util;

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
			Control logo = GetLogo();
			Control tabs = GetTabs();
			int tabs_height = 180;
			
			Button close_btn = Widgets.GetButtonImageText(
				Controller.Id.ABOUTCLOSE_ACTION,
				"&Close", "app-exit.png");
			close_btn.Anchor = AnchorStyles.Right;
			close_btn.TabIndex = 0;
			
			TableLayoutPanel table = Widgets.GetTableLayoutPanel(3, 1, 0, 0);
			
			table.RowStyles.Add(new RowStyle(SizeType.Absolute, logo.Height));
			table.RowStyles.Add(new RowStyle(SizeType.Absolute, tabs_height));
			table.RowStyles.Add(new RowStyle(SizeType.Absolute, close_btn.Height));
			
			table.Controls.Add(logo, 0, 0);
			table.Controls.Add(tabs, 0, 1);
			table.Controls.Add(close_btn, 0, 2);
			
			this.Controls.Add(table);
			
			this.DoubleBuffered = true;
			this.Text = "About " + app_title;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ClientSize = new Size(logo.Width + 2*3,
			                           logo.Height + tabs_height + close_btn.Height + 24);
			
			// prevent disposal by intercepting Close() and calling Hide()
			this.Closing += delegate (object o, CancelEventArgs args) {
				args.Cancel = true;
				this.Hide();
			};
		}
		
		private Control GetLogo()
		{
			Panel panel = new Panel();
			Bitmap logo = new Bitmap(Controller.AsmInfo.GetResource("logo.png"));
			panel.BackgroundImage = logo;
			panel.Size = logo.Size;
			
			Label label = new Label();
			label.Text = "Version " + Controller.AsmInfo.GetAtt("Version");
			label.AutoSize = true;
			label.Location = new Point(10, 170);
			label.BackColor = Color.White;
			label.ForeColor = Color.OrangeRed;
			panel.Controls.Add(label);
			
			return panel;
		}
		
		private Control GetTabs()
		{
			TabControl tabs = new TabControl();
			tabs.Dock = DockStyle.Fill;
			
			tabs.Controls.Add(GetAbout());
			tabs.Controls.Add(GetLibs());
			tabs.Controls.Add(GetLicense());
			
			return tabs;
		}
		
		private TabPage GetAbout()
		{
			TabPage tab = new TabPage();
			tab.Dock = DockStyle.Fill;
			tab.Text = "About";
			
			string s;
			
			s = String.Format("{0} {1}  {2}: {3}, {4}: {5}\n",
			                  Controller.AsmInfo.GetAtt("Title"),
			                  Controller.AsmInfo.GetAtt("Version"),
			                  "{runtime",
			                  Platform.GetRuntime(),
			                  "platform",
			                  Platform.GetPlatform() + "}");
			s += Controller.AsmInfo.GetAtt("Description") + "\n\n";
			
			s += Controller.AsmInfo.GetAtt("Copyright") + "\n\n";
			
			s += "This application was developed in cooperation with";
			s += " the Norwegian University of Science and Technology (NTNU),";
			s += " Norway, and sponsored by The Research Council of Norway.\n\n";
			
			s += "Coordinator: Professor Barbara Matusiak,";
			s += " Department of Architectural Design, Form and Colour Studies, NTNU";

			RichTextBox txt = Widgets.GetRichTextBox(s);
			tab.Controls.Add(txt);
			return tab;
		}
		
		private TabPage GetLibs()
		{
			TabPage tab = new TabPage();
			tab.Dock = DockStyle.Fill;
			tab.Text = "Libraries";
			RichTextBox txt = Widgets.GetRichTextBox(
					Controller.AsmInfo.GetString("COPYING.short"));
			tab.Controls.Add(txt);
			return tab;
		}
		
		private TabPage GetLicense()
		{
			TabPage tab = new TabPage();
			tab.Dock = DockStyle.Fill;
			tab.Text = "License";
			RichTextBox txt = Widgets.GetRichTextBox(
					Controller.AsmInfo.GetString("LICENSE"));
			tab.Controls.Add(txt);
			return tab;
		}
	}
}
