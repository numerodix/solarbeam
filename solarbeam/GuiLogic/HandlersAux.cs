// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using LibSolar;
using LibSolar.Formatting;
using LibSolar.Graphing;
using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Handlers for gui events.
	 */
	partial class Controller
	{
		private static void ShowShortcutDialog(object sender, EventArgs args)
		{
			PlatformName pn = Platform.GetPlatform();
			
			SetValue(registry[Id.SHORTCUT_PLATFORM], pn.ToString());
			
/*			Component platform = registry[Id.SHORTCUT_PLATFORM];
			Component desktop = registry[Id.SHORTCUT_DESKTOP];
			Component startmenu = registry[Id.SHORTCUT_STARTMENU];
			
			Button create_btn = (Button) registry[Id.SHORTCUTINSTALL_ACTION];
			
			create_btn.Enabled = true;
			PlatformName platform_name = Platform.GetPlatform();
			if (platform_name == PlatformName.Windows) {
				SetValue(desktop, Platform.GetPath(PathType.Desktop));
				SetValue(startmenu, Platform.GetPath(PathType.WindowsStartMenu));
			} else {
				create_btn.Enabled = false;
			}
			SetValue(platform, platform_name.ToString());
*/			
			GuiMainForm.shortcutform.Show();
		}
		
		private static void ShortcutPlatformChange(object sender, EventArgs args)
		{
			// value changes will occur during control initialization, sometimes 
			// before all controls have been registered. ignore this early case
			try {
				string pn_s = GetValue(registry[Id.SHORTCUT_PLATFORM]);
				PlatformName pn = (PlatformName) Enum.Parse(typeof(PlatformName), pn_s);
			
				// update labels
				string desc = GuiMainForm.shortcutform.GetString(pn, Id.SHORTCUT_DESC);
				((RichTextBox) registry[Id.SHORTCUT_DESC]).Text = desc;
				
				string p1_lbl = GuiMainForm.shortcutform.GetString(pn, Id.SHORTCUT_PATH_1_LABEL);
				((Label) registry[Id.SHORTCUT_PATH_1_LABEL]).Text = p1_lbl;
				
				string p2_lbl = GuiMainForm.shortcutform.GetString(pn, Id.SHORTCUT_PATH_2_LABEL);
				((Label) registry[Id.SHORTCUT_PATH_2_LABEL]).Text = p2_lbl;
				
				string p1_chk = GuiMainForm.shortcutform.GetString(pn, Id.SHORTCUT_PATH_1_CHECK);
				((CheckBox) registry[Id.SHORTCUT_PATH_1_CHECK]).Text = p1_chk;
				
				string p2_chk = GuiMainForm.shortcutform.GetString(pn, Id.SHORTCUT_PATH_2_CHECK);
				((CheckBox) registry[Id.SHORTCUT_PATH_2_CHECK]).Text = p2_chk;
				
				// hide path 2 on unix, unused
				if (pn == PlatformName.Unix) {
					SetVisibles(false, new object[] {
						registry[Id.SHORTCUT_PATH_2_CHECK],
						registry[Id.SHORTCUT_PATH_2_CHECKLABEL],
						registry[Id.SHORTCUT_PATH_2_INPUT],
						registry[Id.SHORTCUT_PATH_2_BROWSE_ACTION]});
				} else {
					SetVisibles(true, new object[] {
						registry[Id.SHORTCUT_PATH_2_CHECK],
						registry[Id.SHORTCUT_PATH_2_CHECKLABEL],
						registry[Id.SHORTCUT_PATH_2_INPUT],
						registry[Id.SHORTCUT_PATH_2_BROWSE_ACTION]});
				}
			} catch (NullReferenceException) {}
		}
		
		private static void ShortcutInstall(object sender, EventArgs args)
		{
/*			bool desktop = GetBool(GetValue(registry[Id.SHORTCUT_DESKTOPCHECK]));
			string desktop_s = GetValue(registry[Id.SHORTCUT_DESKTOP]);
			
			bool startmenu = GetBool(GetValue(registry[Id.SHORTCUT_STARTMENUCHECK]));
			string startmenu_s = GetValue(registry[Id.SHORTCUT_STARTMENU]);
			
			WindowsShortcutInstall wsi = new WindowsShortcutInstall(Controller.AsmInfo);
			if (desktop)
				wsi.ShortcutTo(desktop_s);
			if (startmenu)
				wsi.ShortcutTo(startmenu_s);
*/		}
		
		private static void HideShortcutDialog(object sender, EventArgs args)
		{
			GuiMainForm.shortcutform.Close();
		}
		
		private static void ShowAboutDialog(object sender, EventArgs args)
		{
			GuiMainForm.aboutform.Show();
		}
		
		private static void HideAboutDialog(object sender, EventArgs args)
		{
			GuiMainForm.aboutform.Close();
		}
	}
}
