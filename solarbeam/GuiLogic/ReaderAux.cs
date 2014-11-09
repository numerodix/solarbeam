// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Windows.Forms;

using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Read widget values.
	 */
	partial class Controller
	{
		private static PlatformName ReadPlatform()
		{
			string pn_s = GetValue(registry[Id.SHORTCUT_PLATFORM]);
			PlatformName pn = (PlatformName) Enum.Parse(typeof(PlatformName), pn_s);
			return pn;
		}
	}
}
