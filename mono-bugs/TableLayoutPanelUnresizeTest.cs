using System;
using System.Drawing;
using System.Windows.Forms;

class TableLayoutPanelUnresizeTest
{	
	public static Label GetLabel(string s, Color color)
	{
		Label label = new Label();
		label.BackColor = color;
		label.Text = s;
		label.AutoSize = true;
		label.Anchor = AnchorStyles.Left;
		return label;
	}

	public static TabControl GetTabControl(Control[] controls,
			string[] labels)
	{
		TabControl tabs = new TabControl();
		tabs.Dock = DockStyle.Fill;

		for (int i=0; i<controls.Length; i++) {
			TabPage tab = new TabPage();
			tab.Dock = DockStyle.Fill;
			tab.Text = labels[i];
			controls[i].Dock = DockStyle.Fill;
			tab.Controls.Add(controls[i]);
			tabs.Controls.Add(tab);
		}

		return tabs;
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

	public static TableLayoutPanel GetLaidOut(Control[] controls, string[] widths)
	{
		TableLayoutPanel layout = new TableLayoutPanel();
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
			c.Dock = DockStyle.Fill;
			layout.Controls.Add(c);
		}
		return layout;
	}

	[STAThread]
	public static void Main()
	{
		Form form = new Form();

		TabControl tabs = GetTabControl(
			new Control[] {
				GetLabel("1", Color.Red),
				GetLabel("2", Color.Blue)},
			new string[] {"uno", "due"});

		Control sidebyside = GetLaidOut(
			new Control[] {
				GetLabel("Left", Color.Green),
				tabs},
			new string[] {"100", "100%"});

		Control main = GetStacked(
			new Control[] {
				GetLabel("Top", Color.Yellow),
				sidebyside,
				GetLabel("Bottom", Color.Navy)},
			new string[] {"30", "100%", "50"});

		form.Controls.Add(main);

		Application.Run(form);
	}
}

/*
 * gmcs -r:System.Windows.Forms -r:System.Drawing TableLayoutPanelUnresizeTest.cs
 *
 * Maximize the window, then restore size again. The TabControl fails to down
 * size accordingly, both length and width wise.
 *
 * linux:mono r132681
 * bug
 *
 * linux:mono 2.0.1
 * bug
 *
 * winxp:mono 2.2
 * bug
 *
 * winxp:.net 3.5
 * working as expected
 */
