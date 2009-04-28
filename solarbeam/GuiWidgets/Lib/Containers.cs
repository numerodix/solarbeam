// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide widget containers.
	 */
	partial class Widgets
	{
		public static readonly string BUTTON_WIDTH = (100).ToString();
		public static readonly string COLON_WIDTH = (10).ToString();
		

		public static Panel GetPanel()
		{
			return InitPanel();
		}
		
		public static TabControl GetTabControl(Control[] controls,
		                                       string[] labels)
		{
			TabControl tabs = InitTabControl();
			tabs.Dock = DockStyle.Fill;
			
			for (int i=0; i<controls.Length; i++) {
				TabPage tabpage = InitTabPage();
				tabpage.Dock = DockStyle.Fill;
				tabpage.Text = labels[i];
				tabpage.Controls.Add(controls[i]);
				tabs.Controls.Add(tabpage);
			}
			
			// give me focus when mouse hovers, so that MouseWheel fires
			tabs.MouseEnter += delegate (object sender, EventArgs args) {
				tabs.Focus();
			};
			
			// cycle tabs on mouse wheel
			tabs.MouseWheel += delegate (object sender, MouseEventArgs args) {
				int len = tabs.TabCount;
				int idx = tabs.SelectedIndex;
				
				int inc = args.Delta > 0 ? -1 : 1;
				idx = (idx + inc) % len;
				idx = idx < 0 ? len + idx : idx;
				
				tabs.SelectedIndex = idx;
			};
			
			return tabs;
		}
		
		public static TableLayoutPanel GetTableLayoutPanel(int rows, int cols, 
		                                                   int margin, int padding)
		{
			TableLayoutPanel layout = InitTableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = cols;
			layout.RowCount = rows;
			layout.Padding = new Padding(padding);
			layout.Margin = new Padding(margin);
			return layout;
		}
		
		public static TableLayoutPanel GetStacked(Control[] controls, string width)
		{
			string[] widths = new string[controls.Length];
			for (int i=0; i<widths.Length; i++) {
				widths[i] = width;
			}
			return GetStacked(controls, widths);
		}
		
		public static TableLayoutPanel GetStacked(Control[] controls, string[] widths)
		{
			TableLayoutPanel layout = InitTableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = 1;
			layout.RowCount = controls.Length;
			
			foreach (string width in widths) {
				float val = 0F;
				SizeType tp = SizeType.Absolute;
				if (width.EndsWith("%")) {
					val = (float) Convert.ToDouble(width.Remove(width.Length - 1));
					tp = SizeType.Percent;
				} else {
					val = (float) Convert.ToDouble(width);
				}
				layout.RowStyles.Add(new RowStyle(tp, val));
			}
	
			foreach (Control c in controls) {
				c.Dock = DockStyle.Fill;
				layout.Controls.Add(c);
			}
			return layout;
		}
		
		public static TableLayoutPanel GetLaidOut(Control[] controls, string[] widths)
		{
			TableLayoutPanel layout = InitTableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = controls.Length;
			layout.RowCount = 1;
			
			foreach (string width in widths) {
				float val = 0F;
				SizeType tp = SizeType.Absolute;
				if (width.EndsWith("%")) {
					val = (float) Convert.ToDouble(width.Remove(width.Length - 1));
					tp = SizeType.Percent;
				} else {
					val = (float) Convert.ToDouble(width);
				}
				layout.ColumnStyles.Add(new ColumnStyle(tp, val));
			}
	
			foreach (Control c in controls) {
				c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				layout.Controls.Add(c);
			}
			return layout;
		}
	}
}
