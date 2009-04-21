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
	sealed class GuiShortcutInstall : Form
	{
		public GuiShortcutInstall(string app_title, string icon)
		{
			InitializeComponent(app_title, icon);
		}
		
		public void InitializeComponent(string app_title, string icon)
		{	
			this.DoubleBuffered = true;
			this.Text = "Create shortcuts";
			this.Icon = new Icon(Controller.AsmInfo.GetResource(icon));
			
			this.Controls.Add(GetInputs());
			
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ClientSize = new Size(450, 200);
			
			// prevent disposal by intercepting Close() and calling Hide()
			this.Closing += delegate (object o, CancelEventArgs args) {
				args.Cancel = true;
				this.Hide();
			};
		}
		
		public Control GetInputs()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(3, 1, 5, 5);

			Label title = Widgets.GetLabel("Create shortcuts");
			title.Font = new Font(Font.SystemFontName, 10);
			
			string s = "On Windows, {0} can create shortcuts on the Desktop";
			s += " and in the Start Menu.";
			s = string.Format(s, Constants.GUI_APPTITLE);
			TextBox desc = Widgets.GetTextBoxAnon(s);
			
			TextBox plat_in = Widgets.GetTextBox(
						Controller.Id.SHORTCUT_PLATFORM,
						string.Empty);
			plat_in.TabStop = false;
			Control platform = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(
						"Platform detected:"),
					plat_in},
				new float[] {25F, 75F});
			
			Control desktop = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_DESKTOPCHECK,
						"Desktop:",
						true),
					Widgets.GetTextBox(
						Controller.Id.SHORTCUT_DESKTOP,
						string.Empty)},
				new float[] {25F, 75F});
			
			Control startmenu = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_STARTMENUCHECK,
						"Start menu:",
						true),
					Widgets.GetTextBox(
						Controller.Id.SHORTCUT_STARTMENU,
						string.Empty)},
				new float[] {25F, 75F});
			
			Control buttons = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUTINSTALL_ACTION,
						"&Create",
						"new.png"),
					Widgets.GetLabel(String.Empty), //layout buffer
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUTCLOSE_ACTION,
						"&Close",
						"app-exit.png")},
				new float[] {20F, 60F, 20F});
			
			layout.Controls.Add(title, 0, 0);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			
			layout.Controls.Add(desc, 0, 1);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
			
			layout.Controls.Add(platform, 0, 2);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
			
			layout.Controls.Add(desktop, 0, 3);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			
			layout.Controls.Add(startmenu, 0, 4);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			
			layout.Controls.Add(buttons, 0, 5);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
			
			return layout;
		}
	}
}
