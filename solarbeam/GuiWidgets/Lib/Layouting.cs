// Copyright (c) 2009-2014 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide layouting.
	 */
	partial class Widgets
	{
		/**
		 * Copy Visual Studio's layout semantics.
		 */
		public static void DoLayout(Control parent)
		{
			DoDeepLayout(parent);
			parent.ResumeLayout(false);
		}
		
		private static void DoDeepLayout(Control parent)
		{
			foreach (Control child in parent.Controls) {
				DoLayout(child); // preorder
				
				child.ResumeLayout(false);
				child.PerformLayout();
			}
		}
		
		
		private static Panel InitPanel()
		{
			Panel panel = new Panel();
			panel.SuspendLayout();
			return panel;
		}
		
		private static TabControl InitTabControl()
		{
			TabControl tab = new TabControl();
			tab.SuspendLayout();
			return tab;
		}
		
		private static TabPage InitTabPage()
		{
			TabPage tabpage = new TabPage();
			tabpage.SuspendLayout();
			return tabpage;
		}
		
		private static TableLayoutPanel InitTableLayoutPanel()
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.SuspendLayout();
			return layout;
		}
	}
}
