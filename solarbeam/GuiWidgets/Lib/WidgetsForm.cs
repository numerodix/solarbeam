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
	partial class Widgets
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
			button.Image = new Bitmap(Controller.AsmInfo.GetResource(img));
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
			button.Image = new Bitmap(Controller.AsmInfo.GetResource(img));
//			button.MinimumSize = button.Image.Size;
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 0;
			button.TabStop = false;
			Controller.RegisterControl(id, button);	// register control
			return button;
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
		
		public static Label GetLabel(string s)
		{
			Label label = new Label();
			label.Text = s;
			label.AutoSize = true;
			label.Anchor = AnchorStyles.Left;
			return label;
		}
		
		public static Label GetLabelImage(Controller.Id id, string img)
		{
			Label label = new Label();
			label.Image = new Bitmap(Controller.AsmInfo.GetResource(img));
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
		
		public static RichTextBox GetRichTextBox(string s)
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
					Console.WriteLine(Clipboard.GetText());
				}
			};
			return txt;
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
