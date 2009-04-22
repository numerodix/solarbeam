// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using LibSolar;
using LibSolar.Util;

namespace SolarbeamGui
{
	sealed class GuiShortcutInstall : Form
	{
		private const int FORM_ROW_HEIGHT = 32;
		
		public GuiShortcutInstall(string app_title, string icon)
		{
			InitializeComponent(app_title, icon);
		}
		
		public static string GetDescWindows()
		{
			string s = "On Windows, {0} can create shortcuts on the Desktop";
			s += " and in the Start Menu.\n";
			s += "(You can safely rerun this to overwrite any existing {1} icons.)";
			s = string.Format(s, Constants.GUI_APPTITLE, Constants.GUI_APPTITLE);
			
			return s;
		}
		
		public void InitializeComponent(string app_title, string icon)
		{	
			this.DoubleBuffered = true;
			this.Text = "Create shortcuts";
			this.Icon = new Icon(Controller.AsmInfo.GetResource(icon));
			
			this.Controls.Add(GetPanel());
			
//			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.StartPosition = FormStartPosition.CenterParent;
			this.ClientSize = new Size(500, 390);
			
			// prevent disposal by intercepting Close() and calling Hide()
			this.Closing += delegate (object o, CancelEventArgs args) {
				args.Cancel = true;
				this.Hide();
			};
		}
		
		private Control GetPanel()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(3, 1, 5, 5);

			Control platform = GetPlatform();
			Control desc = GetDesc();
			Control pathdetect = GetPathDetect();
			Control create = GetCreate();
			Control closebtn = GetCloseButton();
			
/*
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
*/
		
			layout.Controls.Add(platform, 0, 0);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			
			layout.Controls.Add(desc, 0, 1);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
			
			layout.Controls.Add(pathdetect, 0, 2);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
					
			layout.Controls.Add(create, 0, 3);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
								
			layout.Controls.Add(closebtn, 0, 4);
			layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			
			return layout;
		}
		
		private Control GetPlatform()
		{
			List<string> ss = new List<string>();
			foreach (PlatformName pn in Enum.GetValues(typeof(PlatformName))) {
				ss.Add(pn.ToString());
			}
			ComboBox plat_in = Widgets.GetComboBox(
				Controller.Id.SHORTCUT_PLATFORM,
				ss);

			Control layout = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon("Platform detected"),
					Widgets.GetLabelAnon(":"),
					plat_in},
				new float[] {10F, 2F, 33F});
			
			return layout;
		}
		
		private Control GetDesc()
		{
			string s = GetDescWindows();
			
			Control desc = Widgets.GetRichTextBox(s);
			desc.TabStop = false;
			
			return desc;
		}
		
		private Control GetPathDetect()
		{
			int rows = 2;
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(rows, 1, 5, 5);
			
			Control desktop = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(
						Controller.Id.SHORTCUT_PATH_1_LABEL,
						"Desktop"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxRO(
						Controller.Id.SHORTCUT_PATH_1_DETECT,
						string.Empty)},
				new float[] {10F, 2F, 33F});
			
			Control startmenu = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(
						Controller.Id.SHORTCUT_PATH_2_LABEL,
						"Start menu"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxRO(
						Controller.Id.SHORTCUT_PATH_2_DETECT,
						string.Empty)},
				new float[] {10F, 2F, 33F});
			
			layout.Controls.Add(desktop, 0, 0);
			layout.Controls.Add(startmenu, 0, 1);
			
			for (int i = 0; i < layout.Controls.Count; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			GroupBox outputs = new GroupBox();
			outputs.Text = "Detecting paths";
			outputs.Dock = DockStyle.Fill;
			outputs.Controls.Add(layout);
			
			return outputs;
		}
		
		private Control GetCreate()
		{
			int rows = 3;
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(rows, 1, 5, 5);

			Control desktop = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_PATH_1_CHECK,
						"Desktop",
						true),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxRO(
						Controller.Id.SHORTCUT_PATH_1_INPUT,
						string.Empty),
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUT_PATH_1_BROWSE_ACTION,
						"Browse", "browse.png")},
				new float[] {15F, 2F, 33F, 10F});
			
			Control startmenu = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_PATH_2_CHECK,
						"Start menu",
						true),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxRO(
						Controller.Id.SHORTCUT_PATH_2_INPUT,
						string.Empty),
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUT_PATH_2_BROWSE_ACTION,
						"Browse", "browse.png")},
				new float[] {15F, 2F, 33F, 10F});
			
			Control buttons = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon(String.Empty), //layout buffer
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUTINSTALL_ACTION,
						"&Create",
						"new.png")},
				new float[] {80F, 20F});
			
			layout.Controls.Add(desktop, 0, 0);
			layout.Controls.Add(startmenu, 0, 1);
			layout.Controls.Add(buttons, 0, 2);

			for (int i = 0; i < layout.Controls.Count; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, FORM_ROW_HEIGHT));
			}
			
			GroupBox outputs = new GroupBox();
			outputs.Text = "Create shortcut(s)";
			outputs.Dock = DockStyle.Fill;
			outputs.Controls.Add(layout);
			
			return outputs;
		}
		
		private Control GetCloseButton()
		{
			Control buttons = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabelAnon(String.Empty), //layout buffer
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUTCLOSE_ACTION,
						"&Close",
						"app-exit.png")},
				new float[] {80F, 20F});
			
			return buttons;
		}
	}
}
