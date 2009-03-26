using System;
using System.Collections.Generic;
using System.Windows.Forms;

class ComboInputTest
{	
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

	[STAThread]
	public static void Main()
	{
		Form form = new Form();
		List<string> items = new List<string>();
		for (int i=0; i<26; i++) {
			for (int j=0; j<26; j++) {
				items.Add(String.Format("{0}{1}fff", (char) (97+i), (char) (97+j)));
			}
		}

		Control combo = GetComboBoxInputable(items);
		form.Controls.Add(combo);

		Application.Run(form);
	}
}

/*
 * gmcs -r:System.Windows.Forms ComboInputTest.cs
 *
 * When you start typing in the box without clicking the pull down menu
 * that shows all the items, there is a small popup box that appears
 * containing the choices that match your prefix so far.
 *
 * linux:mono r130111
 * missing popup
 *
 * linux:mono 1.9.1
 * missing popup
 *
 * osx:mono 2.2
 * working as expected
 *
 * winxp:mono 2.2
 * working as expected
 *
 * winxp:.net 3.5
 * working as expected
 */
