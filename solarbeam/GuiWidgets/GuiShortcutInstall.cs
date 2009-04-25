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
	sealed class GuiShortcutInstall : GuiBaseChildForm
	{
		private static readonly string KEY_WIDTH = (100).ToString();
		private const int FORM_ROW_HEIGHT = 32;
		
		private Dictionary<string,string> dict;
		
		
		public GuiShortcutInstall(string form_title, string icon)
			: base(form_title, icon)
		{
			InitializeStrings();
		}
		
		
		private void InitializeStrings()
		{
			SetString(PlatformName.Windows, Controller.Id.SHORTCUT_DESC, GetDescWindows());
			SetString(PlatformName.Windows, Controller.Id.SHORTCUT_PATH_1_LABEL, "Desktop");
			SetString(PlatformName.Windows, Controller.Id.SHORTCUT_PATH_2_LABEL, "Start Menu");
			SetString(PlatformName.Windows, Controller.Id.SHORTCUT_PATH_1_CHECK, "Desktop");
			SetString(PlatformName.Windows, Controller.Id.SHORTCUT_PATH_2_CHECK, "Start Menu");
			SetString(PlatformName.Unix, Controller.Id.SHORTCUT_DESC, GetDescUnix());
			SetString(PlatformName.Unix, Controller.Id.SHORTCUT_PATH_1_LABEL, "XDG global");
			SetString(PlatformName.Unix, Controller.Id.SHORTCUT_PATH_2_LABEL, "XDG local");
			SetString(PlatformName.Unix, Controller.Id.SHORTCUT_PATH_1_CHECK, string.Empty);
			SetString(PlatformName.Unix, Controller.Id.SHORTCUT_PATH_2_CHECK, "XDG local");
		}
		
		private void SetString(PlatformName pn, Controller.Id id, string s)
		{
			if (dict == null) {
				dict = new Dictionary<string,string>();
			}
			dict.Add(pn.ToString() + id.ToString(), s);
		}
		
		public string GetString(PlatformName pn, Controller.Id id)
		{
			return dict[pn.ToString() + id.ToString()];
		}	
		
		
		public static string GetDescWindows()
		{
			string s = "On Windows, {0} can create shortcuts on the Desktop";
			s += " and in the Start Menu.\n";
			s += "(You can safely rerun this to overwrite any existing {1} shortcuts.)";
			s = string.Format(s, Constants.GUI_APPTITLE, Constants.GUI_APPTITLE);
			
			return s;
		}
		
		public static string GetDescUnix()
		{
			string s = "On Unix, {0} can create a launcher in the application";
			s += " menu in accordance with the http://freedesktop.org standard";
			s += " (supported by Gnome, KDE and other desktop environments).";
			s += "\n(You can safely rerun this to overwrite any existing {1} launcher.)";
			s = string.Format(s, Constants.GUI_APPTITLE, Constants.GUI_APPTITLE);
			
			return s;
		}
		
		
		protected override void InitializeComponent()
		{	
			this.Controls.Add(GetPanel());
			
			this.ClientSize = new Size(550, 350);
		}
		
		private Control GetPanel()
		{
			Control platform = GetPlatform();
			Control desc = GetDesc();
			Control pathdetect = GetPathDetect();
			Control create = GetCreate();
			Control closebtn = GetCloseButton();
		
			Control layout = Widgets.GetStacked(
				new Control[] {
					platform,
					desc,
					pathdetect,
					create,
					closebtn},
				new string[] {
					FORM_ROW_HEIGHT.ToString(),
					(50).ToString(),
					(100).ToString(),
					(130).ToString(),
					FORM_ROW_HEIGHT.ToString()});
			
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
				new string[] {KEY_WIDTH, Widgets.COLON_WIDTH, "100%"});
			
			return layout;
		}
		
		private Control GetDesc()
		{
			string s = GetDescWindows();
			
			Control desc = Widgets.GetRichTextBox(Controller.Id.SHORTCUT_DESC, s);
			desc.TabStop = false;
			
			return desc;
		}
		
		private Control GetPathDetect()
		{
			string[] fmt = new string[] {KEY_WIDTH, Widgets.COLON_WIDTH, "100%"};
			
			Control desktop = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(
						Controller.Id.SHORTCUT_PATH_1_LABEL,
						"Desktop"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxRO(
						Controller.Id.SHORTCUT_PATH_1_DETECT,
						string.Empty)},
				fmt);
			
			Control startmenu = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetLabel(
						Controller.Id.SHORTCUT_PATH_2_LABEL,
						"Start menu"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxRO(
						Controller.Id.SHORTCUT_PATH_2_DETECT,
						string.Empty)},
				fmt);
						
			Control layout = Widgets.GetStacked(
				new Control[] {
					desktop,
					startmenu},
				FORM_ROW_HEIGHT.ToString());
			
			return Widgets.GetGroupBox("Detected paths", layout);
		}
		
		private Control GetCreate()
		{
			string[] fmt = new string[] {KEY_WIDTH, Widgets.COLON_WIDTH, "100%", "100"};

			Control desktop = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_PATH_1_CHECK,
						"Desktop",
						true),
					Widgets.GetLabel(
						Controller.Id.SHORTCUT_PATH_1_CHECKLABEL,
						":"),
					Widgets.GetTextBoxRW(
						Controller.Id.SHORTCUT_PATH_1_INPUT,
						string.Empty),
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUT_PATH_1_BROWSE_ACTION,
						"Browse", "browse.png")},
				fmt);
			
			Control startmenu = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetCheckBox(
						Controller.Id.SHORTCUT_PATH_2_CHECK,
						"Start menu",
						true),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxRW(
						Controller.Id.SHORTCUT_PATH_2_INPUT,
						string.Empty),
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUT_PATH_2_BROWSE_ACTION,
						"Browse", "browse.png")},
				fmt);
			
			Control buttons = Widgets.GetLaidOut(
				new Control[] {
					Widgets.GetButtonImageText(
						Controller.Id.SHORTCUTINSTALL_ACTION,
						"&Create",
						"new.png"),
					Widgets.GetLabelAnon(String.Empty)},
				new string[] {Widgets.BUTTON_WIDTH, "100%"});
			
			Control layout = Widgets.GetStacked(
				new Control[] {
					desktop,
					startmenu,
					buttons},
				FORM_ROW_HEIGHT.ToString());
			
			return Widgets.GetGroupBox("Create shortcut(s)", layout);
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
				new string[] {"100%", Widgets.BUTTON_WIDTH});
			
			return buttons;
		}
	}
}
