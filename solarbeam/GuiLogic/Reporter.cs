// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Report status.
	 */
	partial class Controller
	{
		public static void Report(Message msg)
		{
			((ToolStripStatusLabel) registry[Id.STATUSBAR_LABEL]).Text = msg.ToString();
			Bitmap icon = null;
			if (msg.Result == Result.OK) {
				icon = Controller.AsmInfo.GetBitmap("status-ok.png");
			} else if (msg.Result == Result.Fail) {
				icon = Controller.AsmInfo.GetBitmap("status-fail.png");
			}
			((ToolStripStatusLabel) registry[Id.STATUSBAR_LABEL]).Image = icon;
		}
	}
}
