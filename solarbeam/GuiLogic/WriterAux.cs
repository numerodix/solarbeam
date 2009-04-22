// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Set widget values.
	 */
	partial class Controller
	{
		private static void DetectPaths(PlatformName pn)
		{
			if (pn == PlatformName.Windows) {
				string desktop = Platform.GetPath(PathType.Desktop);
				SetValue(registry[Id.SHORTCUT_PATH_1_DETECT], desktop);
				SetValue(registry[Id.SHORTCUT_PATH_1_INPUT], desktop);
				string startmenu = Platform.GetPath(PathType.WindowsStartMenu);
				SetValue(registry[Id.SHORTCUT_PATH_2_DETECT], startmenu);
				SetValue(registry[Id.SHORTCUT_PATH_2_INPUT], startmenu);
			} else if (pn == PlatformName.Unix) {
				string apps = Platform.GetPath(PathType.LinuxLocalApplications);
				SetValue(registry[Id.SHORTCUT_PATH_1_DETECT], apps);
				SetValue(registry[Id.SHORTCUT_PATH_1_INPUT], apps);
			}
		}
	}
}
