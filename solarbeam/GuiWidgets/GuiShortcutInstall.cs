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
			
			this.Controls.Add(GetPanel());
			
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ClientSize = new Size(500, 276);
			
			// prevent disposal by intercepting Close() and calling Hide()
			this.Closing += delegate (object o, CancelEventArgs args) {
				args.Cancel = true;
				this.Hide();
			};
		}
		
		public Control GetPanel()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(3, 1, 5, 5);

			string s = "On Windows, {0} can create shortcuts on the Desktop";
			s += " and in the Start Menu.\n";
			s += "(You can safely rerun this to overwrite any existing {1} icons.)";
			s = string.Format(s, Constants.GUI_APPTITLE, Constants.GUI_APPTITLE);
			
			Control desc_in = Widgets.GetRichTextBox(s);
			desc_in.TabStop = false;
			
			Label icon_lbl = Widgets.GetLabelImage("icon64.png");
			icon_lbl.Dock = DockStyle.Fill;
			
			TableLayoutPanel desc = new TableLayoutPanel();
			desc.ColumnCount = 2;
			desc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
			desc.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
			desc.Controls.Add(icon_lbl, 1, 0);
			desc.Controls.Add(desc_in, 0, 0);
			desc.Dock = DockStyle.Fill;
			desc.RowCount = 1;
			desc.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			
			Control inputs = GetInputs();
			
			Control buttons = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(String.Empty), //layout buffer
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUTCLOSE_ACTION,
						"&Close",
						"app-exit.png")},
				new float[] {80F, 20F});
			
			layout.Controls.Add(desc, 0, 0);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
			
			layout.Controls.Add(inputs, 0, 1);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
			
			layout.Controls.Add(buttons, 0, 2);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
			
			return layout;
		}
		
		public Control GetInputs()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(3, 1, 5, 5);
			
			TextBox plat_in = Widgets.GetTextBox(
						Controller.Id.SHORTCUT_PLATFORM,
						string.Empty);
			plat_in.TabStop = false;
			Control platform = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel("Platform detected"),
					Widgets.GetLabel(":"),
					plat_in},
				new float[] {12F, 2F, 31F});
			
			Control desktop = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_DESKTOPCHECK,
						"Desktop",
						true),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(
						Controller.Id.SHORTCUT_DESKTOP,
						string.Empty)},
				new float[] {12F, 2F, 31F});
			
			Control startmenu = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_STARTMENUCHECK,
						"Start menu",
						true),
					Widgets.GetLabel(":"),
					Widgets.GetTextBox(
						Controller.Id.SHORTCUT_STARTMENU,
						string.Empty)},
				new float[] {12F, 2F, 31F});
			
			Control buttons = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(String.Empty),
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUTINSTALL_ACTION,
						"&Create",
						"new.png")},
				new float[] {80F, 20F});
			
			layout.Controls.Add(platform, 0, 0);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
			
			layout.Controls.Add(desktop, 0, 1);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			
			layout.Controls.Add(startmenu, 0, 2);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			
			layout.Controls.Add(buttons, 0, 3);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			
			GroupBox outputs = new GroupBox();
			outputs.Text = "Settings";
			outputs.Dock = DockStyle.Fill;
			outputs.Controls.Add(layout);
			
			return outputs;
		}
	}
}
