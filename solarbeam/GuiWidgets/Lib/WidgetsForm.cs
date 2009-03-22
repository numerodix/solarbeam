// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using LibSolar.Assemblies;

namespace SolarbeamGui
{
	/**
	 * Provide initialization of form widgets.
	 */
	partial class Widgets
	{	
		public static Button GetButtonImaged(Controller.Id id, string tip,
		                                     string s, string img)
		{
			Button button = new Button();
			button.Image = new Bitmap(Controller.AsmInfo.GetResource(img));
			button.Text = s;
			button.TextImageRelation = TextImageRelation.ImageBeforeText;
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 0;
			Controller.RegisterControl(id, tip, button);	// register control
			return button;
		}
		
		public static ComboBox GetComboBox(Controller.Id id, string tip,
		                                   List<string> ss)
		{
			ComboBox combo = new ComboBox();
			foreach (string s in ss) {
				combo.Items.Add(s);
			}
			combo.DropDownStyle = ComboBoxStyle.DropDownList;
			combo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			combo.DropDownHeight = 180;
			Controller.RegisterControl(id, tip, combo);	// register control
			return combo;
		}
		
		public static ComboBox GetComboBoxInputable(Controller.Id id, string tip, 
		                                            List<string> ss)
		{
			ComboBox combo = GetComboBox(id, tip, ss);
			combo.DropDownStyle = ComboBoxStyle.DropDown;
			combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			combo.AutoCompleteSource = AutoCompleteSource.ListItems;
			combo.Sorted = true;
			return combo;
		}
		
		public static Label GetLabel(string s)
		{
			Label label = new Label();
			label.Text = s;
			label.AutoSize = true;
			label.Anchor = AnchorStyles.Left;
			return label;
		}
		
		public static NumericUpDown GetNumericUpDown(Controller.Id id, string tip,
		                                             int min, int max)
		{
			NumericUpDown num = new NumericUpDown();
			num.Minimum = min;
			num.Maximum = max;
			Controller.RegisterControl(id, tip, num);	// register control
			return num;
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
		
		public static ToolTip GetToolTipInfo(string title)
		{
			ToolTip tooltip = new ToolTip();
			tooltip.ToolTipTitle = title;
			tooltip.ToolTipIcon = ToolTipIcon.Info;
			return tooltip;
		}
	}
}
