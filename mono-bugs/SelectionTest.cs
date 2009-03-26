using System;
using System.Windows.Forms;

class SelectionTest
{	
	public static RichTextBox GetRichTextBox(string s)
	{
		RichTextBox txt = new RichTextBox();
		txt.Multiline = true;
		txt.WordWrap = true;
		txt.ReadOnly = true;
		txt.BorderStyle = BorderStyle.None;
		txt.Text = s;
		txt.Dock = DockStyle.Fill;
		txt.SelectionChanged += delegate (object o, EventArgs a) {
			if (txt.SelectedText != String.Empty) {
				Clipboard.SetDataObject(txt.SelectedText);
				Console.WriteLine(Clipboard.GetText());
			}
		};
		return txt;
	}

	[STAThread]
	public static void Main()
	{
		Form form = new Form();
		form.Controls.Add(GetRichTextBox("kasddddd sfasd asd\n fasdfas"));

		Application.Run(form);
	}
}

/*
 * gmcs -r:System.Windows.Forms SelectionTest.cs
 *
 * linux:mono r130111
 * output in console as expected
 * clipboard not working, neither middle click nor paste
 *
 * linux:mono 1.9.1
 * no output in console
 * clipboard not working, neither middle click nor paste
 *
 * winxp:mono 2.2
 * output in console as expected
 * clipboard working as expected
 *
 * winxp:.net 3.5
 * output in console as expected
 * clipboard working as expected
 */
