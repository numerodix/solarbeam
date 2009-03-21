// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide initialization of common gui components.
	 */
	static class Widgets
	{
		public static TableLayoutPanel GetLaidOut(Control[] controls, float[] widths)
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = controls.Length;
			layout.RowCount = 1;
			
			foreach (float width in widths)
			{
				layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, width));
			}
	
			foreach (Control c in controls)
			{
				c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				layout.Controls.Add(c);
			}
			return layout;
		}
		
		public static Button GetButton(Controller.Id id, string s)
		{
			Button button = new Button();
			button.Text = s;
			Controller.RegisterControl(id, button);	// register control
			return button;
		}
		
		public static Label GetLabel(string s)
		{
			Label label = new Label();
			label.Text = s;
			label.AutoSize = true;
			label.Anchor = AnchorStyles.Left;
			return label;
		}
		
		public static TextBox GetTextBox(Controller.Id id, string s)
		{
			TextBox textbox = new TextBox();
			textbox.ReadOnly = true;
			textbox.Anchor = AnchorStyles.Left;
			textbox.BorderStyle = BorderStyle.None;
			Controller.RegisterControl(id, textbox);	// register control
			return textbox;
		}
		
		public static ComboBox GetComboBox(Controller.Id id, string[] ss)
		{
			ComboBox combo = new ComboBox();
			combo.DropDownStyle = ComboBoxStyle.DropDownList;
			foreach (string s in ss) {
				combo.Items.Add(s);
			}
			combo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			Controller.RegisterControl(id, combo);	// register control
			return combo;
		}

		public static ComboBox GetComboBoxList(Controller.Id id, List<string> ss)
		{
			ComboBox combo = new ComboBox();
			combo.DropDownStyle = ComboBoxStyle.DropDownList;
			foreach (string s in ss) {
				combo.Items.Add(s);
			}
			combo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			Controller.RegisterControl(id, combo);	// register control
			return combo;
		}
			
		public static ComboBox GetComboBoxInputable(Controller.Id id, List<string> ss)
		{
			ComboBox combo = GetComboBoxList(id, ss);
			combo.DropDownStyle = ComboBoxStyle.DropDown;
			combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			combo.AutoCompleteSource = AutoCompleteSource.ListItems;
			combo.DropDownHeight = 180;
			return combo;
		}
		
		public static NumericUpDown GetNumericUpDown(Controller.Id id, int min, int max)
		{
			NumericUpDown num = new NumericUpDown();
			num.Minimum = min;
			num.Maximum = max;
			Controller.RegisterControl(id, num);	// register control
			return num;
		}
		
		public static TableLayoutPanel GetTableLayoutPanel(int rows, int cols, 
		                                                   int margin, int padding)
		{
			TableLayoutPanel layout = new TableLayoutPanel();
			layout.Dock = DockStyle.Fill;
			layout.ColumnCount = cols;
			layout.RowCount = rows;
			layout.Padding = new Padding(padding);
			layout.Margin = new Padding(margin);
			return layout;
		}
	}
}