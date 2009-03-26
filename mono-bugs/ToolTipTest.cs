using System;
using System.Collections.Generic;
using System.Windows.Forms;

class ToolTipTest
{	
	public static NumericUpDown GetNumericUpDown(int min, int max)
	{
		NumericUpDown num = new NumericUpDown();
		num.Minimum = min;
		num.Maximum = max;
		return num;
	}

	public static ToolTip GetToolTipInfo(string title)
	{
		ToolTip tooltip = new ToolTip();
		tooltip.ToolTipTitle = title;
		tooltip.ToolTipIcon = ToolTipIcon.Info;
		return tooltip;
	}

	[STAThread]
	public static void Main()
	{
		Form form = new Form();

		Control numericupdown = GetNumericUpDown(5, 15);
		ToolTip tooltip_num = GetToolTipInfo("Numeric Title");
		tooltip_num.SetToolTip(numericupdown, "Numeric Tip");
		form.Controls.Add(numericupdown);

		Application.Run(form);
	}
}

/*
 * gmcs -r:System.Windows.Forms ToolTipTest.cs
 *
 * Aside from the standard tooltip, which is just a message, there is a
 * fancier tooltip popup with a title in bold and an icon.
 *
 * linux:mono r130111
 * plain tooltip
 *
 * linux:mono 1.9.1
 * plain tooltip
 *
 * winxp:mono 2.2
 * plain tooltip
 *
 * winxp:.net 3.5
 * fancier tooltip
 */
