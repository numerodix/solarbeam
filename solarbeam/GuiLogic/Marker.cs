// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Sets markings on widgets.
	 */
	partial class Controller
	{
		private static Color NONMARK_BACKCOLOR = (new ComboBox()).BackColor;
		private static Color MARK_BACKCOLOR = Color.Yellow;
	  	private static Color MARKERROR_BACKCOLOR = Color.Pink;
			
		private static void Mark(Control control)
		{
			control.BackColor = MARK_BACKCOLOR;
		}
	  
	 	private static void MarkError(Control control)
		{
			control.BackColor = MARKERROR_BACKCOLOR;
		}
	
		private static void UnMark(Control control)
		{
			control.BackColor = NONMARK_BACKCOLOR;
		}
		
		public static void Enable(Control control)
		{
			control.Enabled = true;
		}
		
		public static void Disable(Control control)
		{
			control.Enabled = false;
		}
	}
}