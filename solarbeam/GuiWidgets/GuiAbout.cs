// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using LibSolar;
using LibSolar.Util;

namespace SolarbeamGui
{
	sealed class GuiAbout : GuiBaseChildForm
	{
		private Control logo;
		private Button close_btn;
		private int TABS_HEIGHT = 202;
		
		
		public GuiAbout(string form_title, string icon)
			: base(form_title, icon) {}
		
		protected override void InitializeComponent()
		{	
			Control panel = GetPanel();
			this.Controls.Add(panel);

			this.ClientSize = new Size(logo.Width + 2*3,
			                           logo.Height + TABS_HEIGHT + close_btn.Height + 24);
		}
		
		private Control GetPanel()
		{
			logo = GetLogo();
			Control tabs = GetTabs();
			
			close_btn = Widgets.GetButtonImageText(
				Controller.Id.ABOUTCLOSE_ACTION,
				"&Close", "app-exit.png");
			close_btn.Anchor = AnchorStyles.Right;
			close_btn.TabIndex = 0;
			Control btn = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon(string.Empty),
					close_btn},
				new string[] {"100%", Widgets.BUTTON_WIDTH});
			
			Control container = Widgets.GetStacked(
				new Control[] {
					logo,
					tabs,
					btn},
				new string[] {
					logo.Height.ToString(),
					TABS_HEIGHT.ToString(),
					close_btn.Height.ToString()});
			
			return container;
		}
		
		private Control GetLogo()
		{
			Panel panel = Widgets.GetPanel();
			Bitmap logo = Controller.AsmInfo.GetBitmap("logo.png");
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
			
			s = String.Format("{0} {1}  {2}\n",
			                  Controller.AsmInfo.GetAtt("Title"),
			                  Controller.AsmInfo.GetAtt("Version"),
			                  Platform.GetRuntimePlatformString());
			s += Controller.AsmInfo.GetAtt("Description") + "\n\n";
			
			s += Constants.APP_URL + "\n\n";
			
			s += Controller.AsmInfo.GetAtt("Copyright") + "\n\n";
			
			s += "This application was developed in cooperation with";
			s += " the Norwegian University of Science and Technology (NTNU),";
			s += " Norway, and sponsored by The Research Council of Norway.\n\n";
			
			s += "Coordinator: Professor Barbara Matusiak,";
			s += " Department of Architectural Design, Form and Colour Studies, NTNU";

			return Widgets.GetRichTextBoxAnon(s);
		}
		
		private Control GetLibs()
		{
			return Widgets.GetRichTextBoxAnon(
					Controller.AsmInfo.GetString("COPYING.short"));
		}
		
		private Control GetLicense()
		{
			return Widgets.GetRichTextBoxAnon(
					Controller.AsmInfo.GetString("LICENSE"));
		}
	}
}
