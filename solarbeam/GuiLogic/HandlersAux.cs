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
			Component platform = registry[Id.SHORTCUT_PLATFORM];
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
			
			GuiMainForm.shortcutform.Show();
		}
		
		private static void ShortcutInstall(object sender, EventArgs args)
		{
			bool desktop = GetBool(GetValue(registry[Id.SHORTCUT_DESKTOPCHECK]));
			string desktop_s = GetValue(registry[Id.SHORTCUT_DESKTOP]);
			
			bool startmenu = GetBool(GetValue(registry[Id.SHORTCUT_STARTMENUCHECK]));
			string startmenu_s = GetValue(registry[Id.SHORTCUT_STARTMENU]);
			
			WindowsShortcutInstall wsi = new WindowsShortcutInstall(Controller.AsmInfo);
			if (desktop)
				wsi.ShortcutTo(desktop_s);
			if (startmenu)
				wsi.ShortcutTo(startmenu_s);
		}
		
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
