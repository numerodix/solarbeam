// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
			string path1 = null;
			string path2 = null;
			
			if (pn == PlatformName.Windows) {
				path1 = Platform.GetPath(PathType.Desktop);
				SetValue(registry[Id.SHORTCUT_PATH_1_DETECT], path1);
				SetValue(registry[Id.SHORTCUT_PATH_1_INPUT], path1);
				path2 = Platform.GetPath(PathType.WindowsStartMenu);
				SetValue(registry[Id.SHORTCUT_PATH_2_DETECT], path2);
				SetValue(registry[Id.SHORTCUT_PATH_2_INPUT], path2);
			} else if (pn == PlatformName.Unix) {
				path1 = Platform.GetPath(PathType.LinuxLocalXDGApplications);
				SetValue(registry[Id.SHORTCUT_PATH_1_DETECT], path1);
				SetValue(registry[Id.SHORTCUT_PATH_1_INPUT], path1);
				path2 =  Platform.GetPath(PathType.LinuxGlobalXDGApplications);
				SetValue(registry[Id.SHORTCUT_PATH_2_DETECT], path2);
			}
			
			if (Directory.Exists(path1))
				MarkGood(registry[Id.SHORTCUT_PATH_1_DETECT]);
			else
				MarkError(registry[Id.SHORTCUT_PATH_1_DETECT]);
			
			if (Directory.Exists(path2))
				MarkGood(registry[Id.SHORTCUT_PATH_2_DETECT]);
			else
				MarkError(registry[Id.SHORTCUT_PATH_2_DETECT]);
		}
	}
}
