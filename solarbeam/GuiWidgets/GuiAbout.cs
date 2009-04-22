// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using LibSolar;
using LibSolar.Util;

namespace SolarbeamGui
{
	sealed class GuiAbout : Form
	{
		public GuiAbout(string app_title, string icon)
		{
			InitializeComponent(app_title, icon);
		}
		
		public void InitializeComponent(string app_title, string icon)
		{	
			this.DoubleBuffered = true;
			this.Text = "About " + app_title;
			this.Icon = new Icon(Controller.AsmInfo.GetResource(icon));
			
			Control logo = GetLogo();
			Control tabs = GetTabs();
			int tabs_height = 202;
			
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
			Control about = GetAbout();
			Control libs = GetLibs();
			Control lic = GetLicense();
		
			TabControl tabs = Widgets.GetTabControl(
				new Control[] {
					about,
					libs,
					lic},
				new string[] {
					"About",
					"Libraries",
					"License"});
			
			return tabs;
		}
		
		private Control GetAbout()
		{
			string s;
			
			s = String.Format("{0} {1}  {2}: {3}, {4}: {5}\n",
			                  Controller.AsmInfo.GetAtt("Title"),
			                  Controller.AsmInfo.GetAtt("Version"),
			                  "{runtime",
			                  Platform.ToString(Platform.GetRuntime()),
			                  "platform",
			                  Platform.GetPlatform() + "}");
			s += Controller.AsmInfo.GetAtt("Description") + "\n\n";
			
			s += Constants.APP_URL + "\n\n";
			
			s += Controller.AsmInfo.GetAtt("Copyright") + "\n\n";
			
			s += "This application was developed in cooperation with";
			s += " the Norwegian University of Science and Technology (NTNU),";
			s += " Norway, and sponsored by The Research Council of Norway.\n\n";
			
			s += "Coordinator: Professor Barbara Matusiak,";
			s += " Department of Architectural Design, Form and Colour Studies, NTNU";

			return Widgets.GetRichTextBox(s);
		}
		
		private Control GetLibs()
		{
			return Widgets.GetRichTextBox(
					Controller.AsmInfo.GetString("COPYING.short"));
		}
		
		private Control GetLicense()
		{
			return Widgets.GetRichTextBox(
					Controller.AsmInfo.GetString("LICENSE"));
		}
	}
}
