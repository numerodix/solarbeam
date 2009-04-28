using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

class ComboInputTest
{	
	public static string Run(string bin, string args)
	{
		string output = null;
		try {
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = bin;
			p.StartInfo.Arguments = args;
			p.Start();
			output = p.StandardOutput.ReadToEnd().Trim();
			p.WaitForExit();
		} catch {}
		return output;
	}

	public static TableLayoutPanel GetStacked(Control[] controls, string[] widths)
	{
		TableLayoutPanel layout = new TableLayoutPanel();
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

	public static RichTextBox GetRichTextBoxAnon(string s)
	{
		RichTextBox txt = new RichTextBox();
		txt.Multiline = true;
		txt.WordWrap = true;
		txt.Text = s;
		txt.Dock = DockStyle.Fill;
		return txt;
	}

	public static ComboBox GetComboBox(List<string> ss)
	{
		ComboBox combo = new ComboBox();
		foreach (string s in ss) {
			combo.Items.Add(s);
		}
		combo.DropDownStyle = ComboBoxStyle.DropDownList;
		combo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
		combo.DropDownHeight = 180;
		return combo;
	}

	public static ComboBox GetComboBoxInputable(List<string> ss)
	{
		ComboBox combo = GetComboBox(ss);
		combo.DropDownStyle = ComboBoxStyle.DropDown;
		combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
		combo.AutoCompleteSource = AutoCompleteSource.ListItems;
		combo.Sorted = true;
		return combo;
	}

	public static ComboBox GetComboBoxFilled()
	{
		List<string> items = new List<string>();
		for (int i=0; i<26; i++) {
			for (int j=0; j<26; j++) {
				items.Add(String.Format("{0}{1}fff", (char) (97+i), (char) (97+j)));
			}
		}
		return GetComboBoxInputable(items);
	}

	public static TextBox GetTextBox()
	{
		TextBox textbox = new TextBox();
		return textbox;
	}

	[STAThread]
	public static void Main()
	{
		RichTextBox label = GetRichTextBoxAnon(Run("mono", "-V"));
		label.TabStop = false;
		ComboBox combo = GetComboBoxFilled();
		Control text = GetTextBox();

		Control main = GetStacked(
			new Control[] {
				label,
				combo,
				text},
			new string[] {"150", "30", "30"});

		Form form = new Form();
		form.Size = new Size(450, 240);
		form.Controls.Add(main);

		Console.WriteLine("start");
		combo.SelectedValueChanged += delegate {
			string selected = (string) combo.Items[combo.SelectedIndex];
			text.Text = selected;
		};

		Application.Run(form);
	}
}

/*
 * gmcs -r:System.Windows.Forms -r:System.Drawing ComboInputTest.cs
 *
 * 1. Start typing in the combo box.
 * 2. Once a suggestion appears, hit Tab or Enter. This signals that the top item in
 * the suggestion list is to be selected.
 * 3. Selected item appears in text box below combo box.
 *
 * linux:mono r132804
 * selected item not written into text box
 *
 * winxp:.net 3.5
 * working as expected
 */
