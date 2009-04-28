// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SolarbeamGui
{
	/**
	 * Provide initialization of form widgets.
	 */
	static partial class Widgets
	{	
		public static Button GetButtonImageTextAnon(Controller.Id id,
		                                            string s, string img)
		{
			Button button = GetButtonImageText(id, s, img);
			button.TabStop = false;
			return button;
		}

		public static Button GetButtonImageText(Controller.Id id,
		                                        string s, string img)
		{
			Button button = new Button();
			button.Image = Controller.AsmInfo.GetBitmap(img);
			button.Text = s;
			button.TextImageRelation = TextImageRelation.ImageBeforeText;
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 0;
			Controller.RegisterControl(id, button);	// register control
			return button;
		}
		
		public static Button GetButtonImage(Controller.Id id, string img)
		{
			Button button = new Button();
			button.Image = Controller.AsmInfo.GetBitmap(img);
//			button.MinimumSize = button.Image.Size;
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 0;
			button.TabStop = false;
			Controller.RegisterControl(id, button);	// register control
			return button;
		}
		
		public static CheckBox GetCheckBox(Controller.Id id, string caption,
		                                   bool val)
		{
			CheckBox check = new CheckBox();
			check.Text = caption;
			check.Checked = val;
			Controller.RegisterControl(id, check);	// register control
			return check;
		}
		
		public static ComboBox GetComboBox(Controller.Id id, List<string> ss)
		{
			ComboBox combo = new ComboBox();
			foreach (string s in ss) {
				combo.Items.Add(s);
			}
			combo.DropDownStyle = ComboBoxStyle.DropDownList;
			combo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			combo.DropDownHeight = 180;
			Controller.RegisterControl(id, combo);	// register control
			return combo;
		}
		
		public static ComboBox GetComboBoxInputable(Controller.Id id, List<string> ss)
		{
			ComboBox combo = GetComboBox(id, ss);
			combo.DropDownStyle = ComboBoxStyle.DropDown;
			combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			combo.AutoCompleteSource = AutoCompleteSource.ListItems;
			combo.Sorted = true;
			return combo;
		}
		
		public static GroupBox GetGroupBox(string title, Control control)
		{
			GroupBox group = new GroupBox();
			group.Text = title;
			group.Dock = DockStyle.Fill;
			group.Controls.Add(control);
			return group;	
		}
		
		public static Label GetLabelAnon(string s)
		{
			Label label = new Label();
			label.Text = s;
			label.AutoSize = true;
			label.Anchor = AnchorStyles.Left;
			return label;
		}
			
		public static Label GetLabel(Controller.Id id, string s)
		{
			Label label = GetLabelAnon(s);
			Controller.RegisterControl(id, label);	// register control
			return label;
		}
		
		public static Label GetLabelImage(Controller.Id id, string img)
		{
			Label label = new Label();
			label.Image = Controller.AsmInfo.GetBitmap(img);
			Controller.RegisterControl(id, label);	// register control
			return label;
		}
		
		public static NumericUpDown GetNumericUpDown(Controller.Id id,
		                                             int min, int max)
		{
			NumericUpDown num = new NumericUpDown();
			num.Minimum = min;
			num.Maximum = max;
			Controller.RegisterControl(id, num);	// register control
			return num;
		}
		
		public static RichTextBox GetRichTextBoxAnon(string s)
		{
			RichTextBox txt = new RichTextBox();
			txt.Multiline = true;
			txt.WordWrap = true;
			txt.ReadOnly = true;
			txt.BorderStyle = BorderStyle.None;
			txt.Text = s;
			txt.Dock = DockStyle.Fill;
			txt.LinkClicked += delegate (object o, LinkClickedEventArgs a) { 
				Process.Start(a.LinkText); // launch brower
			};
			txt.SelectionChanged += delegate (object o, EventArgs a) {
				if (txt.SelectedText != String.Empty) {
					Clipboard.SetDataObject(txt.SelectedText);
				}
			};
			return txt;
		}
		
		public static RichTextBox GetRichTextBox(Controller.Id id, string s)
		{
			RichTextBox txt = GetRichTextBoxAnon(s);
			Controller.RegisterControl(id, txt);	// register control
			return txt;
		}
		
		public static RichTextBox GetRichTextBoxSingle(Controller.Id id, string s)
		{
			RichTextBox txt = new RichTextBox();
			txt.Multiline = false;
			txt.ReadOnly = true;
			txt.BorderStyle = BorderStyle.None;
			txt.Text = s;
			txt.Dock = DockStyle.Fill;
			Controller.RegisterControl(id, txt);	// register control
			return txt;
		}
		
		public static TextBox GetTextBoxRW(Controller.Id id, string s)
		{
			TextBox textbox = new TextBox();
			textbox.Text = s;
			textbox.Anchor = AnchorStyles.Left;
			Controller.RegisterControl(id, textbox);	// register control
			return textbox;
		}
		
		public static TextBox GetTextBoxRO(Controller.Id id, string s)
		{
			TextBox textbox = GetTextBoxRW(id, s);
			textbox.ReadOnly = true;
			return textbox;
		}
		
		public static TextBox GetTextBoxROPlain(Controller.Id id, string s)
		{
			TextBox textbox = GetTextBoxRO(id, s);
			textbox.BorderStyle = BorderStyle.None;
			return textbox;
		}
		
		public static TextBox GetTextBoxAnon(string s)
		{
			TextBox textbox = new TextBox();
			textbox.ReadOnly = true;
			textbox.Multiline = true;
			textbox.WordWrap = true;
			textbox.TabStop = false;
			textbox.Text = s;
			textbox.Dock = DockStyle.Fill;
			textbox.BorderStyle = BorderStyle.None;
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
