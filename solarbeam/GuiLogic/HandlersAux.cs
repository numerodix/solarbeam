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
			
			DetectPaths(pn);
			
			GuiMainForm.shortcutform.Show();
		}
		
		public static void ShortcutPlatformChange(object sender, EventArgs args)
		{
			// value changes will occur during control initialization, sometimes 
			// before all controls have been registered. ignore this early case
			try {
				PlatformName pn = ReadPlatform();
			
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
				
				// hide path 1 on unix, unused
				if (pn == PlatformName.Unix) {
					SetVisibles(false, new object[] {
						registry[Id.SHORTCUT_PATH_1_CHECK],
						registry[Id.SHORTCUT_PATH_1_CHECKLABEL],
						registry[Id.SHORTCUT_PATH_1_INPUT],
						registry[Id.SHORTCUT_PATH_1_BROWSE_ACTION]});
				} else {
					SetVisibles(true, new object[] {
						registry[Id.SHORTCUT_PATH_1_CHECK],
						registry[Id.SHORTCUT_PATH_1_CHECKLABEL],
						registry[Id.SHORTCUT_PATH_1_INPUT],
						registry[Id.SHORTCUT_PATH_1_BROWSE_ACTION]});
				}
				
				DetectPaths(pn);
			} catch (NullReferenceException) {}
		}
		
		private static void ShortcutBrowse(object sender, EventArgs args)
		{
			Id id = reg_rev[(Component) sender];
			
			// resolve companion textfield
			Component field = null;
			if (id == Id.SHORTCUT_PATH_1_BROWSE_ACTION) {
				field = registry[Id.SHORTCUT_PATH_1_INPUT];
			} else if (id == Id.SHORTCUT_PATH_2_BROWSE_ACTION) {
				field = registry[Id.SHORTCUT_PATH_2_INPUT];
			}
			
			string path = GetValue(field);
			
			FolderBrowserDialog dlg = Widgets.GetFolderBrowserDialog(path);
			DialogResult ans = dlg.ShowDialog();
			path = dlg.SelectedPath;
			
			if (ans == DialogResult.OK) {
				SetValue(field, path);
			}
		}
		
		private static void ShortcutCreate(object sender, EventArgs args)
		{
			bool path1_b = GetBool(GetValue(registry[Id.SHORTCUT_PATH_1_CHECK]));
			bool path2_b = GetBool(GetValue(registry[Id.SHORTCUT_PATH_2_CHECK]));
			
			string path1_s = GetValue(registry[Id.SHORTCUT_PATH_1_INPUT]);
			string path2_s = GetValue(registry[Id.SHORTCUT_PATH_2_INPUT]);
			
			PlatformName pn = ReadPlatform();
			
			if (pn == PlatformName.Windows) {
				ShortcutInstall(pn, path1_b, path1_s);
			}
			ShortcutInstall(pn, path2_b, path2_s);
		}
		
		private static void ShortcutInstall(PlatformName pn, bool flag, string path)
		{
			ShortcutInstaller si = new ShortcutInstaller(Controller.AsmInfo);
			if ((flag) && (path != null) && (path != string.Empty)) {
				try {
					if (pn == PlatformName.Windows) {
						si.WindowsShortcutTo(path);						
					} else {
						si.UnixShortcutTo(path);
					}
					Controller.Report(new Message(Result.OK, "Created shortcut in: " + path));
				} catch {
					Controller.Report(new Message(Result.Fail, "Failed to create shortcut in: " + path));
				}
			}
		}
		
		private static void ShortcutDelete(object sender, EventArgs args)
		{
			bool path1_b = GetBool(GetValue(registry[Id.SHORTCUT_PATH_1_CHECK]));
			bool path2_b = GetBool(GetValue(registry[Id.SHORTCUT_PATH_2_CHECK]));
			
			string path1_s = GetValue(registry[Id.SHORTCUT_PATH_1_INPUT]);
			string path2_s = GetValue(registry[Id.SHORTCUT_PATH_2_INPUT]);
			
			PlatformName pn = ReadPlatform();
			
			if (pn == PlatformName.Windows) {
				ShortcutUninstall(pn, path1_b, path1_s);
			}
			ShortcutUninstall(pn, path2_b, path2_s);
		}

		private static void ShortcutUninstall(PlatformName pn, bool flag, string path)
		{
			ShortcutInstaller si = new ShortcutInstaller(Controller.AsmInfo);
			if ((flag) && (path != null) && (path != string.Empty)) {
				try {
					if (pn == PlatformName.Windows) {
						si.RemoveWindowsShortcut(path);						
					} else {
						si.RemoveUnixShortcut(path);
					}
					Controller.Report(new Message(Result.OK, "Removed shortcut in: " + path));
				} catch {
					Controller.Report(new Message(Result.Fail, "Failed to remove shortcut in: " + path));
				}
			}
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
