// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using LibSolar.Types;

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
			
		private static void Mark(object obj)
		{
			MarkWidget(obj, MARK_BACKCOLOR);
		}
	  
	 	private static void MarkError(object obj)
		{
			MarkWidget(obj, MARKERROR_BACKCOLOR);
		}
	
		private static void UnMark(object obj)
		{
			MarkWidget(obj, NONMARK_BACKCOLOR);
		}
		
		private static void MarkWidget(object obj, Color color)
		{
			if (obj is Control) {
				((Control) obj).BackColor = color;
			} else if (obj is StaticList<Id>) {
				foreach (Id id in (StaticList<Id>) obj) {
					Component control = registry[id];
					((Control) control).BackColor = color;
				}
			}
		}
	}
}