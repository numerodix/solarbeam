// Copyright (c) 2009 Martin Matusiak <numerodix@gmail.com>
// Licensed under the GNU Public License, version 3.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibSolar.Types;
using LibSolar.Util;

namespace SolarbeamGui
{
	/**
	 * Represents details pane.
	 */
	sealed class GuiDetails : Panel
	{
		public GuiDetails()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.AutoScroll = true;
			this.Dock = DockStyle.Fill;
						
			GroupBox parameters = new GroupBox();
			parameters.Text = "Parameters";
			parameters.Dock = DockStyle.Fill;
			parameters.Controls.Add(GetParameters());
			
			this.Controls.Add(parameters);
		}
		
		private Control GetParameters()
		{
			TableLayoutPanel layout = Widgets.GetTableLayoutPanel(4, 1,
			                                                      0, 0);
			
			Control loc = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Location"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_LOCATION, "Equator")},
					new float[] {5F, 2F, 25F});
			
			Control pos = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Position"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_POSITION, "Equator")},
					new float[] {5F, 2F, 25F});
			
			Control tz = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Timezone"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_TIMEZONE, "Equator")},
					new float[] {5F, 2F, 25F});
			
			Control dt = Widgets.GetLaidOut(
					new Control[] {
					Widgets.GetLabelAnon("Date"),
					Widgets.GetLabelAnon(":"),
					Widgets.GetTextBoxROPlain(Controller.Id.DETAIL_DATETIME, "Equator")},
					new float[] {5F, 2F, 25F});			
			
			layout.Controls.AddRange(new Control[] {
				loc,
				pos,
				tz,
				dt,
			});
			
			for (int i = 0; i < layout.Controls.Count; i++) {
				layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
			}
	
			return layout;
		}
	}
}
